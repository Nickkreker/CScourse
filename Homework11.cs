using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public class CachedObject : IDisposable
{
    private int id;

    public CachedObject(int id)
    {
        this.id = id;
    }

    public void Dispose()
    {
        Console.WriteLine($"CachedObject with id {id} released recources");
    }
}

public class Cache<T> where T : IDisposable
{
    private int maxSize;
    private List<Tuple<T, DateTime>> cache;
    private int maxUnaccessedStoreTimeMillis;
    private Mutex mutex;
    private Thread gcLookupThread;

    public Cache(int capacity, int maxUnaccessedStoreTime)
    {
        maxSize = capacity;
        this.maxUnaccessedStoreTimeMillis = maxUnaccessedStoreTime;
        cache = new List<Tuple<T, DateTime>>(maxSize);
        mutex = new Mutex();

        GC.RegisterForFullGCNotification(1, 1);
        gcLookupThread = new Thread(WaitForGCNotification);
        gcLookupThread.Start();
        gcLookupThread.IsBackground = true;
    }
    
    public void Add(T obj)
    {
        mutex.WaitOne();
        if (cache.Count == maxSize)
        {
            DeleteOldItems();
        }

        if (cache.Count < maxSize)
        {
            cache.Add(new Tuple<T, DateTime>(obj, DateTime.Now));
        }

        mutex.ReleaseMutex();
    }

    public T Access(int index)
    {
        mutex.WaitOne();
        T result;
        try
        {
            if (index >= cache.Count || index < 0)
                throw new IndexOutOfRangeException();

            cache[index] = new Tuple<T, DateTime>(cache[index].Item1, DateTime.Now);
            result = cache[index].Item1;
        }
        finally
        {
            mutex.ReleaseMutex();
        }
        return result;
    }

    private void DeleteOldItems()
    {
        DateTime currentDateTime = DateTime.Now;
        cache.RemoveAll(item =>
        { 
            if ((currentDateTime - item.Item2).TotalMilliseconds >= maxUnaccessedStoreTimeMillis)
            {
                item.Item1.Dispose();
                return true;
            }
            return false;
        });
    }

    private void WaitForGCNotification()
    {
        while (true)
        {
            if (GC.WaitForFullGCApproach() == GCNotificationStatus.Succeeded)
            {
                mutex.WaitOne();
                Console.WriteLine("Clearing cache because gc approaches");
                DeleteOldItems();
                mutex.ReleaseMutex();
                GC.WaitForFullGCComplete();
            }
        }
    }
}

public class Homework11
{
    public static void Main()
    {
        TestFreeCacheAfterAdd();
        TestFreeCacheWhenGcApproaches();
    }

    public static void TestFreeCacheAfterAdd()
    {
        Console.WriteLine("Task 1: Part 1");

        Cache<CachedObject> cache = new Cache<CachedObject>(2, 2000);
        cache.Add(new CachedObject(123));
        cache.Add(new CachedObject(234));

        Thread.Sleep(2000);
        cache.Access(1);
        cache.Add(new CachedObject(763)); // We expect that item with id = 123 is deleted
    }

    public static void TestFreeCacheWhenGcApproaches()
    {
        Console.WriteLine("Task 1: Part 2");
        Cache<CachedObject> cache = new Cache<CachedObject>(200000, 1000);
        for (int i = 0; i < 100000; ++i)
        {
            cache.Add(new CachedObject(i));
            // Thread.Sleep();
        }
     
        Thread.Sleep(5000);
        GC.Collect(1);
        for (int i = 0; i < 50000; ++i)
        {
            Console.WriteLine(i);
            cache.Access(i);
        }
        for (int i = 0; i < 100000; ++i)
        {
            cache.Add(new CachedObject(i));
            // Thread.Sleep();
        }

        Thread.Sleep(5000);
    }
}
