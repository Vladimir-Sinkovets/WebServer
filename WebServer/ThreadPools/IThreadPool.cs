using System;

namespace WebServer.ThreadPools
{
    internal interface IThreadPool : IDisposable
    {
        int MaxThreadsCount { get; }
        void Execute(Action<object> action, object state);
    }
}
