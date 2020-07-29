using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Configurations
{
    /// <summary>
    /// Прудставляет общую конфигурацию сервера
    /// </summary>
    public class ServerConfiguration
    {
        /// <summary>
        /// Конфигурация чатов
        /// </summary>
        public ChatsConfig Chats { get; set; }
        
        /// <summary>
        /// Конфигурация контактов
        /// </summary>
        public ContactsConfig Contacts { get; set; }
        
        /// <summary>
        /// Конфигурация путей файлов
        /// </summary>
        public FileRoutesConfig FileRoutes { get; set; }
        
        /// <summary>
        /// Основные параметры
        /// </summary>
        public GeneralConfig General { get; set; }
        
        /// <summary>
        /// Конфигурация пользователей
        /// </summary>
        public UsersConfig Users { get; set; }
        
        /// <summary>
        /// Конфигурация поключений к БД
        /// </summary>
        public ConnectionStringsConfiguration ConnectionStrings { get; set; }
    }
}
