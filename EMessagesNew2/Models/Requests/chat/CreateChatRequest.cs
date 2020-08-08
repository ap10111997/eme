using EMessagesNew2.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Requests.chat
{
    public class CreateChatRequest
    {
        public AESEncryptedData EncryptedTitle { get; set; }
        public List<CreateChatEncryptedPayload> EncryptedPayload { get; set; }
        public AESEncryptedData AESEncryptedChatKey { get; set; }
        public byte[] ChatKeySign { get; set; }
    }
}
