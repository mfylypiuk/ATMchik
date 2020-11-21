using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ATMchik.Handlers
{
    static class VerificationValuesHandler
    {
        public static uint GeneratePvvCode()
        {
            var cryptoRng = new RNGCryptoServiceProvider();
            byte[] buffer = new byte[4];

            cryptoRng.GetBytes(buffer);
            var value = BitConverter.ToUInt32(buffer, 0);

            return value % 8999 + 1000;
        }

        public static string GetPvvHash(uint pvv)
        {
            using var sha512 = new SHA512Managed();

            var data = BitConverter.GetBytes(pvv);
            var hash = sha512.ComputeHash(data);
            hash = sha512.ComputeHash(hash);
            hash = sha512.ComputeHash(hash);
            var hashString = BitConverter.ToString(hash);

            return hashString.Substring(0, hash.Length / 2);
        }

        public static ulong GenerateCardNumber()
        {
            var cryptoRng = new RNGCryptoServiceProvider();
            byte[] buffer = new byte[sizeof(ulong)];

            cryptoRng.GetBytes(buffer);
            var value = BitConverter.ToUInt64(buffer, 0);

            return value % 8999999999999999 + 1000000000000000;
        }

        public static ulong GenerateBankAccountNumber()
        {
            var cryptoRng = new RNGCryptoServiceProvider();
            byte[] buffer = new byte[sizeof(ulong)];

            cryptoRng.GetBytes(buffer);
            var value = BitConverter.ToUInt64(buffer, 0);

            return value % 89999999999999 + 10000000000000;
        }

        public static string GetCvvHash(ulong cvv, ulong ban)
        {
            using var sha512 = new SHA512Managed();
            var cvvString = cvv.ToString();
            var banString = ban.ToString();

            var data = BitConverter.GetBytes(cvv + ban);
            var hash = sha512.ComputeHash(data);
            hash = sha512.ComputeHash(hash);
            hash = sha512.ComputeHash(hash);
            var hashString = BitConverter.ToString(hash);

            return hashString.Substring(0, hash.Length / 2);
        }
    }
}
