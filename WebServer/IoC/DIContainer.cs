using Microsoft.Extensions.DependencyInjection;
using System;

namespace WebServer.IoC
{
    public class DIContainer
    {
        public ServiceProvider Provider { get; set; }

        public TService GetService<TService>() => Provider.GetService<TService>();
    }
}