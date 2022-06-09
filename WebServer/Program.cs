using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using WebServer.Http.Interfaces;
using WebServer.Services;

namespace WebServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            int port = 8888;
            ITcpListener listener = new TcpListenerAdapter(new TcpListener(ip, port));

            IServer server = WebServerBuilder.CreateDefaultBuider()
                .SetListener(listener)
                .ConfigureServices(ConfigureServices)
                .SetHandler(HandleRequest)
                .Build();

            server.Run();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.Add(new ServiceDescriptor(typeof(ICookieIdentifier), typeof(CookieIdentifier), ServiceLifetime.Scoped));
        }

        private static void HandleRequest(IHttpContext context)
        {
            ICookieIdentifier identifier = context.ServiceProvider.GetService<ICookieIdentifier>();

            identifier.IdentifyUser(context);

            context.Response.Content = $"<h1>Welcome to my server. {identifier.CurrentUserId}</h1>";
        }
    }
}