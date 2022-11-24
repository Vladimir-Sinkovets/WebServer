using System.Net.Sockets;

namespace WebServer.Tcp
{
    public interface ITcpListener
    {
        TcpClient AcceptTcpClient();
        void Start();
        void Stop();
    }
}