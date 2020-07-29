using EMessagesNew2.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Database
{
    public class FileData
    {
        public int Id { get; set; }
        public string PhysicalName { get; set; }
        public DateTime UploadDate { get; set; }
        public string FileName { get; set; }
        public FileType Type { get; set; }
        public string Thumbnail { get; set; }
    }
}
