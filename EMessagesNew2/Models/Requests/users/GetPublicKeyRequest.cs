using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Requests.users
{
    public class GetPublicKeyRequest
    {
        public List<string> UserIds { get; set; }
    }
}
