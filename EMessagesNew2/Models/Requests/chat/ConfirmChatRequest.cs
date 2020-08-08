using EMessagesNew2.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Requests.chat
{
    public class ConfirmChatRequest
    {
        public string ChatId { get; set; }
        public byte[] UserChatKeySign { get; set; }
        public byte[] SessionChatKeySign { get; set; }
        public AESEncryptedData EncryptedChatKey { get; set; }
        public AESEncryptedData EncryptedTitle { get; set; }
    }
}
