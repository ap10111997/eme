using EMessagesNew2.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Database
{
    public class UserEvent
    {
        public int Id { get; set; }
        public virtual User User { get; set; }
        public DateTime Date { get; set; }
        public UserEventType EventType { get; set; }
        public virtual List<UserEventParam> Params { get; set; }
    }
}
