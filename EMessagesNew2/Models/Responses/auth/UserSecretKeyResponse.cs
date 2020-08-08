using EMessagesNew2.Interfaces;
using EMessagesNew2.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Responses.auth
{
    public class UserSecretKeyResponse : IResponse
    {
        public bool UnknownKey { get; set; }
        public string PublicKey { get; set; }
        public AESEncryptedData EncryptedPrivateKey { get; set; }
        public int KeyId { get; set; }
    }
}
