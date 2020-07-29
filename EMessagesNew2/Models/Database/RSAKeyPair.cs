using EMessagesNew2.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Database
{
    public class RSAKeyPair
    {
        public int Id { get; set; }
        public string PublicKey { get; set; }
        public virtual AESEncryptedData EncryptedPrivateKey { get; set; }
        public virtual RSAKeyPair LastKeyPair { get; set; }
        public string LastPublicKeySign { get; set; }
    }
}
