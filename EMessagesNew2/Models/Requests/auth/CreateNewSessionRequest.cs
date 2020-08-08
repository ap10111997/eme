using EMessagesNew2.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Requests.auth
{
    public class CreateNewSessionRequest
    {
        public string IdentificationWord { get; set; }
        public string PublicKey { get; set; }
        public AESEncryptedData EncryptedPrivateKey { get; set; }
    }
}
