using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Parser.Infrastructure.ProcessText
{
    public static class TextProcessor
    {
        public static string RemoveExtraSpace(this string text)
        {
            var filter = @"[^\S\r\n]+";

            return Regex.Replace(text.Trim(), filter, " ");
        }

        public static string ReplaceHtmlTags(this string text, string alternativeText)
        {
            var filter = @"(&\s?\S+?\s?;)?(\\\w+)?";

            var cleanString = Regex.Replace(text, filter, alternativeText);
            return cleanString.RemoveExtraSpace();
        }
    }
}
