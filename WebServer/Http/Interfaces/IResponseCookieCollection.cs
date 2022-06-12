using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.Http.Interfaces
{
    public interface IResponseCookieCollection : IEnumerable<KeyValuePair<string, string>>
    {
        void Add(string key, string value);
        void Remove(string key);
        bool ContainHeader(string header);
        bool ContainValue(string value);
        bool TryGetValue(string key, out string value);
    }
}
