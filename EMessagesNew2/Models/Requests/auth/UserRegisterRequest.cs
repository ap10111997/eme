using EMessagesNew2.Models.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Requests.auth
{
    public class UserRegisterRequest
    {
        [MaxLength(50, ErrorMessage = "Длина поля должна быть не не более 50 символов")]
        public string FirstName { get; set; }

        [MaxLength(50, ErrorMessage = "Длина поля должна быть не не более 50 символов")]
        public string LastName { get; set; }

        [MinLength(6, ErrorMessage = "Длина логина должна быть не меньше 6 символов")]
        [MaxLength(50, ErrorMessage = "Длина логина должна быть не не более 50 символов")]
        public string Login { get; set; }

        [MinLength(5, ErrorMessage = "Длина поля должна быть не меньше 5 символов")]
        [MaxLength(50, ErrorMessage = "Длина поля должна быть не не более 50 символов")]
        public string Email { get; set; }

        [MinLength(40, ErrorMessage = "Длина поля должна быть не меньше 40 символов")]
        [MaxLength(128, ErrorMessage = "Длина поля должна быть не не более 128 символов")]
        public string Password { get; set; }

        [MinLength(261)]
        [MaxLength(1023)]
        public string HashKeySeed { get; set; }

        [MinLength(32)]
        [MaxLength(32)]
        public string Iv { get; set; }

        [MinLength(50)]
        [MaxLength(1000)]
        public string IdentificationWord { get; set; }

        public string PublicKey { get; set; }
        public AESEncryptedData EncryptedPrivateKey { get; set; }
    }
}
