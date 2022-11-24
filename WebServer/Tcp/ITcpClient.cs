using System.Net.Sockets;

namespace WebServer.Tcp
{
    public interface ITcpClient
    {
        void Close();
        bool Connected { get; }
        int ReceiveBufferSize { get; }
        NetworkStream GetStream();
    }
}