using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Http.Interfaces;

namespace WebServer.Interfaces
{
    public interface IStartUp
    {
        void ConfigureServices(IServiceCollection services);
        void Handle(IHttpContext context);
    }
}
