using System;
using System.Security.Cryptography;

namespace Pudicitia.Common.Utilities
{
    public static class GuidUtility
    {
        private static readonly RNGCryptoServiceProvider rngCryptoServiceProvider = new RNGCryptoServiceProvider();

        public static Guid NewGuid()
        {
            var randomBytes = new byte[10];
            rngCryptoServiceProvider.GetBytes(randomBytes);

            var timestamp = DateTime.UtcNow.Ticks / 10000L;
            var timestampBytes = BitConverter.GetBytes(timestamp);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(timestampBytes);

            var guidBytes = new byte[16];
            Buffer.BlockCopy(randomBytes, 0, guidBytes, 0, 10);
            Buffer.BlockCopy(timestampBytes, 2, guidBytes, 10, 6);

            return new Guid(guidBytes);
        }
    }
}