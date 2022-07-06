using Microsoft.Extensions.DependencyInjection;
using System.Text;
using WebServer.Http.Interfaces;
using WebServer.Interfaces;
using WebServer.Services;
using WebServer.Extensions.ServiceCollection;
using WebServer.Extensions.ServerCollection;

namespace WebServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DIContainer.ConfigureServices(ConfigureServices);

            IServer server = DIContainer.GetService<IServerCollection>()
                .GetServerByName("server_2");

            server.SetHandler(Handle);
            server.Run();
        }

        private static void Handle(IHttpContext context)
        {
            ICookieIdentifier identifier = context.ServiceProvider.GetService<ICookieIdentifier>();

            identifier.IdentifyUser(context);

            context.Response.StatusCode = Enums.StatusCode.NotFound;

            context.Response.Body = Encoding.ASCII.GetBytes($"<h1>Welcome to my server. {identifier.CurrentUserId}</h1>");
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IServerCollection, ServerCollection>();

            services.AddServer(sectionName: "Server_1");
            services.AddServer(sectionName: "Server_2");

            services.AddScoped<ICookieIdentifier, CookieIdentifier>();
        }
    }
}