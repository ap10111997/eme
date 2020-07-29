using EMessagesNew2.Classes.Database;
using EMessagesNew2.Classes.Utilities;
using EMessagesNew2.Enums;
using EMessagesNew2.Extensions;
using EMessagesNew2.Models.Database;
using EMessagesNew2.Models.External;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EMessagesNew2.Classes.Hubs
{
    /// <summary>
    /// Реализует функции сокет соединений, связанных с чатами
    /// </summary>
    public class ChatHub : Hub
    {
        private readonly MainContext _database;

        /// <summary>
        /// Создает экземляр класса <see cref="ChatHub"/>
        /// </summary>
        /// <param name="database">Контекс базы данных</param>
        public ChatHub(MainContext database)
        {
            _database = database;
        }

        /// <summary>
        /// Сохраняет и рассылает сообщение всем подключеннымк чату пользователям
        /// </summary>
        /// <param name="chatId">Идентификатор чата</param>
        /// <param name="message">Данные сообщения для передачи</param>
        public async Task SendMessage(string chatId, ExtMessage message)
        {
            User user = Context.GetHttpContext().GetUser();
            if (user == null)
            {
                return;
            }

            Chat chat = await _database.Chats.FirstOrDefaultAsync(p => p.Id == chatId);

            if (chat == null)
            {
                await Clients.Caller.SendAsync("ChatError", ResponseUtilities.GetErrorResponse(RequestError.ChatNotFound)).ConfigureAwait(false);
                return;
            }

            Message internalMessage = new Message()
            {
                Chat = chat,
                Date = DateTime.Now,
                EncryptedMessage = message.encryptedMessage,
                Id = RandomUtilities.GetCryptoRandomString(40),
                Sender = _database.Users.First(p => p.Id == user.Id),
                Status = MessageStatus.Encrypted
            };

            chat.LastMessageDate = internalMessage.Date;
            chat.LastMessageId = internalMessage.Id;

            _database.Messages.Add(internalMessage);

            await _database.SaveChangesAsync();

            KeySession[] keySessions = chat.EncryptionChatDatas.Where(p => p.KeySession != null).Select(p => p.KeySession).ToArray();

            User[] users = await _database.Users.Where(p => keySessions.Contains(p.KeySession)).ToArrayAsync();

            for (int i = 0; i < users.Length; i++)
            {
                await Clients.User(users[i].Id).SendAsync("NewMessage", chatId, (ExtMessage)internalMessage);
            }
        }
    }
}
