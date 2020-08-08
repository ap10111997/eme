using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EMessagesNew2.Classes.Database;
using EMessagesNew2.Models.Database;
using EMessagesNew2.Models.Requests.auth;
using EMessagesNew2.Classes.Utilities;
using EMessagesNew2.Classes.ServerResponses;
using EMessagesNew2.Models.Responses.auth;
using System.Text;
using EMessagesNew2.Extensions;
using EMessagesNew2.Models.Responses.users;
using EMessagesNew2.Models.Configurations;
using Microsoft.AspNetCore.Hosting;

namespace EMessagesNew2.Controllers.Authentication
{
    /// <summary>
    /// Контроллер аутентификации пользователей
    /// </summary>
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly MainContext _context;
        private readonly ServerConfiguration _serverConfig;
        private readonly string coockieDomain = "localhost";

        /// <summary>
        /// Создает экземпляр класса
        /// </summary>
        /// <param name="context">Контекст подключения к базе данных</param>
        /// <param name="serverConfig">Конфигурация приложения</param>
        public AuthenticationController(MainContext context, ServerConfiguration serverConfig, IWebHostEnvironment env)
        {
            _context = context;
            _serverConfig = serverConfig;
            if (env.EnvironmentName != "Development")
            {
                coockieDomain = "me.alpi.pw";
            }
        }

        /// <summary>
        /// Регистрирует пользователя
        /// </summary>
        /// <param name="registerRequest">Данные запроса регистрации</param>
        [HttpPost("register")]
        public async Task<ActionResult<MainResponse>> UserRegister(UserRegisterRequest registerRequest)
        {
            if (_context.Users.Any(p => p.Login == registerRequest.Login))
            {
                return MainResponse.GetError(Enums.RequestError.LoginExists);
            }

            RSAKeyPair keyPair = new RSAKeyPair()
            {
                EncryptedPrivateKey = registerRequest.EncryptedPrivateKey,
                LastKeyPair = null,
                LastPublicKeySign = null,
                PublicKey = registerRequest.PublicKey
            };

            _context.RSAKeyPairs.Add(keyPair);

            User user = new User()
            {
                Id = Hash.Sha1(Hash.Sha256(registerRequest.Login + RandomUtilities.GetCryptoRandomString(53))),
                RegistrationDate = DateTime.Now,
                Email = registerRequest.Email,
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                Login = registerRequest.Login,
                HashKeySeed = registerRequest.HashKeySeed,
                UserSalt = RandomUtilities.GetCryptoRandomString(16),
                IdentificationWord = registerRequest.IdentificationWord,
                UserIv = registerRequest.Iv,
                AvailableKeys = 1000,
                RSAKeyPair = keyPair
            };

            user.Password = GetPassword(registerRequest.Password, user.UserSalt);

            _context.Users.Add(user);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return MainResponse.GetSuccess();
        }

        /// <summary>
        /// Авторизует пользователя
        /// </summary>
        /// <param name="loginRequest">Данные запроса для авторизации</param>
        [HttpPost("login")]
        public async Task<ActionResult<MainResponse>> UserLogin(UserLoginRequest loginRequest)
        {
            User user = await _context.Users.FirstOrDefaultAsync(p => p.Login == loginRequest.Login);
            if (user == null)
            {
                return MainResponse.GetError(Enums.RequestError.UserNotFound);
            }

            string assumedPassword = GetPassword(loginRequest.Password, user.UserSalt);

            if (assumedPassword != user.Password)
            {
                return MainResponse.GetError(Enums.RequestError.UserNotFound);
            }

            Auth auth = new Auth()
            {
                Expire = DateTime.Now.AddSeconds(_serverConfig.Users.TokenExpirationTime),
                LocalDataEncryptionKey = RandomUtilities.GetCryptoRandomString(40),
                Token = RandomUtilities.GetCryptoRandomString(32),
                RefreshToken = RandomUtilities.GetCryptoRandomString(64),
            };

            user.UserState = Enums.UserStates.SecretKey;

            _context.Auth.Add(auth);
            await _context.SaveChangesAsync();

            user.Auth.Add(auth);

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            UserLoginResponse userLoginResponse = new UserLoginResponse()
            {
                UserIV = user.UserIv,
                UserSalt = user.UserSalt,
                IdentificationWord = user.IdentificationWord,
                HashKey = GethashKey(user.HashKeySeed, _serverConfig.General.HashKeyLength),
                EncryptedPrivateKey = user.RSAKeyPair.EncryptedPrivateKey,
                PublicKey = user.RSAKeyPair.PublicKey,
                KeyId = user.RSAKeyPair.Id,
                EncryptionKey = auth.LocalDataEncryptionKey
            };

            HttpContext.Response.Cookies.Append("refresh-token", auth.RefreshToken, new CookieOptions()
            {
                Domain = coockieDomain,
                HttpOnly = true,
                Path = "/api/auth/token-refresh"
            });

            HttpContext.Response.Cookies.Append("auth-token", auth.Token, new CookieOptions()
            {
                Domain = coockieDomain,
                HttpOnly = true,
                Path = "/api"
            });

            HttpContext.Response.Cookies.Append("auth-token", auth.Token, new CookieOptions()
            {
                Domain = coockieDomain,
                HttpOnly = true,
                Path = "/sockets"
            });

            return MainResponse.GetSuccess(userLoginResponse);
        }

