using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Database
{
    public class Contact
    {
        public int Id { get; set; }
        public virtual User User1 { get; set; }
        public virtual User User2 { get; set; }
    }
}
