using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SingletonThreadSafeProject
{
    internal class RouterManagerPool
    {
        private static Lazy<RouterManagerPool> _routemanager = new Lazy<RouterManagerPool>(() => new RouterManagerPool());
        public static RouterManagerPool Instance { get; } = _routemanager.Value;
        public int Size { get { return _currentSize; } }
        private ConcurrentBag<Client> _bag = new ConcurrentBag<Client>();
        public int TotalObject { get { return _counter; } }
        private const int defaultSize = 10;
        private volatile int _currentSize;
        private volatile int _counter;
        private object _lockObject = new object();

        private RouterManagerPool()
            : this(defaultSize)
        {
        }
        private RouterManagerPool(int size)
        {
            _currentSize = size;
        }

        public Client RequestObject()
        {
            if (!_bag.TryTake(out Client item))
            {
                lock (_lockObject)
                {
                    if (item == null)
                    {
                        if (_counter >= _currentSize)
                            return null;
                        item = new RequestClient();
                        _counter++;
                    }
                }
            }
            return item;
        }
        public async Task ReleaseObject(Client item)
        {
            await Task.Factory.StartNew(() => ReleaseIpAddress(item));
        }

        private void ReleaseIpAddress(Client item)
        {
            Thread.Sleep(8000);
            _counter--;
            _bag.Add(item);
            Console.WriteLine("Dropped Ip Address : " + item.IpAddress);
            Console.WriteLine("Remaining of the pool : {0},", RouterManagerPool.Instance.Size - RouterManagerPool.Instance.TotalObject);
        }
    }
}
