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
using EMessagesNew2.Extensions;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using EMessagesNew2.Models.Requests.profile;
using EMessagesNew2.Models.Configurations;
using EMessagesNew2.Models.Responses.profile;
using EMessagesNew2.Models.External;
using EMessagesNew2.Classes.Utilities;
using EMessagesNew2.Models.General;
using EMessagesNew2.Controllers.Authentication;

namespace EMessagesNew2.Controllers.Profile
{
    /// <summary>
    /// Контроллер API профиля пользователя
    /// </summary>
    [Route("api/profile/data")]
    [ApiController]
    public class ProfileDataController : ControllerBase
    {
        private readonly MainContext _context;
        private readonly ServerConfiguration _serverConfig;

        /// <summary>
        /// Создает экземпляр класса
        /// </summary>
        /// <param name="context">Контекст подключения к базе данных</param>
        /// <param name="usersConfig">Конфигурация</param>
        public ProfileDataController(MainContext context, ServerConfiguration serverConfig)
        {
            _context = context;
            _serverConfig = serverConfig;
        }

        /// <summary>
        /// Получает данные о профиле пользователя
        /// </summary>
        /// <param name="request">Данные запроса</param>
        [HttpPost("get")]
        public async Task<ActionResult<MainResponse>> GetProfileData(GetProfileDataRequest request)
        {
            User user = await _context.Users.FirstOrDefaultAsync(p => p.Alias == request.UserAlias || p.Login == request.UserAlias);

            if (user == null)
            {
                return MainResponse.GetError(Enums.RequestError.UserNotFound);
            }

            GetProfileResponse response = new GetProfileResponse()
            {
                UserData = (ExtUser)user,
                UserProfile = user.UserProfile,
                Images = new List<ExtFileData>()
            };

            response.UserData.Img?.SetPath(_serverConfig.General.ServerPath + _serverConfig.FileRoutes.UserImages.Route);

            FileData[] files = user.Files.Where(p => p.Type == Enums.FileType.Image).OrderByDescending(p => p.UploadDate).Take(_serverConfig.Users.PartsSize.PreviewImagesPartSize).ToArray();

            for (int i = 0; i < files.Length; i++)
            {
                response.Images.Add((ExtFileData)files[i]);
                response.Images[i].SetPath(_serverConfig.General.ServerPath + _serverConfig.FileRoutes.UserImages.Route);
            }
            return MainResponse.GetSuccess(response);
        }

        /// <summary>
        /// Редактирует аватар пользователя
        /// </summary>
        /// <param name="file">Файл + миниатюра</param>
        [HttpPost("edit-avatar")]
        public async Task<ActionResult<MainResponse>> EditAvatar(IFormFileCollection file)
        {
            User user = HttpContext.GetUser();

            if (file == null || file.Count != 2)
            {
                return BadRequest();
            }

            IFormFile thumb = file.FirstOrDefault(p => p.FileName.EndsWith("::t"));
            

            if (thumb == null)
            {
                return BadRequest();
            }

            IFormFile formFile = file.FirstOrDefault(p => !p.FileName.EndsWith("::t"));

            if (formFile == null)
            {
                return BadRequest();
            }

            string path = _serverConfig.Users.Paths.MainPath + _serverConfig.Users.Paths.ImagesPath;
            string hashName = Hash.Sha1(Path.GetFileNameWithoutExtension(formFile.FileName) + user.Id);
            string extension = Path.GetExtension(formFile.FileName);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string thumbPath = path + "\\" + hashName + "_t" + extension;

            path += "\\" + hashName + extension;

            using (Stream stream = new FileStream(path, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }

            using (Stream stream = new FileStream(thumbPath, FileMode.Create))
            {
                await thumb.CopyToAsync(stream);
            }

            FileData fileData = await _context.Files.FirstOrDefaultAsync(p => p.PhysicalName == hashName + extension);

            if (fileData == null)
            {
                fileData = new FileData()
                {
                    FileName = formFile.FileName,
                    PhysicalName = hashName + extension,
                    Type = Enums.FileType.Image,
                    UploadDate = DateTime.Now,
                    Thumbnail = hashName + "_t" + extension
                };

                _context.Files.Add(fileData);
                user.Files.Add(fileData);
            }

            user.Img = fileData;

            await _context.SaveChangesAsync().ConfigureAwait(false);

            return MainResponse.GetSuccess();
        }

        /// <summary>
        /// Редактирует персональные данные пользователя
        /// </summary>
        /// <param name="request">Данные запроса</param>
        [HttpPost("edit-personal")]
        public async Task<ActionResult<MainResponse>> EditPersonalData(EditPersonalDataRequest request)
        {
            User user = HttpContext.GetUser();
            if (user == null)
            {
                return NotFound();
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Gender = request.Gender;
            if (user.UserProfile == null)
            {
                user.UserProfile = new UserProfile();
            }
            user.UserProfile.BirthDate = request.BirthDate;

            _context.Entry(user).Reference(a => a.UserProfile).TargetEntry.State = EntityState.Modified;
            _context.Users.Update(user);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return MainResponse.GetSuccess();
        }

        /// <summary>
        /// Редактирует учетные данные пользователя
        /// </summary>
        /// <param name="request">Данные запроса</param>
        [HttpPost("edit-credentials")]
        public async Task<ActionResult<MainResponse>> EditCredentials(EditCredentialsRequest request)
        {
            User user = HttpContext.GetUser();
            if (user == null)
            {
                return NotFound();
            }

            user.Alias = request.Alias;
            user.Email = request.Email;
            user.Login = request.Login;
            if (request.Password != null && request.Password.Length > 5)
            {
                user.Password = AuthenticationController.GetPassword(request.Password, user.UserSalt);
                user.RSAKeyPair.EncryptedPrivateKey = request.EncryptedPrivateKey;
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return MainResponse.GetSuccess();
        }
    }
}
