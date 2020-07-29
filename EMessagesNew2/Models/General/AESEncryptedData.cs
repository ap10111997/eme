using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.General
{
    /// <summary>
    /// Представляет AES зашифрованные данные
    /// </summary>
    [Owned]
    public class AESEncryptedData
    {
        public byte[] EncryptedData { get; set; }
        public byte[] Iv { get; set; } 
        public byte[] Salt { get; set; }
    }
}
