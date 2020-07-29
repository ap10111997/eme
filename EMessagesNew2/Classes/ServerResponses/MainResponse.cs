using EMessagesNew2.Classes.Utilities;
using EMessagesNew2.Enums;
using EMessagesNew2.Interfaces;
using EMessagesNew2.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Classes.ServerResponses
{
    /// <summary>
    /// Представляет объект для стандартного ответа сервера
    /// </summary>
    public class MainResponse
    {
        /// <summary>
        /// Успешность выполнения запроса
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Ошибка с описанием проблемы
        /// </summary>
        public ErrorResponse Error { get; set; }

        /// <summary>
        /// Результат выполнения запроса
        /// </summary>
        public IResponse Data { get; set; }

        /// <summary>
        /// Возвращает успешную версию ответа сервера с данными
        /// </summary>
        /// <param name="response">Класс, реализующий интерфейс ответа сервера</param>
        /// <returns>Экземпляр успешного ответа сервера</returns>
        public static MainResponse GetSuccess(IResponse response)
        {
            return new MainResponse()
            {
                Success = true,
                Error = null,
                Data = response
            };
        }

        /// <summary>
        /// Возвращяет успешную версию ответа сервера
        /// </summary>
        /// <returns>Экземпляр успешного ответа сервера</returns>
        public static MainResponse GetSuccess()
        {
            return new MainResponse()
            {
                Success = true,
                Error = null,
                Data = null
            };
        }

        /// <summary>
        /// Возвращает экземпляр ответа сервера с ошибкой
        /// </summary>
        /// <param name="error">Тип ошибки</param>
        /// <returns>Экземпляр ошибочного ответа сервера</returns>
        public static MainResponse GetError(RequestError error)
        {
            return new MainResponse()
            {
                Success = false,
                Data = null,
                Error = ResponseUtilities.GetErrorResponse(error)
            };
        }
    }
}
