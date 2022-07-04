using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer
{
    public class DIContainer
    {
        public static ServiceProvider Provider { get; private set; }

        public static void ConfigureServices(Action<IServiceCollection> action)
        {
            if (Provider != null)
                return;

            IServiceCollection services = new ServiceCollection();

            action(services);

            Provider = services.BuildServiceProvider();
        }

        public static TService GetService<TService>() => Provider.GetService<TService>();
    }
}
