using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EMessagesNew2.Classes.Database;
using EMessagesNew2.Models.Database;
using EMessagesNew2.Classes.ServerResponses;
using EMessagesNew2.Models.Requests.chat;
using EMessagesNew2.Models.Responses.chats;
using EMessagesNew2.Models.External;
using EMessagesNew2.Extensions;
using EMessagesNew2.Classes.Utilities;
using EMessagesNew2.Models.General;
using EMessagesNew2.Models.Configurations;
using Microsoft.AspNetCore.SignalR;
using EMessagesNew2.Classes.Hubs;
using EMessagesNew2.Models.Responses.users;

namespace EMessagesNew2.Controllers.Chats
{
    /// <summary>
    /// Контроллер API чатов
    /// </summary>
    [Route("api/chat")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly MainContext _context;
        private readonly ServerConfiguration _serverConfig;
        private readonly IHubContext<EventsHub> _hubContext;

        /// <summary>
        /// Создает экземпляр контроллера
        /// </summary>
        /// <param name="context">Контекст подключения к базе данных</param>
        /// <param name="serverConfig">Конфигурация</param>
        public ChatsController(MainContext context, ServerConfiguration serverConfig, IHubContext<EventsHub> hubContext)
        {
            _context = context;
            _serverConfig = serverConfig;
            _hubContext = hubContext;
        }

        /// <summary>
        /// Получает сообщения пользователя
        /// </summary>
        /// <param name="request">Данные запроса</param>
        [HttpPost("messages")]
        public async Task<ActionResult<MainResponse>> GetMessages(GetMessagesRequest request)
        {
            User user = HttpContext.GetUser();
            if (user.KeySession == null)
            {
                return Unauthorized();
            }

            Chat chat = await _context.Chats.FirstOrDefaultAsync(p => p.Id == request.ChatId);

            if (chat == null)
            {
                return MainResponse.GetError(Enums.RequestError.ChatNotFound);
            }

            Message[] messages = chat.Messages.OrderByDescending(p => p.Date).Skip(_serverConfig.Chats.PartsSize.MessagesInPart * (request.Part - 1)).Take(_serverConfig.Chats.PartsSize.MessagesInPart).ToArray();

            GetMessagesResponse getMessagesResponse = new GetMessagesResponse()
            {
                Messages = messages.GetExts()
            };

            return MainResponse.GetSuccess(getMessagesResponse);
        }

        /// <summary>
        /// Получает беседы пользователя, связанные с текущей сессией пользователя
        /// </summary>
        /// <param name="request">Данные для запроса</param>
        [HttpPost("conversations")]
        public async Task<ActionResult<MainResponse>> GetConversations(GetChatsRequest request)
        {
            User user = HttpContext.GetUser();
            if (user == null || user.KeySession == null)
            {
                return Unauthorized();
            }

            Chat[] chats = await _context.Chats.Where(p => p.EncryptionChatDatas.Where(c => c.KeySession != null && c.KeySession.Id == user.KeySession.Id).Count() > 0).OrderByDescending(p => p.LastMessageDate).Skip(_serverConfig.Chats.PartsSize.ChatsInPart * (request.Part - 1)).Take(_serverConfig.Chats.PartsSize.ChatsInPart).ToArrayAsync().ConfigureAwait(false);
            string[] messageIds = chats.Select(p => p.LastMessageId).ToArray();
            Message[] messages = await _context.Messages.Where(p => messageIds.Contains(p.Id)).ToArrayAsync();

            GetChatsResponse chatsResponse = new GetChatsResponse()
            {
                Chats = new List<ExtChat>()
            };

            EncryptionChatData encryptionChatData;

            for (int i = 0; i < chats.Length; i++)
            {
                encryptionChatData = chats[i].EncryptionChatDatas.FirstOrDefault(p => p?.KeySession?.Id == user.KeySession.Id);
                ExtChat extChat = (ExtChat)chats[i];
                extChat.LastMessage = (ExtMessage)messages.FirstOrDefault(p => p.Id == chats[i].LastMessageId);
                extChat.EncryptedChatKey = encryptionChatData.AESEncryptedKey;
                extChat.Title = encryptionChatData.EncryptedTitle;
                chatsResponse.Chats.Add(extChat);
            }

            return MainResponse.GetSuccess(chatsResponse);
        }

        /// <summary>
        /// Создает чат в текущей сессии
        /// </summary>
        /// <param name="request">Данные для запроса</param>
        [HttpPost("create")]
        public async Task<ActionResult<MainResponse>> CreateChat(CreateChatRequest request)
        {
            User user = HttpContext.GetUser();
            if (user == null || user.KeySession == null)
            {
                return Unauthorized();
            }

            int[] keyIds = request.EncryptedPayload.Select(p => p.KeyId).ToArray();

            Chat chat = new Chat()
            {
                Id = RandomUtilities.GetCryptoRandomString(20),
                EncryptionChatDatas = new List<EncryptionChatData>(),
                Image = "",
                LastMessageDate = DateTime.Now,
                LastMessageId = null,
                Messages = new List<Message>(),
            };

            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();

            User[] users = await _context.Users.Where(p => keyIds.Contains(p.RSAKeyPair.Id)).ToArrayAsync();

            EncryptionChatData[] encryptionChatDatas = new EncryptionChatData[users.Length + 1];

            CreateChatEncryptedPayload encryptedPayload;

            for (int i = 0; i < users.Length; i++)
            {
                encryptedPayload = request.EncryptedPayload.First(p => p.KeyId == users[i].RSAKeyPair.Id);
                encryptionChatDatas[i] = new EncryptionChatData()
                {
                    AESEncryptedKey = new AESEncryptedData(),
                    Chat = chat,
                    EncryptedTitle = new AESEncryptedData(),
                    KeyTransferPayload = encryptedPayload.PayLoad,
                    RSAEncryptedKey = encryptedPayload.EncryptedChatKey,
                    RSAKeyPair = users[i].RSAKeyPair
                };
            }

            encryptionChatDatas[^1] = new EncryptionChatData()
            {
                Chat = chat,
                AESEncryptedKey = request.AESEncryptedChatKey,
                KeySession = user.KeySession,
                SessionKeySign = request.ChatKeySign,
                EncryptedTitle = request.EncryptedTitle
            };

            chat.EncryptionChatDatas.AddRange(encryptionChatDatas);
            await _context.SaveChangesAsync();

            UserEvent[] events = new UserEvent[users.Length];

            for (int i = 0; i < events.Length; i++)
            {
                events[i] = new UserEvent()
                {
                    EventType = Enums.UserEventType.ChatCreated,
                    User = users[i],
                    Params = new List<UserEventParam>()
                    {
                        new UserEventParam()
                        {
                            Alias = "chatId",
                            Value = chat.Id
                        }
                    }
                };
                await _hubContext.Clients.User(users[i].Id).SendAsync("NewEvent", new GetUserEventsResponse()
                {
                    UserEvents = new ExtUserEvent[]
                    {
                        (ExtUserEvent)events[i]
                    }
                });
            }

            _context.UserEvents.AddRange(events);
            await _context.SaveChangesAsync();

            CreateChatResponse createChatResponse = new CreateChatResponse()
            {
                ChatId = chat.Id
            };

            return MainResponse.GetSuccess(createChatResponse);
        }

        /// <summary>
        /// Выполняет действия для подтверждения присоединения к чату в текущей сессии ключей
        /// </summary>
        /// <param name="request">Данные запроса</param>
        [HttpPost("confirm")]
        public async Task<ActionResult<MainResponse>> ConfirmChat(ConfirmChatRequest request)
        {
            User user = HttpContext.GetUser();
            if (user == null || user.KeySession == null)
            {
                return Unauthorized();
            }

            Chat chat = await _context.Chats.FirstOrDefaultAsync(p => p.Id == request.ChatId);

            if (chat == null)
            {
                return MainResponse.GetError(Enums.RequestError.ChatNotFound);
            }

            EncryptionChatData encryptionChatData = chat.EncryptionChatDatas.FirstOrDefault(p => p.RSAKeyPair == user.RSAKeyPair);

            if (encryptionChatData == null)
            {
                return MainResponse.GetError(Enums.RequestError.ChatKeyNotFound);
            }

            encryptionChatData.KeySession = user.KeySession;
            encryptionChatData.KeyTransferPayload = null;
            encryptionChatData.RSAEncryptedKey = null;
            encryptionChatData.SessionKeySign = request.SessionChatKeySign;
            encryptionChatData.UserKeySign = request.UserChatKeySign;
            encryptionChatData.AESEncryptedKey = request.EncryptedChatKey;
            encryptionChatData.EncryptedTitle = request.EncryptedTitle;

            _context.Entry(encryptionChatData).Reference(a => a.AESEncryptedKey).TargetEntry.State = EntityState.Modified;
            _context.Entry(encryptionChatData).Reference(a => a.EncryptedTitle).TargetEntry.State = EntityState.Modified;

            UserEvent userEvent = await _context.UserEvents.FirstOrDefaultAsync(p => p.Params.FirstOrDefault(p => p.Alias == "chatId").Value == chat.Id);
            _context.UserEvents.Remove(userEvent);

            await _context.SaveChangesAsync().ConfigureAwait(false);

            return MainResponse.GetSuccess();
        }

        /// <summary>
        /// Отправляет информацию о чате по запросу
        /// </summary>
        /// <param name="request">Данные запроса</param>
        /// <returns></returns>
        [HttpPost("unconfirmed-info")]
        public async Task<ActionResult<MainResponse>> UnconfirmedInfo(GetUnconfirmedChatInfoRequest request)
        {
            User user = HttpContext.GetUser();
            if (user == null)
            {
                return Unauthorized();
            }

            Chat chat = await _context.Chats.FirstOrDefaultAsync(p => p.Id == request.ChatId);

            if (chat == null)
            {
                return MainResponse.GetError(Enums.RequestError.ChatNotFound);
            }

            EncryptionChatData chatData = chat.EncryptionChatDatas.FirstOrDefault(p => p?.RSAKeyPair?.Id == user.RSAKeyPair.Id);

            if (chatData == null)
            {
                return MainResponse.GetError(Enums.RequestError.ChatKeyNotFound);
            }

            if (chatData.AESEncryptedKey != null)
            {
                return MainResponse.GetError(Enums.RequestError.ChatAlreadyConfirmed);
            }

            GetUnconfirmedChatInfoResponse response = new GetUnconfirmedChatInfoResponse()
            {
                Payload = chatData.KeyTransferPayload,
                RSAEncryptedChatKey = chatData.RSAEncryptedKey
            };

            return MainResponse.GetSuccess(response);
        }
    }
}
