using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Requests.auth
{
    /// <summary>
    /// Модель запроса проверки существования логина
    /// </summary>
    public class CheckLoginRequest
    {
        public string Login { get; set; }
    }
}
