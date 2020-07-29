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
using EMessagesNew2.Models.Responses.users;
using EMessagesNew2.Models.External;
using EMessagesNew2.Models.Requests.users;
using EMessagesNew2.Extensions;
using EMessagesNew2.Models.Configurations;

namespace EMessagesNew2.Controllers.Users
{
    /// <summary>
    /// Контроллер API контактов пользователя
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly MainContext _context;
        private readonly ServerConfiguration _serverConfig;

        /// <summary>
        /// Создает экземпляр класса
        /// </summary>
        /// <param name="context">Контекст подключения к базе данных</param>
        /// <param name="serverConfig">Конфигурация</param>
        public ContactsController(MainContext context, ServerConfiguration serverConfig)
        {
            _context = context;
            _serverConfig = serverConfig;
        }

        /// <summary>
        /// Получает контакты пользователя
        /// </summary>
        /// <param name="request">Данные запроса</param>
        [HttpPost]
        public async Task<ActionResult<MainResponse>> GetContacts(GetContactsRequest request)
        {
            User user = HttpContext.GetUser();
            if (user == null)
            {
                return Unauthorized();
            }

            Contact[] contacts = await _context.Contacts.Where(p => p.User1.Id == user.Id || p.User2.Id == user.Id).Skip(_serverConfig.Contacts.PartsSize.ContactsPerPart * (request.Part - 1)).Take(_serverConfig.Contacts.PartsSize.ContactsPerPart).ToArrayAsync().ConfigureAwait(false);
            List<ExtUser> extUsers = new List<ExtUser>();

            for (int i = 0; i < contacts.Length; i++)
            {
                extUsers.Add(contacts[i].User1.Id == user.Id ? (ExtUser)contacts[i].User2 : (ExtUser)contacts[i].User1);
                extUsers[i].Img?.SetPath(_serverConfig.General.ServerPath + _serverConfig.FileRoutes.UserImages.Route);
            }

            GetContactsResponse getContactsResponse = new GetContactsResponse()
            {
                Contacts = extUsers
            };

            return MainResponse.GetSuccess(getContactsResponse);
        }

        /// <summary>
        /// Ищет пользователей, не состоящих у пользователя в контактах
        /// </summary>
        /// <param name="request">Данные запроса</param>
        [HttpPost("search")]
        public async Task<ActionResult<MainResponse>> SearchNewContacts(SearchContactsRequest request)
        {
            User user = HttpContext.GetUser();
            if (user == null)
            {
                return StatusCode(401);
            }

            string searchKey = "%" + request.SearchKey.Replace(" ", "%") + "%";

            User[] users1 = new User[0];
            User[] users2 = new User[0];
            string[] friends1 = new string[0];
            string[] friends2 = new string[0];

            IQueryable<Contact> contacts = _context.Contacts.Where(p => p.User1.Id == user.Id || p.User2.Id == user.Id);

            if (request.IsNewContacts)
            {
                users1 = await _context.Users.Where(p => (EF.Functions.Like(p.Login, searchKey) || EF.Functions.Like(p.FirstName, searchKey) || EF.Functions.Like(p.LastName, searchKey) || EF.Functions.Like(p.FirstName + p.LastName, searchKey) || EF.Functions.Like(p.LastName + p.FirstName, searchKey)) && p.Id != user.Id).Skip(_serverConfig.Contacts.PartsSize.SearchResultsPerPart * (request.Part - 1)).Take(_serverConfig.Contacts.PartsSize.SearchResultsPerPart).ToArrayAsync();
                string[] usersId = users1.Select(p => p.Id).ToArray();
                friends1 = contacts.Where(p => usersId.Contains(p.User1.Id)).Select(p => p.User1.Id).ToArray();
                friends2 = contacts.Where(p => usersId.Contains(p.User2.Id)).Select(p => p.User2.Id).ToArray();
            }
            else
            {
                users1 = await contacts.Where(p => p.User1.Id != user.Id).Select(p => p.User1).Where(p => (EF.Functions.Like(p.Login, searchKey) || EF.Functions.Like(p.FirstName, searchKey) || EF.Functions.Like(p.LastName, searchKey) || EF.Functions.Like(p.FirstName + p.LastName, searchKey) || EF.Functions.Like(p.LastName + p.FirstName, searchKey)) && p.Id != user.Id).Skip(_serverConfig.Contacts.PartsSize.SearchResultsPerPart * (request.Part - 1)).Take(_serverConfig.Contacts.PartsSize.SearchResultsPerPart).ToArrayAsync();
                users2 = await contacts.Where(p => p.User2.Id != user.Id).Select(p => p.User2).Where(p => (EF.Functions.Like(p.Login, searchKey) || EF.Functions.Like(p.FirstName, searchKey) || EF.Functions.Like(p.LastName, searchKey) || EF.Functions.Like(p.FirstName + p.LastName, searchKey) || EF.Functions.Like(p.LastName + p.FirstName, searchKey)) && p.Id != user.Id).Skip(_serverConfig.Contacts.PartsSize.SearchResultsPerPart * (request.Part - 1)).Take(_serverConfig.Contacts.PartsSize.SearchResultsPerPart).ToArrayAsync();
            }

            SearchContactsResponse searchContacts = new SearchContactsResponse()
            {
                Users = new List<ExtUser>(),
                FriendsId = new List<string>()
            };

            for (int i = 0; i < users1.Length; i++)
            {
                searchContacts.Users.Add((ExtUser)users1[i]);
                searchContacts.Users[i].Img?.SetPath(_serverConfig.General.ServerPath + _serverConfig.FileRoutes.UserImages.Route);
            }

            for (int i = 0; i < users2.Length; i++)
            {
                searchContacts.Users.Add((ExtUser)users2[i]);
            }

            searchContacts.FriendsId.AddRange(friends1);
            searchContacts.FriendsId.AddRange(friends2);

            return MainResponse.GetSuccess(searchContacts);
        }

