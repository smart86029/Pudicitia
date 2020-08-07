using System;
using System.Security.Cryptography;
using System.Text;

namespace Pudicitia.Common.Utilities
{
    public static class CryptographyUtility
    {
        private const string Salt = "8B2085F74DFA9C78A23B7D573C23D27D6D0B0E50C82A9B13138B193325BE3814";

        public static string Hash(string input)
        {
            var sha256 = SHA256.Create();
            var source = Encoding.UTF8.GetBytes(input + Salt);
            var hash = sha256.ComputeHash(source);
            var result = Convert.ToBase64String(hash);

            return result;
        }
    }
}