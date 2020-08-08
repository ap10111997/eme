using EMessagesNew2.Enums;
using EMessagesNew2.Models.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Database
{
    public class User
    {
        [MaxLength(40)]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserGender Gender { get; set; }

        public virtual UserProfile UserProfile { get; set; }
        public virtual List<FileData> Files { get; set; }

        public string Email { get; set; }
        public virtual FileData Img { get; set; }
        public string Alias { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string HashKeySeed { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public string UserSalt { get; set; }
        public string UserIv { get; set; }
        public string IdentificationWord { get; set; }
        public virtual List<Auth> Auth { get; set; }
        public int AvailableKeys { get; set; }
        public UserStates UserState { get; set; }
        public virtual KeySession KeySession { get; set; }
        public virtual RSAKeyPair RSAKeyPair { get; set; }
        public virtual List<EncryptedPublicKey> EncryptedPublicKeys { get; set; }
    }
}
