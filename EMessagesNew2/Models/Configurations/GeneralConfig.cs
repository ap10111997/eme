using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Configurations
{
    /// <summary>
    /// Представляет общую конфигурацию приложения
    /// </summary>
    public class GeneralConfig
    {
        /// <summary>
        /// Длина параметра HashKey
        /// </summary>
        public int HashKeyLength { get; set; }

        /// <summary>
        /// Адрес сервера
        /// </summary>
        public string ServerPath { get; set; }
    }
}
