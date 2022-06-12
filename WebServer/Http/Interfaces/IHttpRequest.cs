using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Enums;

namespace WebServer.Http.Interfaces
{
    public interface IHttpRequest
    {
        HttpMethod Method { get; }
        string Path { get; }
        IRequestCookieCollection Cookie { get; }
        byte[] Body { get; }
        string ContentType { get; } 
        string QueryString { get; }
        IDictionary<string, string> Query { get; }
        IDictionary<string, string> Headers { get; }

    }
}
