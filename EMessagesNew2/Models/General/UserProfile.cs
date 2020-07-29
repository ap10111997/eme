using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.General
{
    /// <summary>
    /// Представляет данные профиля пользователя
    /// </summary>
    [Owned]
    public class UserProfile
    {
        public string Quote { get; set; }
        public string AboutMe { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
