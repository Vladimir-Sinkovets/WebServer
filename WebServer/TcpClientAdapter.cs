using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WebServer.Interfaces;

namespace WebServer
{
    public class TcpClientAdapter : ITcpClient
    {
        private TcpClient _tcpClient;

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
