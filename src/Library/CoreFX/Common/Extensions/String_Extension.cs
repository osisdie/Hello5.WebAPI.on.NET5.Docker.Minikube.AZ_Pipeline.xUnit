using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CoreFX.Common.Extensions
{
    public static class String_Extension
    {
        public static string TrimStartSlash(this string path) => path?.TrimStart(new char[] { '\\', '/' })?.Trim();
        public static string TrimEndSlash(this string path) => path?.TrimEnd(new char[] { '\\', '/' })?.Trim();
        public static string CombineAppFolder(this string fileName) => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

        public static string MaskLeft(this string src, char c = '*')
        {
            if (string.IsNullOrWhiteSpace(src))
            {
                return string.Empty;
            }

            return src.Substring(src.Length - src.Length / 2).PadLeft(src.Length, c);
        }

        public static string MaskRight(this string src, char c = '*')
        {
            if (string.IsNullOrWhiteSpace(src))
            {
                return string.Empty;
            }

            return src.Substring(0, src.Length / 2).PadRight(src.Length, c);
        }

        public static string GrepFirst(this string src, char[] dividers = null)
        {
            dividers ??= GeneralDividers;
            return src?.Split(dividers, StringSplitOptions.RemoveEmptyEntries)?.First();
        }

        public static int ToInt(this string src, int defaultVal = 0)
        {
            if (int.TryParse(src, NumberStyles.Number, CultureInfo.InvariantCulture, out var parsedInt))
            {
                return parsedInt;
            }

            return defaultVal;
        }

        public static long ToInt64(this string src, long defaultVal = 0)
        {
            if (long.TryParse(src, NumberStyles.Number, CultureInfo.InvariantCulture, out var parsedInt))
            {
                return parsedInt;
            }

            return defaultVal;
        }

        public static bool ToBool(this string src, bool defaultVal = false)
        {
            if (string.IsNullOrWhiteSpace(src))
            {
                return defaultVal;
            }

            if (TrueStrings.Contains(src, StringComparer.OrdinalIgnoreCase))
            {
                return true;
            }

            if (FalseStrings.Contains(src, StringComparer.OrdinalIgnoreCase))
            {
                return false;
            }

            return defaultVal;
        }

        public static string ToMD5(this string src, Encoding encoding = null)
        {
            using (var md5 = MD5.Create())
            {
                var sourceBytes = (encoding ?? Encoding.UTF8).GetBytes(src);
                var hashBytes = md5.ComputeHash(sourceBytes);
                return BitConverter.ToString(hashBytes).Replace("-", string.Empty);
            }
        }

        public static bool IsNullOrEmpty(this string src) => false == src.HasValue();
        public static bool HasValue(this string src) => false == string.IsNullOrWhiteSpace(src?.Trim(Whitespaces));
        public static bool Contains(this string source, string word, StringComparison comp) => source?.IndexOf(word, comp) >= 0;

        public static string ToDefaultIfEmpty(this string value, string defaultValue = null) =>
            string.IsNullOrEmpty(value) ? defaultValue : value;

        public static string ToDefaultIfWhiteSpace(this string value, string defaultValue = null) =>
            string.IsNullOrWhiteSpace(value) ? defaultValue : value;

        public static readonly string[] TrueStrings = { "1", "t", "y", "yes", "true", "on" };
        public static readonly string[] FalseStrings = { "0", "f", "n", "no", "false", "off" };
        public static readonly char[] GeneralDividers = new char[] { ',', ';' };

        public static readonly char[] Whitespaces = new char[] {
            '\u0020', '\u1680', '\u180E', '\u2000', '\u2001', '\u2002', '\u2003',
            '\u2004', '\u2005', '\u2006', '\u2007', '\u2008', '\u2009', '\u200A',
            '\u202F', '\u205F', '\u3000', '\u2028', '\u2029', '\u200B', '\uFEFF',
            '\u0009', '\u000A', '\u000B', '\u000C', '\u000D', '\u0085', '\u00A0',
        };
    }
}
