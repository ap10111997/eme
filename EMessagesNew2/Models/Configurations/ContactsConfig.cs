
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Configurations
{
    /// <summary>
    /// Предстваляет конфигурацию контактов
    /// </summary>
    public class ContactsConfig
    {
        /// <summary>
        /// Размеры отправляемых частичных данных
        /// </summary>
        public ContactsPartsSize PartsSize { get; set; }
    }

    /// <summary>
    /// Представляет реализацию параметров частичных данных для контактов
    /// </summary>
    public class ContactsPartsSize
    {
        /// <summary>
        /// Количество контактов в ответе
        /// </summary>
        public int ContactsPerPart { get; set; }

        /// <summary>
        /// Количество результатов поиска в ответе
        /// </summary>
        public int SearchResultsPerPart { get; set; }
    }
}
