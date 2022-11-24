using System.Net.Sockets;

namespace WebServer.Services.TcpListenerFactories
{
    public interface ITcpListener
    {
        ITcpClient AcceptTcpClient();
        void Start();
        void Stop();
    }
}