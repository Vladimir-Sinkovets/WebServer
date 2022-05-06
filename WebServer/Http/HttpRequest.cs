using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace WebServer
{
    public class HttpRequest
    {
        public string Method { get; }
        public string Url { get; }
        public string Cookie { get; }

        private IDictionary<string, string> _headers = new Dictionary<string, string>();

        public HttpRequest(string httpData)
        {
            Regex cookieRegex = new Regex(@"(?<=Cookie:).+", RegexOptions.IgnoreCase);
            Cookie = cookieRegex.Match(httpData).Value;
        }
    }
}