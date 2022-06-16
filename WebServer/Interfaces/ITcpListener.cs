using System.Net.Sockets;

namespace WebServer.Interfaces
{
    public interface ITcpListener
    {
        TcpClient AcceptTcpClient();
        void Start();
        void Stop();
    }
}