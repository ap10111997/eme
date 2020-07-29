using EMessagesNew2.Interfaces;
using EMessagesNew2.Models.Database;
using EMessagesNew2.Models.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Responses.users
{
    public class UserDataResponse : IResponse
    {
        public ExtUser UserData { get; set; }
    }
}
