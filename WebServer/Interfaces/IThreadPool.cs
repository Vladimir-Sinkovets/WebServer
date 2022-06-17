using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.Interfaces
{
    internal interface IThreadPool
    {
        int MaxThreadsCount { get; }
        void QueueUserWorkItem(Action action, object state);
    }
}
