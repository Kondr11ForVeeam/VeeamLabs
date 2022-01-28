using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _1._5.Lab
{
    class Program
    {
        static void Main(string[] args)
        {
            NaiveCache<string, FileWrapper> cache = new NaiveCache<string, FileWrapper>(new TimeSpan(0, 0, 0, 1, 100), 3); 
            var wapper = cache.GetOrCreate("wrapper", CreateWapper, "mamba");

            Console.WriteLine(cache.Count());
            Thread.Sleep(2000);
            Console.WriteLine(cache.Count());
            Console.ReadKey();
            cache.Dispose();
        }

        public static FileWrapper CreateWapper(string name)
        {
            return new FileWrapper(name);
        }
    }

    public class NaiveCache<T,TItem> where TItem : IDisposable
    {
        private TimeSpan cacheTimeExistence;
        private int casheSize;
        private int currentCasheSize;
        private Thread checkThread;
        private bool stopFlag;
        public NaiveCache(TimeSpan time, int size)
        {
            cacheTimeExistence = time;
            casheSize = size;
            currentCasheSize = size;
            checkThread = new Thread(new ThreadStart(CheckCacheTimeExistence));
            checkThread.Start();
        }
        Dictionary<object, (TItem Value, DateTime LastRequest)> _cache = new Dictionary<object, (TItem, DateTime)>();

        public TItem GetOrCreate(object key, Func<T, TItem> createItem, T item)
        {
            if (currentCasheSize > 0)
            {
                if (!_cache.ContainsKey(key))
                {
                    _cache[key] = (createItem(item), DateTime.Now);
                    currentCasheSize--;
                }
                return _cache[key].Value;
            }
            else
            {
                _cache.Clear();
                _cache[key] = (createItem(item), DateTime.Now);
                currentCasheSize = casheSize - 1;
                return _cache[key].Value;
            }
        }

        private void CheckCacheTimeExistence()
        {
            while (true && !stopFlag)
            {
                var keyCollection = new List<object>();
                if (currentCasheSize != casheSize)
                {
                    foreach (var item in _cache)
                    {
                        var sub = DateTime.Now.Subtract(cacheTimeExistence);
                        if (item.Value.LastRequest.CompareTo(sub) < 0)
                        {
                            item.Value.Value.Dispose();
                            keyCollection.Add(item.Key);
                        }
                    }
                    foreach (var key in keyCollection)
                    {
                        _cache.Remove(key);
                        currentCasheSize++;
                    }
                }
                Thread.Sleep(cacheTimeExistence);
            }
        }

        public int Count()
        {
            return casheSize - currentCasheSize;
        }

        public void Dispose()
        {
            _cache.Clear();
            stopFlag = true;
        }
    }

    public class FileWrapper : IDisposable
    {
        SafeFileHandle _handle;
        bool _disposed;
        object _disposingSync = new object();

        public FileWrapper(string name)
        {
            _handle = CreateFile(name, 0, 0, IntPtr.Zero, 0, 0, IntPtr.Zero);
        }

        public void Seek(int position)
        {
            lock (_disposingSync)
            {
                CheckDisposed();
            }
        }

        public void Dispose()
        {
            lock (_disposingSync)
            {
                if (_disposed) return;
                _disposed = true;
            }
            InternalDispose();
            GC.SuppressFinalize(this);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckDisposed()
        {
            lock (_disposingSync)
            {
                if (_disposed)
                {
                    throw new ObjectDisposedException(string.Empty);
                }
            }
        }

        private void InternalDispose()
        {
            CloseHandle(_handle);
        }

        ~FileWrapper()
        {
            InternalDispose();
        }

        [DllImport("kernel32.dll", EntryPoint = "CreateFile", SetLastError = true)]
        private static extern SafeFileHandle CreateFile(String lpFileName,
        UInt32 dwDesiredAccess, UInt32 dwShareMode,
        IntPtr lpSecurityAttributes, UInt32 dwCreationDisposition,
        UInt32 dwFlagsAndAttributes,
        IntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(SafeFileHandle hObject);
        /// other methods
    }
}
