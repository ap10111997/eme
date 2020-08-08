using EMessagesNew2.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Database
{
    public class KeySession
    {
        public int Id { get; set; }
        public string VerificationData { get; set; }
        public virtual RSAKeyPair RSAKeyPair { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
