using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Dn35x.Crypting
{
    public static class ShaExtends
    {
        public static string ToSha256(this string text)
        {
            byte[] data = Encoding.UTF8.GetBytes(text);
            SHA256 sha = new SHA256Managed();
            byte[] result = sha.ComputeHash(data);
            return BitConverter.ToString(result).Replace("-", "");
        }
    }
}
