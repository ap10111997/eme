using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Enums
{
    /// <summary>
    /// Представляет тип ошибки запроса
    /// </summary>
    public enum RequestError
    {
        /// <summary>
        /// Пользователь с таким логином существует
        /// </summary>
        LoginExists, 
        /// <summary>
        /// Пользователь не найден
        /// </summary>
        UserNotFound, 
        /// <summary>
        /// Достигнут лимит использования секретных ключей
        /// </summary>
        NoKeysAvailable,
        /// <summary>
        /// Переданный ключ не найден
        /// </summary>
        KeyNotFound,
        /// <summary>
        /// Чат не найден
        /// </summary>
        ChatNotFound,
        /// <summary>
        /// Зашифрованный ключ чата не найден для этой пары ключей
        /// </summary>
        ChatKeyNotFound,
        /// <summary>
        /// Вход в чат был произведен ранее
        /// </summary>
        ChatAlreadyConfirmed
    }
}
