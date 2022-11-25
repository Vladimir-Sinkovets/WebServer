﻿using Microsoft.Extensions.DependencyInjection;
using System;

namespace WebServer
{
    public class DIContainer 
    {
        public ServiceProvider Provider { get; private set; }

        public void ConfigureServices(Action<IServiceCollection> action)
        {
            if (Provider != null)
                return;

            IServiceCollection services = new ServiceCollection();

            action(services);

            Provider = services.BuildServiceProvider();
        }

        public TService GetService<TService>() => Provider.GetService<TService>();
    }
}
