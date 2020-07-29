using EMessagesNew2.Models.General;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Database
{
    public class ChatTitle
    {
        public int Id { get; set; }
        public virtual AESEncryptedData EncryptetChatTitle { get; set; }
        public virtual KeySession KeySession { get; set; }
    }
}
