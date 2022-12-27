using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

class Task1
{
    private Mutex mutex1 = new Mutex();
    private Mutex mutex2 = new Mutex();

    public void worker1()
    {
        mutex1.WaitOne();
        Console.WriteLine("Worker 1 locked mutex1");

        Thread.Sleep(3000);

        Console.WriteLine("Worker 1 tries to lock mutex2");
        mutex2.WaitOne();
        Console.WriteLine("Worker 1 locked mutex2");
    }

    public void worker2()
    {
        mutex2.WaitOne();
        Console.WriteLine("Worker 2 locked mutex2");

        Thread.Sleep(3000);

        Console.WriteLine("Worker 2 tries to lock mutex1");
        mutex1.WaitOne();
        Console.WriteLine("Worker 2 locked mutex1");
    }
}

class Task2
{
    private object locker = new object();
    
    public void worker1()
    {
        Monitor.Enter(locker);
        for (int i = 0; i < 10; ++i)
        {
            Monitor.Pulse(locker);
            Monitor.Wait(locker);
            Console.WriteLine($"worker1 {i + 1}");
        }
        if (Monitor.IsEntered(locker))
        {
            Monitor.Pulse(locker);
            Monitor.Exit(locker);
        }
    }

    public void worker2()
    {
        Monitor.Enter(locker);
        for (int i = 0; i < 10; ++i)
        {
            Monitor.Pulse(locker);
            Monitor.Wait(locker);
            Console.WriteLine($"worker2 {i + 1}");
        }
        if (Monitor.IsEntered(locker))
        {
            Monitor.Pulse(locker);
            Monitor.Exit(locker);
        }
    }
}

public class FooBar
{
    private int n;
    private object locker = new object();
    public FooBar(int n)
    {
        this.n = n;
    }
    public void Foo(Action printFoo)
    {
        Monitor.Enter(locker);
        for (int i = 0; i < n; i++)
        {
            Monitor.Pulse(locker);
            Monitor.Wait(locker);
            // printFoo() outputs "foo". Do not change or remove this line.
            printFoo();
        }
        if (Monitor.IsEntered(locker))
        {
            Monitor.Pulse(locker);
            Monitor.Exit(locker);
        }
    }
    public void Bar(Action printBar)
    {
        Monitor.Enter(locker);
        for (int i = 0; i < n; i++)
        {
            Monitor.Pulse(locker);
            Monitor.Wait(locker);
            // printBar() outputs "bar". Do not change or remove this line.
            printBar();
        }
        if (Monitor.IsEntered(locker))
        {
            Monitor.Pulse(locker);
            Monitor.Exit(locker);
        }
    }
}

public class Homework13
{
    public static void Main()
    {
        // Task 3
        Console.WriteLine("Task 3");
        FooBar fooBar = new FooBar(5);
        Thread task3_2 = new Thread(() => fooBar.Bar(() => Console.Write("bar")));
        Thread task3_1 = new Thread(() => fooBar.Foo(() => Console.Write("foo")));
        task3_1.Start();
        task3_2.Start();
        task3_1.Join();
        task3_2.Join();



        // Task 2
        Console.WriteLine("\nTask 2");
        Task2 task2 = new Task2();
        Thread task2_1 = new Thread(task2.worker1);
        Thread task2_2 = new Thread(task2.worker2);
        task2_1.Start();
        task2_2.Start();
        task2_1.Join();
        task2_2.Join();


        // Task 1
        Console.WriteLine("Task 1");
        Task1 task1 = new Task1();
        new Thread(task1.worker1).Start();
        new Thread(task1.worker2).Start();
    } 
}
