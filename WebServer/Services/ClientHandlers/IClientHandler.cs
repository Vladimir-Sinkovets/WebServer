using WebServer.Services.TcpListenerFactories;

namespace WebServer.Services.ClientHandlers
{
    public interface IClientHandler
    {
        void Handle(ITcpClient client);
    }
}
