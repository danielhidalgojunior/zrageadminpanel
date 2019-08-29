using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MapHelper
{
    class HtmlFileParser
    {
        string Url { get; set; }

        public HtmlFileParser(string url)
        {
            Url = url;
        }

        private string GetDirectoryListingRegexForUrl(string url)
        {
            return "<a href=\".*\">(?<name>.*)</a>";
        }

        public async Task<IEnumerable<string>> GetVisibleFiles()
        {
            List<string> files = new List<string>();

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                using (var response = await request.GetResponseAsync())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string html = reader.ReadToEnd();
                        Regex regex = new Regex(GetDirectoryListingRegexForUrl(Url));
                        MatchCollection matches = regex.Matches(html);
                        if (matches.Count > 0)
                        {
                            foreach (Match match in matches)
                            {
                                if (match.Success)
                                {
                                    files.Add(match.Groups["name"].ToString());
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { }

            return files;
        }
    }
}
