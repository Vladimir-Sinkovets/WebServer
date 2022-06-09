using System.Net.Sockets;

namespace WebServer
{
    public interface ITcpListener
    {
        TcpClient AcceptTcpClient();
        void Start();
        void Stop();
    }
}