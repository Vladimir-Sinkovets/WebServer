using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.OptionsModels
{
    public class WebServerConfiguration
    {
        public int ThreadCount { get; set; }
        public string IpAdress { get; set; }
        public int Port { get; set; }
    }
}
