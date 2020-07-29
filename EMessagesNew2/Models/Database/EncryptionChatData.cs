using EMessagesNew2.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Database
{
    public class EncryptionChatData
    {
        public int Id { get; set; }
        public virtual Chat Chat { get; set; }

        #region KeyTransfer

        /// <summary>
        /// Используется для передачи ключа чата между пользователями
        /// </summary>
        public byte[] RSAEncryptedKey { get; set; }

        /// <summary>
        /// Используется для передачи дополнительной информации при передаче ключа (Зашифровано)
        /// </summary>
        public byte[] KeyTransferPayload { get; set; }
        #endregion

        #region KeyData

        /// <summary>
        /// Используется для хранения ключа чата после передачи
        /// </summary>
        public virtual AESEncryptedData AESEncryptedKey { get; set; }

        /// <summary>
        /// Указатель на сесию, соответствующую зашифрованным данным
        /// </summary>
        public virtual KeySession KeySession { get;set;  }

        /// <summary>
        /// Указатель на общюю пару ключей
        /// </summary>
        public virtual RSAKeyPair RSAKeyPair { get; set; }

        /// <summary>
        /// Подпись ключа чата общим приватным ключом
        /// </summary>
        public byte[] UserKeySign { get; set; }

        /// <summary>
        /// Подпись ключа чата сессионным ключом
        /// </summary>
        public byte[] SessionKeySign { get; set; }
        #endregion

        /// <summary>
        /// Зашифрованный заголовок чата
        /// </summary>
        public virtual AESEncryptedData EncryptedTitle { get; set; }
    }
}
