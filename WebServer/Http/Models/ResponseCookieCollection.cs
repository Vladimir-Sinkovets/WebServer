using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WebServer.Http.Interfaces;

namespace WebServer.Http.Models
{
    public class ResponseCookieCollection : IResponseCookieCollection
    {
        private readonly IDictionary<string, string> _pairs = new Dictionary<string, string>();

        public void Add(string key, string value) => _pairs.Add(key, value);

        public void Remove(string key) => _pairs.Remove(key);

        public bool ContainValue(string value) => _pairs.Any(p => p.Value == value);

        public bool ContainHeader(string header) => _pairs.ContainsKey(header);

        public bool TryGetValue(string key, out string value)
        {
            bool result = _pairs.TryGetValue(key, out string pairValue);

            value = pairValue;

            return result;
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _pairs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _pairs.GetEnumerator();
        }

    }
}
