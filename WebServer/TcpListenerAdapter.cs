using System.Net.Sockets;
using WebServer.Interfaces;

namespace WebServer
{
    public class TcpListenerAdapter : ITcpListener
    {
        readonly TcpListener _listener;

        public TcpListenerAdapter(TcpListener listener)
        {
            _listener = listener;
        }

        public TcpClient AcceptTcpClient() => _listener.AcceptTcpClient();

        public void Start() => _listener.Start();

        public void Stop() => _listener.Stop();
    }
}