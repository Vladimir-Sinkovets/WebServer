using System.Net.Sockets;

namespace WebServer.Services.TcpListenerFactories
{
    public class TcpClientAdapter : ITcpClient
    {
        private readonly TcpClient _tcpClient;

        public TcpClientAdapter(TcpClient tcpClient)
        {
            _tcpClient = tcpClient;
        }

        public int ReceiveBufferSize { get => _tcpClient.ReceiveBufferSize; }

        public void Close() => _tcpClient.Close();

        public bool Connected { get => _tcpClient.Connected; }

        public NetworkStream GetStream() => _tcpClient.GetStream();
    }
}
