using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using EMessagesNew2.Extensions;

namespace EMessagesNew2.Classes.Utilities
{
    /// <summary>
    /// Представляет руализацию различных функций, связанных со случайностями
    /// </summary>
    public static class RandomUtilities
    {
        private static Random random = new Random();
        private static RandomNumberGenerator rng = new RNGCryptoServiceProvider();

        /// <summary>
        /// Возвращает случайную hex строку заданной длины
        /// </summary>
        /// <param name="length">Длина выходной строки</param>
        /// <returns>Случайная строка</returns>
        public static string GetRandomString(int length)
        {
            byte[] buffer = new byte[length / 2];
            random.NextBytes(buffer);
            return buffer.GetHexString();
        }

        /// <summary>
        /// Получает криптографически случайную hex строку заданной длины
        /// </summary>
        /// <param name="length">Длина выходной строки</param>
        /// <returns>Случайная строка</returns>
        public static string GetCryptoRandomString(int length)
        {
            byte[] buffer = new byte[length / 2];
            rng.GetBytes(buffer);
            return buffer.GetHexString();
        }

        /// <summary>
        /// Заполняет массив случайными данными
        /// </summary>
        /// <param name="bytes">Пустой массив для заполнения</param>
        public static void GetRandomBytes(ref byte[] bytes)
        {
            random.NextBytes(bytes);
        }

        /// <summary>
        /// Возвращает случайное целое число
        /// </summary>
        /// <param name="min">Минимальное значение</param>
        /// <param name="max">Максимальное значение</param>
        /// <returns>Случайное целое число</returns>
        public static int GetInt(int min, int max)
        {
            return random.Next(min, max);
        }

        /// <summary>
        /// Заполняет массив криптографически случайными данными
        /// </summary>
        /// <param name="array">ПУстой массив для зполнения</param>
        public static void GetCryptoRandomBytes(ref byte[] array)
        {
            rng.GetBytes(array);
        }
    }
}
