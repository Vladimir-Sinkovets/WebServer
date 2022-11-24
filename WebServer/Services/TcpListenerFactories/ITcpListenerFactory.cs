using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.Services.TcpListenerFactories
{
    public interface ITcpListenerFactory
    {
        ITcpListener GetInstance(IPAddress address, int localPort);
    }
}
