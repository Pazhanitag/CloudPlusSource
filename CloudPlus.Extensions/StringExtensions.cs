using System;
using System.Text.RegularExpressions;

namespace CloudPlus.Extensions
{
    public static class StringExtensions
    {
        public static string TrySubstring(this string input, int startIndex, int length)
        {
            var inputLength = input.Length;

            var result = inputLength - startIndex < length ?
                input.Substring(startIndex, inputLength - startIndex)
                : input.Substring(startIndex, length);

            return result;
        }
        public static string RemoveWhitespaces(this string input)
        {
            return input.Replace(" ", "");
        }
        public static string Remove(this string input, string chars)
        {
            return input.Replace(chars, "");
        }

        public static string OnlyAlphaNumeric(this string input)
        {
            var rgx = new Regex("[^a-zA-Z0-9 -]");
            return rgx.Replace(input, "");
        }

        /// <summary>
        /// Replace middle characters of first part of email (left of @) with *
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string MaskEmail(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentNullException();

            var atSignIndex = input.IndexOf("@", StringComparison.Ordinal);

            if (atSignIndex < 3) return input;

            var numberOfCharactersToMask = atSignIndex - 4;

            if (atSignIndex < 4) numberOfCharactersToMask = atSignIndex - 2;

            input = input.Remove(2, numberOfCharactersToMask);

            for (var i = 0; i < numberOfCharactersToMask; i++)
            {
                input = input.Insert(2, "*");
            }

            return input;
        }
        public static bool IsNotNullAndEquals(this string str1, string str2)
        {
            return !string.IsNullOrEmpty(str1) && str1.Equals(str2);
        }
    }
}
