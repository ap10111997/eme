using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Enums
{
    /// <summary>
    /// Представляет статус пользователя
    /// </summary>
    public enum UserStates
    {
        /// <summary>
        /// Не на сайте
        /// </summary>
        Offline,

        /// <summary>
        /// На этапе ввода секретного ключа
        /// </summary>
        SecretKey,

        /// <summary>
        /// В сети
        /// </summary>
        Online
    }
}
