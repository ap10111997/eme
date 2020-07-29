using EMessagesNew2.Classes.Database;
using EMessagesNew2.Classes.ServerResponses;
using EMessagesNew2.Classes.Utilities;
using EMessagesNew2.Extensions;
using EMessagesNew2.Models.Database;
using EMessagesNew2.Models.External;
using EMessagesNew2.Models.Responses.users;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Classes.Hubs
{
    /// <summary>
    /// Реализует функции сокет соединений, связанных с оповещениями пользователей
    /// </summary>
    public class EventsHub : Hub
    {
        private MainContext _database;

        /// <summary>
        /// Создает экземпляр класса
        /// </summary>
        /// <param name="database">Контекст базы данных</param>
        public EventsHub(MainContext database)
        {
            _database = database;
        }

        /// <summary>
        /// Выполняет рассылку уведомлений для подключившихся пользователей
        /// </summary>
        public override async Task OnConnectedAsync()
        {
            User user = Context.GetHttpContext().GetUser();
            if (user == null)
            {
                return;
            }

            UserEvent[] userEvents = await _database.UserEvents.Where(p => p.User.Id == user.Id).ToArrayAsync();

            if (userEvents.Length > 0)
            {
                ExtUserEvent[] extUserEvents = new ExtUserEvent[userEvents.Length];
                for (int i = 0; i < userEvents.Length; i++)
                {
                    extUserEvents[i] = (ExtUserEvent)userEvents[i];
                }

                GetUserEventsResponse userEventsResponse = new GetUserEventsResponse()
                {
                    UserEvents = extUserEvents
                };

                await Clients.Caller.SendAsync("NewEvent", userEventsResponse);
            }

            await base.OnConnectedAsync();
        }
    }
}
