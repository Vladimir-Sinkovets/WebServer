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

namespace WebServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DIContainer.ConfigureServices(ConfigureServices);

            IServer server = DIContainer.GetService<IServer>();

            //server.SetHandler(Handle);
            server.Run();

            //Thread.Sleep(10000);

            //server.Stop();
        }

        private static void Handle(IHttpContext context)
        {
            ICookieIdentifier identifier = context.ServiceProvider.GetService<ICookieIdentifier>();

            identifier.IdentifyUser(context);

            throw new Exception();

            context.Response.Body = Encoding.ASCII.GetBytes($"<h1>Welcome to my server. {identifier.CurrentUserId}</h1>");
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IServerCollection, ServerCollection>();
            
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            services.Configure<WebServerConfiguration>(configuration.GetSection("WebServerSettings"));

            services.AddSingleton<IServer, WebServer>();
            services.AddScoped<ICookieIdentifier, CookieIdentifier>();
        }
    }
}