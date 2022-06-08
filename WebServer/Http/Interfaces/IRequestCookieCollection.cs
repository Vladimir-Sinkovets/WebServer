using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.Http.Interfaces
{
    public interface IRequestCookieCollection
    {
        bool ContainsKey(string key);
        bool TryGetValue(string key, out string value);
    }
}
