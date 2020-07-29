using EMessagesNew2.Extensions;
using EMessagesNew2.Models.Database;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Classes.Sockets
{
    /// <summary>
    /// Представляет класс, создающий собственную реализацию получения идентиифкатора пользователя
    /// </summary>
    public class CustomUserIdProvider : IUserIdProvider
    {
        /// <summary>
        /// Возвращает идентификатор пользователя
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public virtual string GetUserId(HubConnectionContext connection)
        {
            return connection.GetHttpContext().GetUser().Id;
        }
    }
}
