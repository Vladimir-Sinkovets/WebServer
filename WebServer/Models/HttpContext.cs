using System;

namespace WebServer.Models
{
    public class HttpContext
    {
        public HttpRequest Request { get; }
        public HttpResponse Response { get; }
        public IServiceProvider ServiceProvider { get; }

        public HttpContext(HttpRequest request, HttpResponse response, IServiceProvider serviceProvider)
        {
            Request = request;
            Response = response;
            ServiceProvider = serviceProvider;
        }
    }
}