using EMessagesNew2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Responses.users
{
    public class GetCredentialsReqponse : IResponse
    {
        public string Login { get; set; }
        public string Email { get; set; }
        public string Alias { get; set; }
    }
}
