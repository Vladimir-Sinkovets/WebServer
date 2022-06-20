using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.ThreadPools
{
    internal interface IThreadPool : IDisposable
    {
        int MaxThreadsCount { get; }
        void Execute(Action<object> action, object state);
    }
}
