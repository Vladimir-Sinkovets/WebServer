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
        public IHttpRequest Request { get; }
        public IHttpResponse Response { get; }
        public IServiceProvider ServiceProvider { get; }

        public HttpContext(string requestData, IServiceProvider serviceProvider)
        {
            Request = new HttpRequest(requestData);
            Response = new HttpResponse();
            ServiceProvider = serviceProvider;
        }
    }
}
