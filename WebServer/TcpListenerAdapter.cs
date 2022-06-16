using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WebServer.Interfaces;

namespace WebServer
{
    public class TcpListenerAdapter : ITcpListener
    {
        TcpListener _listener;

        public TcpListenerAdapter(TcpListener listener)
        {
            _listener = listener;
        }

        public TcpClient AcceptTcpClient() => _listener.AcceptTcpClient();

        public void Start() => _listener.Start();

        public void Stop() => _listener.Stop();
    }
}
