using EMessagesNew2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Responses.auth
{
    public class UserCheckLoginResponse : IResponse
    {
        public bool CheckSuccess { get; set; }
    }
}
