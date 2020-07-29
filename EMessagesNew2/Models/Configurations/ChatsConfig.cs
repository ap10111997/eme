using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Configurations
{
    /// <summary>
    /// Представляет конфигурацию чатов
    /// </summary>
    public class ChatsConfig
    {
        /// <summary>
        /// Размеры отправляемых частичных данных
        /// </summary>
        public ChatsPartsSize PartsSize { get; set; }
    }

    /// <summary>
    /// Представляет реализацию параметров частичных данных для чатов
    /// </summary>
    public class ChatsPartsSize
    {
        /// <summary>
        /// Количесвто сообщений в ответе
        /// </summary>
        public int MessagesInPart { get; set; }

        /// <summary>
        /// Количество чатов в ответе
        /// </summary>
        public int ChatsInPart { get; set; }
    }
}
