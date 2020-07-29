using EMessagesNew2.Models.Database;
using EMessagesNew2.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.External
{
    /// <summary>
    /// Представляет внешнюю модель чата
    /// </summary>
    public class ExtChat
    {
        public string Id { get; set; }
        public AESEncryptedData Title { get; set; }
        public ExtMessage LastMessage { get; set; }
        public string Image { get; set; }
        public AESEncryptedData EncryptedChatKey { get; set; }
        public byte[] RSAEncryptedKey { get; set; }


        public static explicit operator ExtChat(Chat chat)
        {
            if (chat == null)
            {
                return null;
            }

            return new ExtChat()
            {
                Id = chat.Id,
                Image = chat.Image,
                LastMessage = null
            };
        }
    }
}
