using EMessagesNew2.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Requests.chat
{
    public class CreateChatEncryptedPayload
    {
        public int KeyId { get; set; }
        public byte[] EncryptedChatKey { get; set; }
        public byte[] PayLoad { get; set; }
    }
}
