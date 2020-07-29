using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Requests.users
{
    public class SearchContactsRequest
    {
        public bool IsNewContacts { get; set; }
        public string SearchKey { get; set; }

        [Range(1, int.MaxValue)]
        public int Part { get; set; }
    }
}
