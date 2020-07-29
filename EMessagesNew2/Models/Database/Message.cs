using EMessagesNew2.Enums;
using EMessagesNew2.Models.External;
using EMessagesNew2.Models.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Database
{
    public class Message
    {
        [MaxLength(40)]
        public string Id { get; set; }
        public MessageStatus Status { get; set; }
        public virtual AESEncryptedData EncryptedMessage { get; set; }
        public DateTime Date { get; set; }
        public virtual Chat Chat { get; set; }
        public virtual User Sender { get; set; }
    }
}
