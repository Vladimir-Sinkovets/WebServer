using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.Http.Cookie
{
    public class ResponseCookie : IResponseCookie
    {
        private IDictionary<string, string> _pairs = new Dictionary<string, string>();

        public void Add(string key, string value)
        {
            _pairs.Add(key, value);
        }

        public void Delete(string key)
        {
            _pairs.Remove(key);
        }
        public override string ToString()
        {
            return _pairs
                .Select(p => $"{p.Key}={p.Value}")
                .Aggregate((x, y) => x + y);
        }
    }
}
