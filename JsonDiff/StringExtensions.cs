using System;
using System.Text;
using Newtonsoft.Json.Linq;

namespace JsonDiff
{
    /// <summary>
    /// Extension methods for System.String class
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Check if string is in base64 format
        /// </summary>
        /// <param name="base64String"></param>
        public static bool IsBase64(this string base64String)
        {
            if (string.IsNullOrEmpty(base64String) || base64String.Length % 4 != 0
                || base64String.Contains(" ") || base64String.Contains("\t") || base64String.Contains("\r") || base64String.Contains("\n"))
                return false;
            try
            {
                Convert.FromBase64String(base64String);
                return true;
            }
            catch
            {
                // ignored
            }
            return false;
        }

        /// <summary>
        /// Check if string is on a valid json format
        /// </summary>
        /// <param name="strInput"></param>
        public static bool IsValidJson(this string strInput)
        {
            strInput = strInput.Trim();
            if (strInput.StartsWith("{") && strInput.EndsWith("}") ||
                strInput.StartsWith("[") && strInput.EndsWith("]"))
            {
                try
                {
                    JToken.Parse(strInput);
                    return true;
                }
                catch
                { 
                    // ignored 
                }
            }
            return false;
        }

        /// <summary>
        /// Decode string from Base64 format
        /// </summary>
        /// <param name="encodedString"></param>
        public static string Base64Decode(this string encodedString)
        {
            var data = Convert.FromBase64String(encodedString);
            return Encoding.UTF8.GetString(data);
        }

        /// <summary>
        /// Encode string to base64 format
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string Base64Encode(this string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

    }
}
