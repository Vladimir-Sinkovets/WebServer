using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WebServer.Http.Interfaces;

namespace WebServer
{
    public interface IWebServerBuilder
    {
        IWebServerBuilder SetHandler(Action<IHttpContext> action);
        IWebServerBuilder SetListener(ITcpListener listener);
        IWebServerBuilder ConfigureServices(Action<IServiceCollection> action);
        IServer Build();
    }
}
