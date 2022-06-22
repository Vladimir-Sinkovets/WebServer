using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Http.Interfaces;

namespace WebServer.Http.Models
{
    public class HttpContext : IHttpContext
    {
        public IHttpRequest Request { get; }
        public IHttpResponse Response { get; }
        public IServiceProvider ServiceProvider { get; }

        public HttpContext(IHttpRequest request, IHttpResponse response, IServiceProvider serviceProvider)
        {
            Request = request;
            Response = response;
            ServiceProvider = serviceProvider;
        }
    }
}