        [HttpPost("token-refresh")]
        public async Task<ActionResult<MainResponse>> TokenRefresh()
        {
            User user = HttpContext.GetUser();
            if (user == null)
            {
                return MainResponse.GetError(Enums.RequestError.UserNotFound);
            }

            if (!HttpContext.Request.Cookies.Any(p => p.Key == "refresh-token"))
            {
                return Forbid();
            }

            Auth auth = _context.Auth.FirstOrDefault(p => p.RefreshToken == HttpContext.Request.Cookies["refresh-token"]);

            if (auth == null)
            {
                return Forbid();
            }

            auth.Token = RandomUtilities.GetCryptoRandomString(32);
            auth.RefreshToken = RandomUtilities.GetCryptoRandomString(32);
            auth.Expire = DateTime.Now.AddSeconds(_serverConfig.Users.TokenExpirationTime);

            _context.Auth.Update(auth);
            await _context.SaveChangesAsync();

            HttpContext.Response.Cookies.Append("refresh-token", auth.RefreshToken, new CookieOptions()
            {
                Domain = coockieDomain,
                HttpOnly = true,
                Path = "/api/auth/token-refresh"
            });

            HttpContext.Response.Cookies.Append("auth-token", auth.Token, new CookieOptions()
            {
                Domain = coockieDomain,
                HttpOnly = true,
                Path = "/api"
            });

            HttpContext.Response.Cookies.Append("auth-token", auth.Token, new CookieOptions()
            {
                Domain = coockieDomain,
                HttpOnly = true,
                Path = "/sockets"
            });

            TokenRefreshResponse response = new TokenRefreshResponse()
            {
                EncryptionKey = auth.LocalDataEncryptionKey
            };

            return MainResponse.GetSuccess(response);
        }

        /// <summary>
        /// Проверяет наличие логина в базе данных
        /// </summary>
        /// <param name="checkLogin">Данные для запроса проверки логина</param>
        [HttpPost("check-login")]
        public async Task<ActionResult<MainResponse>> UserCheckLogin(CheckLoginRequest checkLogin)
        {
            UserCheckLoginResponse response = new UserCheckLoginResponse()
            {
                CheckSuccess = !await _context.Users.AnyAsync(p => p.Login == checkLogin.Login).ConfigureAwait(false)
            };

            return MainResponse.GetSuccess(response);
        }

        /// <summary>
        /// Если сессия найдена, связывает пользователя с сессией ключей, иначе отправляет информацию о том, что сессия не найдена
        /// </summary>
        /// <param name="secretKeyRequest">Данные для запроса</param>
        [HttpPost("test-word")]
        public async Task<ActionResult<MainResponse>> TestWord(UserSecretKeyRequest secretKeyRequest)
        {
            User user = HttpContext.GetUser();
            if (user == null)
            {
                return Unauthorized();
            }

            KeySession keySession = await _context.KeySessions.FirstOrDefaultAsync(p => p.VerificationData == secretKeyRequest.IdentificationWord);

            if (keySession != null)
            {
                user.KeySession = keySession;
                user.UserState = Enums.UserStates.Online;
                _context.Update(user);
                await _context.SaveChangesAsync();
            }

            UserSecretKeyResponse secretKeyResponse = new UserSecretKeyResponse()
            {
                UnknownKey = keySession == null
            };

            if (keySession != null)
            {
                secretKeyResponse.PublicKey = keySession.RSAKeyPair.PublicKey;
                secretKeyResponse.EncryptedPrivateKey = keySession.RSAKeyPair.EncryptedPrivateKey;
                secretKeyResponse.KeyId = keySession.RSAKeyPair.Id;
            }

            return MainResponse.GetSuccess(secretKeyResponse);
        }