        /// <summary>
        /// Добавляет пользователя в контакты
        /// </summary>
        /// <param name="request">Данные для запроса</param>
        [HttpPost("add")]
        public async Task<ActionResult<MainResponse>> AddToContacts(AddToContactsRequest request)
        {
            User user = HttpContext.GetUser();
            if (user == null)
            {
                return StatusCode(401);
            }

            User friend = await _context.Users.FirstOrDefaultAsync(p => p.Id == request.Id);

            if (friend == null)
            {
                return MainResponse.GetError(Enums.RequestError.UserNotFound);
            }

            if (!await _context.Contacts.AnyAsync(p => (p.User1.Id == user.Id && p.User2.Id == friend.Id) || (p.User2.Id == user.Id && p.User1.Id == friend.Id)))
            {
                _context.Contacts.Add(new Contact()
                {
                    User1 = user,
                    User2 = friend
                });

                await _context.SaveChangesAsync().ConfigureAwait(false);
            }

            return MainResponse.GetSuccess();
        }

        /// <summary>
        /// Удалает пользователя из контактов
        /// </summary>
        /// <param name="request">Данные для запроса</param>
        [HttpPost("remove")]
        public async Task<ActionResult<MainResponse>> RemoveFromContacts(AddToContactsRequest request)
        {
            User user = HttpContext.GetUser();
            if (user == null)
            {
                return StatusCode(401);
            }

            User friend = await _context.Users.FirstOrDefaultAsync(p => p.Id == request.Id);

            if (friend == null)
            {
                return MainResponse.GetError(Enums.RequestError.UserNotFound);
            }

            if (await _context.Contacts.AnyAsync(p => (p.User1.Id == user.Id && p.User2.Id == friend.Id) || (p.User2.Id == user.Id && p.User1.Id == friend.Id)))
            {
                _context.Contacts.RemoveRange(_context.Contacts.Where(p => (p.User1.Id == user.Id && p.User2.Id == friend.Id) || (p.User2.Id == user.Id && p.User1.Id == friend.Id)));

                await _context.SaveChangesAsync().ConfigureAwait(false);
            }

            return MainResponse.GetSuccess();
        }
    }
}
