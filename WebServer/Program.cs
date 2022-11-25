using Microsoft.Extensions.DependencyInjection;
using System.Text;
using WebServer.Extensions.ServiceCollectionEx;
using WebServer.Services.CookieIdentifiers;
using WebServer.Services.ServerCollections;
using WebServer.Services.Servers;
using WebServer.Services.Http.Enums;
using WebServer.Services.Http.Models;
using System.ComponentModel;

namespace WebServer
{
    internal class Program
    {
        static void Main()
        {
            IServer server = ConfigureServer();
            server.Run();
        }

        private static void Handle(HttpContext context)
        {
            ICookieIdentifier identifier = context.ServiceProvider.GetService<ICookieIdentifier>();

            identifier.IdentifyUser(context);

            context.Response.StatusCode = StatusCode.OK;
            context.Response.ContentType = null;

            context.Response.Body = Encoding.ASCII.GetBytes($"Hello world!");
        }

        private static IServer ConfigureServer()
        {
            var container = new DIContainer();

            container.ConfigureServices(services =>
            {
                services.AddSingleton<IServerCollection, ServerCollection>();
                services.AddServers(new string[] { "Server_1", });
                services.AddScoped<ICookieIdentifier, CookieIdentifier>();
            });

            IServer server = container.GetService<IServerCollection>()
                .GetServer(name: "server_1");

            //server.SetHandler(Handle);
            
            return server;
        }
    }
}