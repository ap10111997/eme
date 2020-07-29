using EMessagesNew2.Classes.Database;
using EMessagesNew2.Classes.Utilities;
using EMessagesNew2.Extensions;
using EMessagesNew2.Models.Database;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EMessagesNew2.Middlewares.Requests
{
    /// <summary>
    /// Представляет реализацию установки пользователя в <see cref="HttpContext"/> по полученному токену в запросе
    /// </summary>
    public class AcceptRequest
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Создает экземпляр класса
        /// </summary>
        /// <param name="next">Следующий обработчик</param>
        public AcceptRequest(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Выполняет установку пользователя в <see cref="HttpContext"/> при переданном токене
        /// </summary>
        /// <param name="context">Контекст запроса</param>
        /// <param name="database">Контекст подключения к базе данных</param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context, MainContext database)
        {
            string token = null;
            if (context.Request.Cookies.Count > 0)
            {
                if (context.Request.Cookies.Any(p => p.Key == "auth-token")) {
                    token = context.Request.Cookies["auth-token"];
                }
            }

            if (token != null && token.Trim() != "")
            {
                User user = database.Users.FirstOrDefault(p => p.Auth.Any(c => c.Token == token));
                if (user != null)
                {
                    context.SetUser(user);
                }
            }

            await _next.Invoke(context).ConfigureAwait(false);
        }
    }
}
