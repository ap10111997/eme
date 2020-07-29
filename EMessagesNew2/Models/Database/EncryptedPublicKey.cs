using EMessagesNew2.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Database
{
    public class EncryptedPublicKey
    {
        public int Id { get; set; }
        public virtual AESEncryptedData EncryptedPubicKey { get; set; }
        public virtual RSAKeyPair RSAKeyPair { get; set; }
    }
}
