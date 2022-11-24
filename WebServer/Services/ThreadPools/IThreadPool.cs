using System;

namespace WebServer.Services.ThreadPools
{
    public interface IThreadPool : IDisposable
    {
        int MaxThreadsCount { get; }
        void Execute(Action<object> action, object state);
    }
}
