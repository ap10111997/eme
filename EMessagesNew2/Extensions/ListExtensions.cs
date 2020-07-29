using EMessagesNew2.Models.Database;
using EMessagesNew2.Models.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Extensions
{
    /// <summary>
    /// Представляет расширения <see cref="List<T>"/>
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Получает список типа <see cref="List<Message>"/>
        /// </summary>
        /// <param name="messages"></param>
        /// <returns></returns>
        public static List<ExtMessage> GetExts(this List<Message> messages)
        {
            List<ExtMessage> extMessages = new List<ExtMessage>();

            for (int i = 0; i < messages.Count; i++)
            {
                extMessages.Add((ExtMessage)messages[i]);
            }

            return extMessages;
        }

        /// <summary>
        /// Получает список типа <see cref="List<Message>"/>
        /// </summary>
        /// <param name="messages"></param>
        /// <returns></returns>
        public static List<ExtMessage> GetExts(this Message[] messages)
        {
            List<ExtMessage> extMessages = new List<ExtMessage>();

            for (int i = 0; i < messages.Length; i++)
            {
                extMessages.Add((ExtMessage)messages[i]);
            }

            return extMessages;
        }
    }
}
