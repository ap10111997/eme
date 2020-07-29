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
    /// Контроллер API пользователя
    /// </summary>
    [Route("api/user")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MainContext _context;
        private readonly ServerConfiguration _serverConfig;

        /// <summary>
        /// Создает экземпляр класса
        /// </summary>
        /// <param name="context">Контекст подключения к базе данных</param>
        public UsersController(MainContext context, ServerConfiguration serverConfig)
        {
            _context = context;
            _serverConfig = serverConfig;
        }

        /// <summary>
        /// Получает данные текущего пользователя
        /// </summary>
        [HttpGet("data")]
        public async Task<ActionResult<MainResponse>> GetUser()
        {
            User user = HttpContext.GetUser();
            if (user == null)
            {
                return Unauthorized();
            }

            UserDataResponse userDataResponse = new UserDataResponse()
            {
                UserData = (ExtUser)user
            };

            userDataResponse.UserData.Img?.SetPath(_serverConfig.General.ServerPath + _serverConfig.FileRoutes.UserImages.Route);

            return MainResponse.GetSuccess(userDataResponse);
        }

        /// <summary>
        /// Получает публичный ключ пользователя
        /// </summary>
        /// <param name="request">Данные для запроса</param>
        [HttpPost("get-public-key")]
        public async Task<ActionResult<MainResponse>> GetPublicKey(GetPublicKeyRequest request)
        {
            User user = HttpContext.GetUser();
            if (user == null)
            {
                return Unauthorized();
            }

            User user1;

            GetPublicKeyResponse getPublicKeyResponse = new GetPublicKeyResponse()
            {
                PublicKeys = new List<GetPublicKeyResponseItem>()
            };

            for (int i = 0; i < request.UserIds.Count; i++)
            {
                user1 = await _context.Users.FirstOrDefaultAsync(p => p.Id == request.UserIds[i]);
                if (user1 != null)
                {
                    getPublicKeyResponse.PublicKeys.Add(new GetPublicKeyResponseItem()
                    {
                        PublicKey = user1.RSAKeyPair.PublicKey,
                        UserId = user1.Id,
                        KeyId = user1.RSAKeyPair.Id
                    });
                }
            }

            return MainResponse.GetSuccess(getPublicKeyResponse);
        }

        /// <summary>
        /// Сохраняет зашифрованный публичный ключ пользователя
        /// </summary>
        /// <param name="request">Данные для запроса</param>
        [HttpPost("set-encrypted-public-key")]
        public async Task<ActionResult<MainResponse>> SetEndryptedPublicKey(SetEncryptedPublicKeyRequest request)
        {
            User user = HttpContext.GetUser();
            if (user == null)
            {
                return Unauthorized();
            }

            User user1;

            for (int i = 0; i < request.EncryptedPublicKeys.Count; i++)
            {
                user1 = await _context.Users.FirstOrDefaultAsync(p => p.Id == request.EncryptedPublicKeys[i].UserId);

                if (user1 != null)
                {
                    if (!user.EncryptedPublicKeys.Any(p => p.RSAKeyPair.Id == user1.RSAKeyPair.Id))
                    {
                        EncryptedPublicKey encryptedPublicKey = new EncryptedPublicKey()
                        {
                            RSAKeyPair = user1.RSAKeyPair,
                            EncryptedPubicKey = request.EncryptedPublicKeys[i].EncryptedPublicKey
                        };
                        _context.EncryptedPublicKeys.Add(encryptedPublicKey);
                        user.EncryptedPublicKeys.Add(encryptedPublicKey);
                        await _context.SaveChangesAsync();
                    }
                }
            }

            return MainResponse.GetSuccess();
        }

        /// <summary>
        /// Возвращает учетные данные пользователя
        /// </summary>
        [HttpGet("credentials")]
        public async Task<ActionResult<MainResponse>> GetCredentials()
        {
            User user = HttpContext.GetUser();
            if (user == null)
            {
                return NotFound();
            }

            GetCredentialsReqponse response = new GetCredentialsReqponse()
            {
                Alias = user.Alias,
                Email = user.Email,
                Login = user.Login
            };

            return MainResponse.GetSuccess(response);
        }
    }
}
