using System.Net.Sockets;

namespace WebServer
{
    public interface ITcpClient
    {
        void Close();
        bool Connected { get; }
        int ReceiveBufferSize { get; }
        NetworkStream GetStream();
    }
}