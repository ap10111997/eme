using EMessagesNew2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Responses.users
{
    public class GetPublicKeyResponse : IResponse
    {
        public List<GetPublicKeyResponseItem> PublicKeys { get; set; }
    }
}
