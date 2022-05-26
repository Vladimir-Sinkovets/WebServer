using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Http.Interfaces;

namespace WebServer.Http
{
    public class HttpContext : IHttpContext
    {
        public IHttpRequest Request { get; set; }
        public IHttpResponse Response { get; set; }

        public HttpContext(string requestData)
        {
            Request = new HttpRequest(requestData);
            Response = new HttpResponse();
        }
    }
}
