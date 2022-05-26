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
        string Url { get; }
        IRequestCookieCollection Cookie { get; }
        public string GetHeaderValue(string headerName);
    }
}
