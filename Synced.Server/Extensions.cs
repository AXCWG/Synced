using System.Security.Cryptography;
using System.Text;

namespace Synced.Server
{
    public static class Extensions
    {
        public static string ToSHA256HexHashString(this string input)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha256.ComputeHash(bytes);
            var hex = BitConverter.ToString(hash).Replace("-", "").ToLower();
            return hex;
        }
    }
}
