using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Http.Interfaces;
using WebServer.Interfaces;
using WebServer.Services;

namespace WebServer
{
    internal class DefaultStartUp : IStartUp
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.Add(new ServiceDescriptor(typeof(ICookieIdentifier), typeof(CookieIdentifier), ServiceLifetime.Scoped));

        }

        public void Handle(IHttpContext context)
        {
            ICookieIdentifier identifier = context.ServiceProvider.GetService<ICookieIdentifier>();

            identifier.IdentifyUser(context);

            context.Response.Body = Encoding.ASCII.GetBytes($"<h1>Welcome to my server. {identifier.CurrentUserId}</h1>");
        }
    }
}
