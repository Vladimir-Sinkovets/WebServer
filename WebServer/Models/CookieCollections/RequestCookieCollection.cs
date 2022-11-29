using System.Collections;
using System.Collections.Generic;

namespace WebServer.Models.CookieCollections
{
    public class RequestCookieCollection : IRequestCookieCollection
    {
        private readonly IDictionary<string, string> _pairs;

        public RequestCookieCollection(IDictionary<string, string> pairs)
        {
            _pairs = pairs;
        }

        public RequestCookieCollection()
        {
            _pairs = new Dictionary<string, string>();
        }

        public bool ContainsHeader(string key)
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
