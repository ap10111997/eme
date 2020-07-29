using EMessagesNew2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Responses.auth
{
    /// <summary>
    /// Модель ответа обновления токена пользователя
    /// </summary>
    public class TokenRefreshResponse : IResponse
    {
        public string EncryptionKey { get; set; }
    }
}
