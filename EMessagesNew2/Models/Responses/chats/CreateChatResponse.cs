using EMessagesNew2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Responses.chats
{
    public class CreateChatResponse : IResponse
    {
        public string ChatId { get; set; }
    }
}
