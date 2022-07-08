using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using WebServer.Http.Interfaces;
using WebServer.Interfaces;
using WebServer.OptionsModels;
using WebServer.Services;
using WebServer.Extensions.ServerCollectionEx;
using WebServer.Extensions.ServiceCollectionEx;

namespace WebServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IServer server = ConfigureServer();
            server.Run();
        }

        private static void Handle(IHttpContext context)
        {
            ICookieIdentifier identifier = context.ServiceProvider.GetService<ICookieIdentifier>();

            identifier.IdentifyUser(context);

            context.Response.ContentType = "text/html";
            context.Response.Body = Encoding.ASCII.GetBytes($"{identifier.CurrentUserId}");
        }


        private static IServer ConfigureServer()
        {
            DIContainer.ConfigureServices(services =>
            {
                services.AddServers(new string[] { "Server_1", });
                services.AddScoped<ICookieIdentifier, CookieIdentifier>();
            });

            IServer server = DIContainer.GetService<IServerCollection>()
                .GetServer(name: "server_1");

            server.SetHandler(Handle);

            return server;
        }
    }
}