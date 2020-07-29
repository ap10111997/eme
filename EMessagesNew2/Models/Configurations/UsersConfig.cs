using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Configurations
{
    /// <summary>
    /// Представляет конфигурацию пользователей
    /// </summary>
    public class UsersConfig
    {
        /// <summary>
        /// Пути хранения данных на сервере
        /// </summary>
        public Paths Paths { get; set; }
        public UsersPartsSize PartsSize { get; set; }
        public long TokenExpirationTime { get; set; }
    }

    /// <summary>
    /// Представляет пути хранения данных на сервере
    /// </summary>
    public class Paths
    {
        /// <summary>
        /// Главная папка хранения пользовательских данных
        /// </summary>
        public string MainPath { get; set; }

        /// <summary>
        /// Путь до изображений
        /// </summary>
        public string ImagesPath { get; set; }

        /// <summary>
        /// Путь до файлов
        /// </summary>
        public string FilesPath { get; set; }
    }
    
    /// <summary>
    /// Представляет конфигурацию пагинации для пользователей
    /// </summary>
    public class UsersPartsSize
    {
        /// <summary>
        /// Размер страницы предпросмотра изображений
        /// </summary>
        public int PreviewImagesPartSize { get; set; }
    }
}
