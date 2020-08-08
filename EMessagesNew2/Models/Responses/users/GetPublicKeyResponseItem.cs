using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Responses.users
{
    public class GetPublicKeyResponseItem
    {
        public string UserId { get; set; }
        public string PublicKey { get; set; }
        public int KeyId { get; set; }
    }
}
