using EMessagesNew2.Models.Database;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Extensions
{
    /// <summary>
    /// Представляет расширения HTTP контекста
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Устанавливает пользователя
        /// </summary>
        /// <param name="context"></param>
        /// <param name="user">Модель пользователя</param>
        public static void SetUser(this HttpContext context, User user)
        {
            context.Items.Add("user", user);
        }

        /// <summary>
        /// Возвращает пользователя из контекста
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Модель пользователя или null</returns>
        public static User GetUser(this HttpContext context)
        {
            return context.Items.ContainsKey("user") ? (User)context.Items["user"] : null;
        }
    }
}
