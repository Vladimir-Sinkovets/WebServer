using Microsoft.Extensions.DependencyInjection;
using System.Text;
using WebServer.Extensions.ServiceCollectionEx;
using WebServer.IoC;
using WebServer.Models;
using WebServer.Services.ClientHandlers;
using WebServer.Services.CookieIdentifiers;
using WebServer.Services.HttpContextFactories;
using WebServer.Services.ServerCollections;
using WebServer.Services.Servers;
using WebServer.Services.TcpListenerFactories;
using WebServer.Services.ThreadPools;

namespace WebApplication
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IServer server = ConfigureServer();
            server.Run();
        }

        private static void Handle(HttpContext context)
        {
            context.Response.Body = Encoding.UTF8.GetBytes("Hello");
        }

        private static IServer ConfigureServer()
        {
            var container = new DIContainer();

            container.ConfigureServices(services =>
            {
                services.AddServers(new string[] { "server", });

                services.AddTransient<IClientHandler, ClientHandler>(sp =>
                {
                    var clientHandler = new ClientHandler(sp, sp.GetService<IHttpContextFactory>());

                    clientHandler.RequestHandler = Handle;

                    return clientHandler;
                });

                services.AddTransient<IHttpContextFactory, HttpContextFactory>();
                services.AddTransient<ITcpListenerFactory, TcpListenerFactory>();
                services.AddSingleton<IThreadPool, MyThreadPool>();

                services.AddScoped<ICookieIdentifier, CookieIdentifier>();
            });

            IServer server = container.GetService<IServerCollection>()
                .GetServer(name: "server");

            return server;
        }
    }
}
