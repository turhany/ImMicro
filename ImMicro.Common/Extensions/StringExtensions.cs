using System;
using System.Security.Cryptography;
using System.Text;

namespace ImMicro.Common.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Generate SHA hash from text with given Key
        /// </summary>
        /// <param name="text">text</param>
        /// <param name="key">SHA generate key</param>
        /// <returns></returns>
        public static string ComputeHashSha(this string text, string key)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            using (var hmac = new HMACSHA1(keyBytes))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(text));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    builder.Append(hash[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static string SafeFormat(this string format, object arg)
        {
            var response = string.Empty;
            try
            {
                response = format.Contains("{0}") ? string.Format(format, arg) : format;
            }
            catch
            { 
               //Ignored
            }

            return response;
        }
    }
}