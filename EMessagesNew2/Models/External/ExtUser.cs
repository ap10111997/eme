using EMessagesNew2.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.External
{
    /// <summary>
    /// Представляет внешнюю модель пользователя
    /// </summary>
    public class ExtUser
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ExtFileData Img { get; set; }
        public string Alias { get; set; }
        public string Gender { get; set; }

        public static explicit operator ExtUser(User user)
        {
            if (user == null)
            {
                return null;
            };

            return new ExtUser()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Img = (ExtFileData)user.Img,
                Alias = user.Alias != null && user.Alias.Length > 0 ? user.Alias : user.Login,
                Gender = user.Gender.ToString()
            };
        }
    }
}
