using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;

namespace CalvinHobbes.Common
{
    public class Helper
    {
        public static async Task<ComicStrip> GetComicStripAsync(DateTime date)
        {
            string requestUrl = string.Format(Constants.BASE_URL, date.Year, date.Month, date.Day);
            string pageContent = await WebAccess.GetHttpWebResponseAsync(requestUrl);

            string pattern = @"<meta name=""twitter:image""[a-z0-9:/. =""]*";
            Match match = Regex.Match(pageContent, pattern, RegexOptions.IgnoreCase);
            string[] tokens = match.Value.Split(new char[] { '"' });

            string widthPattern = @"<meta name=""twitter:image:width""[a-z0-9:/. =""]*";
            Match widthMatch = Regex.Match(pageContent, widthPattern, RegexOptions.IgnoreCase);
            string[] widthTokens = widthMatch.Value.Split(new char[] { '"' });

            string heightPattern = @"<meta name=""twitter:image:height""[a-z0-9:/. =""]*";
            Match heightMatch = Regex.Match(pageContent, heightPattern, RegexOptions.IgnoreCase);
            string[] heightTokens = heightMatch.Value.Split(new char[] { '"' });

            return new ComicStrip(tokens[3], date, Int32.Parse(widthTokens[3]), Int32.Parse(heightTokens[3]));
        }
    }
}
