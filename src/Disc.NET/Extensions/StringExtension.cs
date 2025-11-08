using System.Text;

namespace Disc.NET.Extensions
{
    internal static class StringExtension
    {
        public static byte[] ToUTF8Bytes(this string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        public static string ToUTF8String(this byte[] arrayBytes)
        {
            return Encoding.UTF8.GetString(arrayBytes);
        }
    }
}
