using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Extensions
{
    /// <summary>
    /// Представляет расширения массива байт
    /// </summary>
    public static class BytesArrayExtensions
    {
        /// <summary>
        /// Возвращает hex строку из набора байт
        /// </summary>
        /// <returns>Hex строка из набора байт</returns>
        public static string GetHexString(this byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }
}
