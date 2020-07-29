using EMessagesNew2.Interfaces;
using EMessagesNew2.Models.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Responses.users
{
    public class SearchContactsResponse : IResponse
    {
        public List<ExtUser> Users { get; set; }
        public List<string> FriendsId { get; set; }
    }
}
