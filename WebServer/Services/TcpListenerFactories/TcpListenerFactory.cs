using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.Services.TcpListenerFactories
{
    public class TcpListenerFactory : ITcpListenerFactory
    {
        public ITcpListener GetInstance(IPAddress address, int localPort)
        {
            return new TcpListenerAdapter(new TcpListener(address, localPort));
        }
    }
}
