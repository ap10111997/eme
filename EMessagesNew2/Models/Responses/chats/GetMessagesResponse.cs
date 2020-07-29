using EMessagesNew2.Interfaces;
using EMessagesNew2.Models.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Responses.chats
{
    public class GetMessagesResponse : IResponse
    {
        public List<ExtMessage> Messages { get; set; }
    }
}
