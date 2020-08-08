using EMessagesNew2.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Requests.users
{
    public class SetEncryptedPublicKeyRequestItem
    {
        public string UserId { get; set; }
        public AESEncryptedData EncryptedPublicKey { get; set; }
    }
}
