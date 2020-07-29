using EMessagesNew2.Middlewares.Requests;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Extensions
{
    /// <summary>
    /// Представляет расширения <see cref="IApplicationBuilderExtensions"/>
    /// </summary>
    public static class IApplicationBuilderExtensions
    {
        /// <summary>
        /// Использование проверки и валидации пользователя при приходе запроса
        /// </summary>
        public static IApplicationBuilder UseRequestProcessing(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<AcceptRequest>();
            return builder.UseMiddleware<ValidateRequest>();
        }
    }
}
