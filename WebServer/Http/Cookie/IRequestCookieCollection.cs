using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.Http.Cookie
{
    public interface IRequestCookieCollection
    {
        bool ContainsKey(string key);
        bool TryGetValue(string key, out string value);
    }
}
