using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using EMessagesNew2.Classes.Database;
using Microsoft.Extensions.FileProviders;
using System.IO;
using EMessagesNew2.Classes.Hubs;
using Microsoft.AspNetCore.SignalR;
using EMessagesNew2.Classes.Sockets;
using EMessagesNew2.Models.Configurations;
using EMessagesNew2.Extensions;

namespace EMessagesNew2
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            try
            {
                IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(env.ContentRootPath);
                builder.AddYamlFile("appsettings.yml", true, true);
                builder.AddYamlFile($"appsettings.{env.EnvironmentName}.yml", optional: true, true);
                Configuration = builder.Build();
                Env = env;
            }
            catch (Exception e)
            {
                File.AppendAllText(env.ContentRootPath + "\\errors.log", e.Message);
                File.AppendAllText(env.ContentRootPath + "\\errors.log", e.StackTrace);
            }
        }

        public IConfigurationRoot Configuration { get; }
        private IWebHostEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                string connection = Configuration.GetConnectionString("DefaultConnection");
                services.AddDbContext<MainContext>(options =>
                {
                    options.UseLazyLoadingProxies();
                    options.UseMySql(connection);
                });

                services.AddControllers().AddNewtonsoftJson();
                services.AddCors();
                services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
                services.AddSignalR();

                ServerConfiguration serverConfiguration = Configuration.Get<ServerConfiguration>();

                if (!Directory.Exists(Env.ContentRootPath + serverConfiguration.Users.Paths.MainPath))
                {
                    Directory.CreateDirectory(Env.ContentRootPath + serverConfiguration.Users.Paths.MainPath);
                }

                serverConfiguration.Users.Paths.MainPath = Env.ContentRootPath + serverConfiguration.Users.Paths.MainPath;

                services.AddSingleton(serverConfiguration);
            }
            catch (Exception e)
            {
                File.AppendAllText(Env.ContentRootPath + "\\errors.log", e.Message);
                File.AppendAllText(Env.ContentRootPath + "\\errors.log", e.StackTrace);
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ServerConfiguration serverConfig)
        {
            try
            {
                app.UseDirectoryBrowser();
                app.UseDefaultFiles();
                app.UseDeveloperExceptionPage();

                app.UseCors((builder) =>
                {
                    builder.WithOrigins(new string[] { "http://localhost:8080", "http://localhost:8081", "http://localhost:8082", "http://localhost:8083" });
                    builder.AllowCredentials();
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                });

                FileRoute[] fileRoutes = serverConfig.FileRoutes.GetFileRoutes();

                for (int i = 0; i < fileRoutes.Length; i++)
                {
                    string path = Path.Combine(env.WebRootPath ?? env.ContentRootPath, fileRoutes[i].Path);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    app.UseStaticFiles(new StaticFileOptions
                    {
                        FileProvider = new PhysicalFileProvider(path),
                        RequestPath = fileRoutes[i].Route
                    });
                }

                /*app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(Path.Combine(env.WebRootPath, "App_Data\\StaticFiles")),
                    RequestPath = "/content"
                });*/

                app.UseStaticFiles();

                app.UseRequestProcessing();

                app.UseRouting();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");
                    endpoints.MapHub<ChatHub>("/sockets/chat");
                    endpoints.MapHub<EventsHub>("/sockets/events");
                });
            }
            catch (Exception e)
            {
                File.AppendAllText(Env.ContentRootPath + "\\errors.log", e.Message);
                File.AppendAllText(Env.ContentRootPath + "\\errors.log", e.StackTrace);
            }
        }
    }
}
