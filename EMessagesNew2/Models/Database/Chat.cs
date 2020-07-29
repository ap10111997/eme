using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Database
{
    public class Chat
    {
        [MaxLength(20)]
        public string Id { get; set; }
        public virtual List<EncryptionChatData> EncryptionChatDatas { get; set; }
        public virtual List<Message> Messages { get; set; }
        public DateTime LastMessageDate { get; set; }
        public string LastMessageId { get; set; }
        public string Image { get; set; }
    }
}
