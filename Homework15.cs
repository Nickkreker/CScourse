using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

class BearAndBees
{
    private object bearLocker = new object();
    private object beeLocker = new object();
    private int curAmount;
    private int n;
    private int x;

    public BearAndBees()
    {
        Console.WriteLine("Enter number of bees");
        n = Int32.Parse(Console.ReadLine());
        Console.WriteLine("Enter pot capacity");
        x = Int32.Parse(Console.ReadLine());
        curAmount = 0;

        Task bearTask = new Task(BearWork);
        Task[] beesTasks = new Task[n];

        bearTask.Start();
        for (int i = 0; i < n; ++i)
        {
            int beeId = i + 1;
            beesTasks[i] = new Task(() => BeeWork(beeId));
            beesTasks[i].Start();
        }
     

        bearTask.Wait();
        Task.WaitAll(beesTasks);
    }

    private void BearWork()
    {
        Monitor.Enter(bearLocker);
        while (true)
        {
            while (curAmount != x)
            {
                Console.WriteLine("Bear sleeps now");
                Monitor.Wait(bearLocker);
                Console.WriteLine("Bear wakes up");
            }
            curAmount = 0;
            Console.WriteLine("Bear sets honey amount to zero");
        }
    }

    private void BeeWork(int beeId)
    {
        Random rnd = new Random();
        Stopwatch stopwatch = new Stopwatch();
        while (true)
        {
            // Monitor.Wait(beeLocker);
            Monitor.Enter(beeLocker);
            while (curAmount == x)
            {
                Monitor.Exit(beeLocker);
                Thread.Sleep(10);
                Monitor.Enter(beeLocker);
            }

            if (curAmount < x - 1)
            {
                curAmount += 1;
                Console.WriteLine($"bee {beeId} adds honey, curAmount = {curAmount}");
                Monitor.Exit(beeLocker);
            }
            else
            {
                curAmount += 1;
                Console.WriteLine($"bee {beeId} adds honey, curAmount = {curAmount}");
                lock (bearLocker)
                {
                    Monitor.Pulse(bearLocker);
                }
                Monitor.Exit(beeLocker);
            }
            Thread.Sleep(rnd.Next(5) * 500);
        }
    }
}

class SleepingBarber
{
    Queue<int> clients;
    bool isWorking;
    int currentClientId;
    int maxQueueSize;
    int barberingTimeMillis;
    private object locker = new object();


    public SleepingBarber(int queueSize)
    {
        isWorking = false;
        barberingTimeMillis = 1000;
        maxQueueSize = queueSize;
        clients = new Queue<int>();
        Thread barber = new Thread(BarberWork);
        barber.Start();
    }

    public void AddClient(int id)
    {
        Monitor.Enter(locker);
        if (isWorking == false)
        {
            currentClientId = id;
            isWorking = true;
            Monitor.Pulse(locker);
        }
        else if (clients.Count < maxQueueSize)
        {
            clients.Enqueue(id);
        }
        Monitor.Exit(locker);
    }

    private void BarberWork()
    {
        Monitor.Enter(locker);
        while (true)
        {
            while (!isWorking)
            {
                Console.WriteLine("Barber goes to sleep");
                Monitor.Wait(locker);
                Console.WriteLine("Barber wakes up from sleep");
            }


            Monitor.Exit(locker);
            Console.WriteLine($"barbering client with id {currentClientId}");
            Thread.Sleep(barberingTimeMillis); // Imitate work
            Monitor.Enter(locker);

            while (clients.Count() != 0)
            {
                currentClientId = clients.Dequeue();
                Monitor.Exit(locker);
                Console.WriteLine($"barbering client with id {currentClientId}");
                Thread.Sleep(barberingTimeMillis); // Imitate work
                Monitor.Enter(locker);
            }

            isWorking = false;
        }
    }
}

public class Homework15
{
    public static void Main()
    {
        // Task 3
        SleepingBarber barber = new SleepingBarber(2);

        // Тест 1. Ожидаем, что все клиенты будут подстрижены
        Console.WriteLine("Task 3: test 1");
        barber.AddClient(11);
        barber.AddClient(33);
        barber.AddClient(145);

        Thread.Sleep(5000);
        // Тест 2. Ожидаем, что только первые 3 клиента будут подстрижены
        Console.WriteLine("Task 3: test 2");
        barber.AddClient(11);
        barber.AddClient(33);
        barber.AddClient(145);
        barber.AddClient(1111);
        barber.AddClient(1134);
        barber.AddClient(113);
        Thread.Sleep(5000);

        // Тест 3. Ожидаем, что только 2 клиента будет подстрижен
        Console.WriteLine("Task 3: test 3");
        SleepingBarber barberNoQueue = new SleepingBarber(0);
        barberNoQueue.AddClient(1111);
        barberNoQueue.AddClient(1134);
        barberNoQueue.AddClient(113);
        Thread.Sleep(2000);
        barberNoQueue.AddClient(155);
        Thread.Sleep(2000);


        // Task 1
        Console.WriteLine("Task 1");
        BearAndBees bab = new BearAndBees(); 
    } 
}
