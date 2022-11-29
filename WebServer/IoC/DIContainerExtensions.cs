using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.IoC
{
    public static class DIContainerExtensions
    {
        public static void ConfigureServices(this DIContainer container, Action<IServiceCollection> action)
        {
            if (container.Provider != null)
                return;

            IServiceCollection services = new ServiceCollection();

            action(services);

            container.Provider = services.BuildServiceProvider();
        }
    }
}
