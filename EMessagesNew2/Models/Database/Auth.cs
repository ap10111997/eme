using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Database
{
    public class Auth
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expire { get; set; }
        public string LocalDataEncryptionKey { get; set; }
    }
}
