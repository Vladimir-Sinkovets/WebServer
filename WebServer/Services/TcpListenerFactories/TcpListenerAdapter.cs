using System.Net.Sockets;

namespace WebServer.Services.TcpListenerFactories
{
    public class TcpListenerAdapter : ITcpListener
    {
        readonly TcpListener _listener;

        public TcpListenerAdapter(TcpListener listener)
        {
            _listener = listener;
        }

        public ITcpClient AcceptTcpClient() => new TcpClientAdapter(_listener.AcceptTcpClient());

        public void Start() => _listener.Start();

        public void Stop() => _listener.Stop();
    }
}