using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Enums
{
    /// <summary>
    /// Представляет статус сообщения
    /// </summary>
    public enum MessageStatus
    {
        /// <summary>
        /// Не зашифровано секретным ключом
        /// </summary>
        Unencrypted,
        /// <summary>
        /// Зашифровано секретным ключом
        /// </summary>
        Encrypted,
        /// <summary>
        /// Прочитано
        /// </summary>
        Read
    }
}
