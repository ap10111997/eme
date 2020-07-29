using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMessagesNew2.Extensions
{
    /// <summary>
    /// Реализует расширения для строк
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Получает массив байт из строка
        /// </summary>
        /// <returns>Массив байт</returns>
        public static byte[] GetBytes(this string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }
    }
}
