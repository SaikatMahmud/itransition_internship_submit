using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto.Digests;

namespace RockPaper
{
    public class Hash
    {
        public string GetKey()
        {
            byte[] key = new byte[32];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(key);
            }
            string keyString = BitConverter.ToString(key).Replace("-", "").ToUpperInvariant();
            return keyString;
        }

        public string GetHMAC(string message, string key)
        {
            var myString = message + key;
            byte[] data = Encoding.UTF8.GetBytes(myString);
            Sha3Digest sha3 = new Sha3Digest(256);
            sha3.BlockUpdate(data, 0, data.Length);
            byte[] result = new byte[sha3.GetDigestSize()];
            sha3.DoFinal(result, 0);
            string hashString = BitConverter.ToString(result).Replace("-", "").ToUpperInvariant();
            return hashString;
        }
    }
}
