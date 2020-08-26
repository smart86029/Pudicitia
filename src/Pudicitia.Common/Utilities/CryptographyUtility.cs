using System;
using System.Security.Cryptography;
using System.Text;

namespace Pudicitia.Common.Utilities
{
    public static class CryptographyUtility
    {
        public static string Hash(string input, string salt)
        {
            var sha256 = SHA256.Create();
            var source = Encoding.UTF8.GetBytes(input + salt);
            var hash = sha256.ComputeHash(source);
            var result = Convert.ToBase64String(hash);

            return result;
        }
    }
}