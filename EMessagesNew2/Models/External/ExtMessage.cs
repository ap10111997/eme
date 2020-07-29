using EMessagesNew2.Enums;
using EMessagesNew2.Models.Database;
using EMessagesNew2.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.External
{
    /// <summary>
    /// Представляет внешнюю модель сообщения
    /// </summary>
    public class ExtMessage
    {
        public string Id { get; set; }
        public MessageStatus Status { get; set; }
        public AESEncryptedData encryptedMessage { get; set; }
        public DateTime Date { get; set; }
        public ExtUser Sender { get; set; }

        public static explicit operator ExtMessage(Message message)
        {
            if (message == null)
            {
                return null;
            }

            return new ExtMessage()
            {
                Id = message.Id,
                Status = message.Status,
                encryptedMessage = message.EncryptedMessage,
                Date = message.Date,
                Sender = (ExtUser)message.Sender
            };
        }
    }
}
