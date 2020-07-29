using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Requests.profile
{
    /// <summary>
    /// Подель запроса получаения профиля пользователя
    /// </summary>
    public class GetProfileDataRequest
    {
        [Required]
        public string UserAlias { get; set; }
    }
}
