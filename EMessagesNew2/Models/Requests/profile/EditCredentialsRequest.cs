using EMessagesNew2.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Requests.profile
{
    /// <summary>
    /// Модель запроса редактирования учетных данных пользователя
    /// </summary>
    public class EditCredentialsRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Alias { get; set; }
        public AESEncryptedData EncryptedPrivateKey { get; set; }
    }
}
