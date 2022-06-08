using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Http.Interfaces;

namespace WebServer.Http
{
    public class RequestCookieCollection : IRequestCookieCollection
    {
        private IDictionary<string, string> _pairs = new Dictionary<string, string>();
        public RequestCookieCollection()
        {

        }
        public RequestCookieCollection(string cookieData)
        {
            cookieData = cookieData.Replace(" ", "");

            string[] pairs = cookieData.Split(';');

            foreach (var item in pairs)
            {
                string[] s = item.Split('=');

                _pairs.Add(s[0], s[1]);
            }
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
    }
}
