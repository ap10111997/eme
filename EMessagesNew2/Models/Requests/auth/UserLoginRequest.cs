using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Requests.auth
{
    /// <summary>
    /// Модель данных авторизации пользователя
    /// </summary>
    public class UserLoginRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
