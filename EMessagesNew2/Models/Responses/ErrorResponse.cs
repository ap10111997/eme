using EMessagesNew2.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Responses
{
    public class ErrorResponse
    {
        public string Description { get; set; }
        public RequestError Code { get; set; }
    }
}