        /// <summary>
        /// Создает новую сессию ключей для пользователя
        /// </summary>
        /// <param name="request">Данные для запроса</param>
        [HttpPost("create-key-session")]
        public async Task<ActionResult<MainResponse>> CreateTestWord(CreateNewSessionRequest request)
        {
            User user = HttpContext.GetUser();
            if (user == null)
            {
                return StatusCode(401);
            }

            KeySession keySession = _context.KeySessions.FirstOrDefault(p => p.VerificationData == request.IdentificationWord);

            if (keySession == null)
            {
                RSAKeyPair keyPair = new RSAKeyPair()
                {
                    EncryptedPrivateKey = request.EncryptedPrivateKey,
                    LastKeyPair = null,
                    LastPublicKeySign = null,
                    PublicKey = request.PublicKey
                };

                _context.RSAKeyPairs.Add(keyPair);

                keySession = new KeySession()
                {
                    CreationDate = DateTime.Now,
                    VerificationData = request.IdentificationWord,
                    RSAKeyPair = keyPair
                };

                user.KeySession = keySession;
                user.UserState = Enums.UserStates.Online;
                user.AvailableKeys--;
                _context.Users.Update(user);
                await _context.KeySessions.AddAsync(keySession);
                await _context.SaveChangesAsync();
            }

            UserSecretKeyResponse secretKeyResponse = new UserSecretKeyResponse()
            {
                UnknownKey = false
            };

            return MainResponse.GetSuccess(secretKeyResponse);
        }

        /// <summary>
        /// Создает HashKey для пользвателя
        /// </summary>
        /// <param name="hashKeySeed">Зерно для генерации ключа</param>
        /// <param name="l">Длина ключа</param>
        /// <returns>HashKey</returns>
        private string GethashKey(string hashKeySeed, int l)
        {
            string[] result = new string[l / 3];
            string[] hashKeySeedData = new string[(int)Math.Ceiling(hashKeySeed.Length / 4.0)];
            for (int i = 0, j = 0; i < hashKeySeed.Length; i += 4, j++)
            {
                hashKeySeedData[j] = Hash.Sha512(new string(hashKeySeed.Skip(i).Take(4).ToArray())) + Hash.Sha512(new string(hashKeySeed.Skip(i).Take(4).ToArray().Reverse().ToArray()));
            }

            for (int i = 0, j = 0, k = 0; i < result.Length; i++, j++)
            {
                if (j > hashKeySeedData.Length - 1)
                {
                    j = 0;
                    k += 2;

                    if (k > hashKeySeed.Length)
                    {
                        k = 0;
                    }
                }
                string current = new string(hashKeySeed.Skip(k).Take(2).ToArray());
                int index = Convert.ToInt32(current, 16);
                result[i] = new string(hashKeySeedData[j].Skip(index).Take(3).ToArray());
                if (result[i].Length < 3)
                {
                    for (int c = result[i].Length; c < 3; c++)
                    {
                        result[i] = "0" + result[i];
                    }
                }
            }

            return string.Join("", result);
        }

        /// <summary>
        /// Генерирует строку из пароля для сохранения и проверки в базе данных
        /// </summary>
        /// <param name="password">Полученный пароль пользователя</param>
        /// <param name="userSalt">Пользовательская соль</param>
        /// <returns>Строку, представляющую хешированный пароль</returns>
        public static string GetPassword(string password, string userSalt)
        {
            byte[] passBytes = Hash.Sha256(password).GetBytes();
            byte[] saltBytes = userSalt.GetBytes();

            List<byte> result = new List<byte>();

            for (int i = 0, j = 0; i < passBytes.Length; i++, j++)
            {
                result.Add(passBytes[i]);
                if (j >= saltBytes.Length)
                {
                    j = 0;
                }
                result.Add(saltBytes[j]);
            }
            return Hash.Sha256(result.ToArray()).GetHexString();
        }
    }
}
