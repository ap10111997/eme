using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Configurations
{
    /// <summary>
    /// Представляет конфигурацию роутов путей на сервере
    /// </summary>
    public class FileRoutesConfig
    {
        /// <summary>
        /// Путь к общим
        /// </summary>
        public FileRoute Images { get; set; }
        
        /// <summary>
        /// Путь к изображениям пользователя
        /// </summary>
        public FileRoute UserImages { get; set; }
        
        /// <summary>
        /// Возвращает массив путей
        /// </summary>
        /// <returns>Массив путей</returns>
        public FileRoute[] GetFileRoutes()
        {
            return new FileRoute[]
            {
                Images, UserImages
            };
        }
    }
    
    /// <summary>
    /// Представляет соответствие путя обращения к серверу реальной папке на сервере
    /// </summary>
    public class FileRoute
    {
        /// <summary>
        /// Физический путь на сервере
        /// </summary>
        public string Route { get; set; }
        
        /// <summary>
        /// Адрес обращения
        /// </summary>
        public string Path { get; set; }
    }
}
