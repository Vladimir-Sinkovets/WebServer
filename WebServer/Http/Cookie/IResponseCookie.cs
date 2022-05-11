using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.Http.Cookie
{
    public interface IResponseCookie
    {
        void Add(string key, string value);
        void Delete(string key);
    }
}
