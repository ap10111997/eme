using EMessagesNew2.Interfaces;
using EMessagesNew2.Models.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Responses.chats
{
    public class GetChatsResponse : IResponse
    {
        public List<ExtChat> Chats { get; set; }
    }
}
