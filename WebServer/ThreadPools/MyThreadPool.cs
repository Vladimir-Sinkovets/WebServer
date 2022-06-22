using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebServer.ThreadPools;

namespace WebServer.MyThreadPools
{
    public class MyThreadPool : IThreadPool
    {
        private readonly ThreadPriority _prioroty;
        private readonly string _name;
        private readonly Thread[] _threads;
        private readonly Queue<(Action<object> Work, object Parameter)> _works = new();
        private volatile bool _isWorking = true;


        private readonly AutoResetEvent _workingEvent = new(false);
        private readonly AutoResetEvent _queueEvent = new(true);

        private const int _disposeThreadJoinTimeout = 100;

        public string Name { get => _name; }

        public int MaxThreadsCount { get => _threads.Length; }

        public MyThreadPool(int maxThreadsCount, ThreadPriority prioroty = ThreadPriority.Normal, string name = null)
        {
            if (maxThreadsCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxThreadsCount), maxThreadsCount, "Число потоков в пуле должно быть больше, либо равно 1");

            _prioroty = prioroty;
            _threads = new Thread[maxThreadsCount];
            _name = name ?? GetHashCode().ToString("x");

            InitializeThreadsArray();
        }

        private void InitializeThreadsArray()
        {
            for (var i = 0; i < _threads.Length; i++)
            {
                string name = $"{nameof(MyThreadPool)}[{Name}] - Thread[{i}]";

                Thread thread = new(WorkingThread)
                {
                    Name = name,
                    IsBackground = true,
                    Priority = _prioroty
                };

                _threads[i] = thread;

                thread.Start();
            }
        }

        public void Execute(Action<object> action, object parameter)
        {
            if (!_isWorking)
                throw new InvalidOperationException("Попытка передать задание уничтоженному пулу потоков");

            _queueEvent.WaitOne();

            if (!_isWorking)
                throw new InvalidOperationException("Попытка передать задание уничтоженному пулу потоков");

            _works.Enqueue((action, parameter));

            _queueEvent.Set();

            _workingEvent.Set();
        }

        private void WorkingThread()
        {
            try
            {
                while (_isWorking == true)
                {
                    _workingEvent.WaitOne();

                    if (_isWorking == false)
                        break;

                    _queueEvent.WaitOne();
                    
                    WaitWhile(_works.Count == 0);

                    var (work, parameter) = _works.Dequeue();

                    if (_works.Count > 0)
                        _workingEvent.Set();

                    _queueEvent.Set();


                    work(parameter);
                }
            }
            catch (ThreadInterruptedException)
            {

            }
            finally
            {
                //Console.WriteLine($"Thread {Thread.CurrentThread.Name} - has been stopped");
                if (!_workingEvent.SafeWaitHandle.IsClosed)
                    _workingEvent.Set();
            }
        }

        private void WaitWhile(bool condition)
        {
            while (condition == true)
            {
                _queueEvent.Set();

                _workingEvent.WaitOne();

                if (_isWorking == false)
                    break;

                _queueEvent.WaitOne();
            }
        }

        public void Dispose()
        {
            _isWorking = false;

            _workingEvent.Set();

            foreach (var thread in _threads)
            {
                if (!thread.Join(_disposeThreadJoinTimeout))
                    thread.Interrupt();
            }

            _queueEvent.Dispose();
            _workingEvent.Dispose();
        }
    }
}