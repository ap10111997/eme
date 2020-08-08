using EMessagesNew2.Interfaces;
using EMessagesNew2.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Responses.auth
{
    public class UserLoginResponse : IResponse
    {
        public string UserSalt { get; set; }
        public string UserIV { get; set; }
        public string IdentificationWord { get; set; }
        public string EncryptionKey { get; set; }
        public string HashKey { get; set; }
        public AESEncryptedData EncryptedPrivateKey { get; set; }
        public string PublicKey { get; set; }
        public int KeyId { get; set; }
    }
}
