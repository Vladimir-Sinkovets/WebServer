using Microsoft.Extensions.Configuration;
using System;

namespace WebServer.OptionsModels
{
    public class WebServerConfiguration
    {
        public int ThreadsCount { get; set; }
        public string IpAddress { get; set; }
        public string Name { get; set; }
        public int Port { get; set; }

        internal IConfiguration GetSection(string v)
        {
            throw new NotImplementedException();
        }
    }
}
