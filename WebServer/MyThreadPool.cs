using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebServer.Interfaces;

namespace WebServer
{
    internal class MyThreadPool : IThreadPool
    {
        private Thread[] _threads;

        public int MaxThreadsCount { get => _threads.Length; }

        public MyThreadPool(int maxThreadsCount)
        {
            _threads = new Thread[maxThreadsCount];
        }

        public void QueueUserWorkItem(Action action, object state)
        {

        }
    }
}
