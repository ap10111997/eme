using EMessagesNew2.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EMessagesNew2.Classes.Utilities
{
    /// <summary>
    /// Реализует функционал хеширования данных
    /// </summary>
    public static class Hash
    {
        /// <summary>
        /// Возвращает массив байт, полученный после хеширования входных данных
        /// </summary>
        /// <param name="data">Данные для хеширования</param>
        /// <returns>Хеш</returns>
        public static byte[] Sha256(byte[] data)
        {
            using (SHA256 shaM = new SHA256Managed())
            {
                return shaM.ComputeHash(data);
            }
        }

        /// <summary>
        /// Возвращает массив байт, полученный после хеширования входных данных
        /// </summary>
        /// <param name="data">Данные для хеширования</param>
        /// <returns>Хеш</returns>
        public static string Sha256(string data)
        {
            using (SHA256 shaM = new SHA256Managed())
            {
                return shaM.ComputeHash(data.GetBytes()).GetHexString();
            }
        }

        /// <summary>
        /// Возвращает массив байт, полученный после хеширования входных данных
        /// </summary>
        /// <param name="data">Данные для хеширования</param>
        /// <returns>Хеш</returns>
        public static byte[] Sha512(byte[] data)
        {
            using (SHA512 shaM = new SHA512Managed())
            {
                return shaM.ComputeHash(data);
            }
        }

        /// <summary>
        /// Возвращает массив байт, полученный после хеширования входных данных
        /// </summary>
        /// <param name="data">Данные для хеширования</param>
        /// <returns>Хеш</returns>
        public static string Sha512(string data)
        {
            using (SHA512 shaM = new SHA512Managed())
            {
                return shaM.ComputeHash(data.GetBytes()).GetHexString();
            }
        }

        /// <summary>
        /// Возвращает массив байт, полученный после хеширования входных данных
        /// </summary>
        /// <param name="data">Данные для хеширования</param>
        /// <returns>Хеш</returns>
        public static byte[] Sha1(byte[] data)
        {
            using (SHA1Managed shaM = new SHA1Managed())
            {
                return shaM.ComputeHash(data);
            }
        }

        /// <summary>
        /// Возвращает массив байт, полученный после хеширования входных данных
        /// </summary>
        /// <param name="data">Данные для хеширования</param>
        /// <returns>Хеш</returns>
        public static string Sha1(string data)
        {
            using (SHA1Managed shaM = new SHA1Managed())
            {
                return shaM.ComputeHash(data.GetBytes()).GetHexString();
            }
        }
    }
}
