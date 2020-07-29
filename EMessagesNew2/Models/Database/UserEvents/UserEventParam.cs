using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Database
{
    public class UserEventParam
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string Alias { get; set; }
        public virtual UserEvent UserEvent { get; set; }
    }
}
