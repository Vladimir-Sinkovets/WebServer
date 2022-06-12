using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Http.Helpers;
using WebServer.Http.Interfaces;

namespace WebServer.Http
{
    public class RequestCookieCollection : IRequestCookieCollection
    {
        private IDictionary<string, string> _pairs;

        public RequestCookieCollection(string cookieData)
        {
            _pairs = HttpRequestParseHelper.GetCookieDictionary(cookieData);
        }

        public RequestCookieCollection()
        {
            _pairs = new Dictionary<string, string>();
        }

        public bool ContainsKey(string key)
        {
            return _pairs.ContainsKey(key);
        }

        public bool TryGetValue(string key, out string value)
        {
            bool result = _pairs.TryGetValue(key, out string pairValue);

            value = pairValue;

            return result;
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => _pairs.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _pairs.GetEnumerator();
    }
}
