using System;

namespace WebServer.Services.ThreadPools
{
    internal interface IThreadPool : IDisposable
    {
        int MaxThreadsCount { get; }
        void Execute(Action<object> action, object state);
    }
}
