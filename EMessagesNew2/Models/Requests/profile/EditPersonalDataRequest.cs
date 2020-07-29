using EMessagesNew2.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Requests.profile
{
    /// <summary>
    /// Модель запроса редактирования персональных данных пользователя
    /// </summary>
    public class EditPersonalDataRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public UserGender Gender { get; set; }

    }
}
