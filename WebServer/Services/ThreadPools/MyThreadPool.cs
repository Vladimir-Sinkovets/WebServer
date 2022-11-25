using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Threading;

namespace WebServer.Services.ThreadPools
{
    internal class MyThreadPool : IThreadPool
    {
        private ThreadPriority _prioroty;
        private string _name;
        private Thread[] _threads;
        private Queue<(Action<object> Work, object Parameter)> _works = new();
        private volatile bool _isWorking = true; // ?


        private readonly AutoResetEvent _workingEvent = new(false);
        private readonly AutoResetEvent _queueEvent = new(true);
        private const int _disposeThreadJoinTimeout = 100;
        private readonly IOptions<ThreadPoolOptions> _options;


        public string Name { get => _name; }

        public int MaxThreadsCount { get => _threads.Length; }

        public MyThreadPool(IOptions<ThreadPoolOptions> options)
        {
            _options = options;
            var maxThreadsCount = _options.Value.ThreadPoolCount;
            
            if (maxThreadsCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxThreadsCount), maxThreadsCount, "Число потоков в пуле должно быть больше, либо равно 1");

            ThreadPriority prioroty = ThreadPriority.Normal;

            _prioroty = prioroty;
            _name = _options.Value.Name ?? GetHashCode().ToString("x");

            InitializeThreadsArray(maxThreadsCount);
        }

        private void InitializeThreadsArray(int maxThreadsCount)
        {
            _threads = new Thread[maxThreadsCount];
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