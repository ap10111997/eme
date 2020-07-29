using EMessagesNew2.Enums;
using EMessagesNew2.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.External
{
    /// <summary>
    /// Представляет внешнюю модель данных файла
    /// </summary>
    public class ExtFileData
    {
        public string Path { get; set; }
        public string FileName { get; set; }
        public string Thumbnail { get; set; }

        public static explicit operator ExtFileData(FileData fileData)
        {
            if (fileData == null)
            {
                return null;
            }

            return new ExtFileData()
            {
                FileName = fileData.FileName,
                Path = fileData.PhysicalName,
                Thumbnail = fileData.Thumbnail
            };
        }

        public void SetPath(string path)
        {
            if (Thumbnail != "")
            {
                Thumbnail = path + "/" + Thumbnail;
            }
            if (Path != "")
            {
                Path = path + "/" + Path;
            }
        }
    }
}
