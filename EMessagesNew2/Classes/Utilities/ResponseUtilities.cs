using EMessagesNew2.Classes.Database;
using EMessagesNew2.Enums;
using EMessagesNew2.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Classes.Utilities
{
    /// <summary>
    /// Представляет функции для обработки ответа
    /// </summary>
    public static class ResponseUtilities
    {
        private static readonly ErrorResponse[] errorResponses = new ErrorResponse[]
        {
            new ErrorResponse()
            {
                Code = RequestError.LoginExists,
                Description = "Пользователь с таким логином уже существует"
            },
            new ErrorResponse()
            {
                Code = RequestError.UserNotFound,
                Description = "Пользователь с таким логином не найден"
            },
            new ErrorResponse()
            {
                Code = RequestError.NoKeysAvailable,
                Description = "Количество ключей превысило максимальное"
            },
            new ErrorResponse()
            {
                Code = RequestError.KeyNotFound,
                Description = "Выбранный ключ не найден"
            },
            new ErrorResponse()
            {
                Code = RequestError.ChatNotFound,
                Description = "Чат не найден"
            },
            new ErrorResponse()
            {
                Code = RequestError.ChatKeyNotFound,
                Description = "Зашифрованный ключ чата не найден для этой пары ключей"
            },
            new ErrorResponse()
            {
                Code = RequestError.ChatAlreadyConfirmed,
                Description = "Вход в чат был произведен ранее"
            }
        };

        /// <summary>
        /// Возвращает экземпляр ошибки сервера заданного типа
        /// </summary>
        /// <param name="error">Тип ошибки</param>
        /// <returns>Экземпляр ошибки</returns>
        public static ErrorResponse GetErrorResponse(RequestError error)
        {
            return errorResponses[(int)error];
        }
    }
}
