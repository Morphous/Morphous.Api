using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Morphous.Api.Extensions
{
    public static class StringExtensions
    {

        /// <summary>
        /// ToCamelCase method taken from Json.net internal utilities and used in CamelCasePropertyNameContractResolver
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToCamelCase(this string s) {
            if (string.IsNullOrEmpty(s) || !char.IsUpper(s[0])) {
                return s;
            }

            char[] chars = s.ToCharArray();

            for (int i = 0; i < chars.Length; i++) {
                if (i == 1 && !char.IsUpper(chars[i])) {
                    break;
                }

                bool hasNext = (i + 1 < chars.Length);
                if (i > 0 && hasNext && !char.IsUpper(chars[i + 1])) {
                    break;
                }
        #if !(DOTNET || PORTABLE)
                        chars[i] = char.ToLower(chars[i], CultureInfo.InvariantCulture);
        #else
                        chars[i] = char.ToLowerInvariant(chars[i]);
        #endif
            }

            return new string(chars);
        }
    }
}