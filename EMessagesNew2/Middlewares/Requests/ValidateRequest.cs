using EMessagesNew2.Classes.Database;
using EMessagesNew2.Classes.ServerResponses;
using EMessagesNew2.Extensions;
using EMessagesNew2.Models.Database;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Middlewares.Requests
{
    /// <summary>
    /// Реализует валидацию запроса по пути обращения и правам пользователя
    /// </summary>
    public class ValidateRequest
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Создает экземпляр класса
        /// </summary>
        /// <param name="next">Следущий обработчик</param>
        public ValidateRequest(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Выполняет валидацию запроса. ПРи неуспешном завершении отправляет ответ с ошибкой и прерывает конвейер
        /// </summary>
        /// <param name="context">Контекст запроса</param>
        /// <param name="database">Контекст подключения к базе данных</param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context, MainContext database)
        {
            string[] path = context.Request.Path.Value.Split('/');


            if (path.Length > 1 && (path[1] == "api" || path[1] == "sockets"))
            {
                if (path.Length > 2 && path[2] != "auth")
                {
                    User user = context.GetUser();
                    if (user == null)
                    {
                        context.Response.StatusCode = 403;
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(MainResponse.GetError(Enums.RequestError.UserNotFound))).ConfigureAwait(false);
                        return;
                    }
                }
            }

            await _next.Invoke(context).ConfigureAwait(false);
        }
    }
}
