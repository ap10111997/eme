using EMessagesNew2.Enums;
using EMessagesNew2.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.External
{
    /// <summary>
    /// Представляет внешнюю модель уведомления пользователя
    /// </summary>
    public class ExtUserEvent
    {
        public int Id { get; set; }
        public ExtUser User { get; set; }
        public UserEventType EventType { get; set; }
        public List<ExtUserEventParam> Params { get; set; }
        public DateTime Date { get; set; }

        public static explicit operator ExtUserEvent(UserEvent userEvent)
        {
            if (userEvent == null)
            {
                return null;
            }

            ExtUserEvent ext = new ExtUserEvent()
            {
                EventType = userEvent.EventType,
                Id = userEvent.Id,
                User = (ExtUser)userEvent.User,
                Date = userEvent.Date
            };

            if (userEvent.Params != null)
            {
                ext.Params = new List<ExtUserEventParam>();
                for (int i = 0; i < userEvent.Params.Count; i++)
                {
                    ext.Params.Add((ExtUserEventParam)userEvent.Params[i]);
                }
            }

            return ext;
        }
    }
}
