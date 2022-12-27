using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

class Task1
{
    private int n = 0;
    public void worker1()
    {
        if (n == 0)
        {
            Console.WriteLine("Increasing n since it is 0");
            Thread.Sleep(3000); // Имитируем, что поток был снят с исполнения
            n += 1;
        }
    }

    public void worker2()
    {
        Thread.Sleep(1000);
        Console.WriteLine("Increasing n while another thread is sleeping");
        n += 1;
    }

    public int Get()
    {
        return n;
    }
}
public class Foo
{
    private bool firstExecuted = false;
    private object firstLocker = new object();
    bool secondExecuted = false;
    private object secondLocker = new object();
    public void first() {
        Monitor.Enter(firstLocker);
        Console.Write("first");
        firstExecuted = true;
        Monitor.Pulse(firstLocker);
        Monitor.Exit(firstLocker);
    }
    public void second() {
        Monitor.Enter(firstLocker);
        while (!firstExecuted)
            Monitor.Wait(firstLocker);
        Monitor.Exit(firstLocker);
        Monitor.Enter(secondLocker);
        Console.Write("second");
        secondExecuted = true;
        Monitor.Pulse(secondLocker);
        Monitor.Exit(secondLocker);
    }
    public void third() {
        Monitor.Enter(secondLocker);
        while (!secondExecuted)
            Monitor.Wait(secondLocker);
        Monitor.Exit(secondLocker);
        Console.Write("third");
    }
}

class FileCalculator
{
    private string directory;
    private Thread[] threads;
    private double result;
    private readonly object resultLocker = new object();

    public FileCalculator(string workingDirectory, int numThreads)
    {
        directory = workingDirectory;
        result = 0D;
        threads = new Thread[numThreads];
        List<String>[] threadWorkFiles = new List<string>[numThreads];

        int i = 0;
        foreach (string file in Directory.EnumerateFiles(directory))
        {
            if (threadWorkFiles[i % numThreads] is null)
                threadWorkFiles[i % numThreads] = new List<string>();
            threadWorkFiles[i % numThreads].Add(file);
            i += 1;
        }

        for (i = 0; i < numThreads; ++i)
        {
            int idx = i;
            threads[idx] = new Thread(() => worker(threadWorkFiles[idx]));
        }
    }

    public void RunCalculation()
    {
        foreach (Thread thread in threads)
            thread.Start();
        foreach (Thread thread in threads)
            thread.Join();

        using (StreamWriter writer = new StreamWriter($"{directory}/out.dat"))
            writer.WriteLine(result);
        Console.WriteLine($"Result is {result}");
    }

    private void worker(List<String> workFiles)
    {
        double total = 0D;
        foreach (String file in workFiles)
        {
            string[] lines = File.ReadAllLines(file);
            double fileTotal;
            switch (lines[0].Trim()[0])
            {
                case '1':
                    fileTotal = 0D;
                    foreach (String value in lines[1].Split())
                    {
                        fileTotal += Double.Parse(value);
                    }
                    break;
                case '2':
                    fileTotal = 1D;
                    foreach (String value in lines[1].Split())
                    {
                        fileTotal *= Double.Parse(value);
                    }
                    break;
                case '3':
                    fileTotal = 0D;
                    foreach (String value in lines[1].Split())
                    {
                        fileTotal += (Double.Parse(value) * Double.Parse(value));
                    }
                    break;
                default:
                    throw new ArgumentException("First line of file should contain either 1, 2 or 3");
            }
            total += fileTotal;
        }

        lock (resultLocker)
        {
            result += total;
        }
    }
}

public class Homework14
{
    public static void Main()
    {
        // Task 1
        Console.WriteLine("Task 1");
        Task1 task1 = new Task1();
        Thread task1_1 = new Thread(task1.worker1);
        Thread task1_2 = new Thread(task1.worker2);
        task1_1.Start();
        task1_2.Start();
        task1_1.Join();
        task1_2.Join();
        // Ожидаем, что Get вернет 1, т.к worker1 увеличивает значение на 1
        // только когда n = 0.
        Console.WriteLine($"n = {task1.Get()}");

        // Task 2
        // String input = Console.ReadLine();
        String input = "[3, 1, 2]";
        Console.WriteLine("Task 2");
        Foo foo = new Foo();
        Queue<Thread> threadsTask2 = new Queue<Thread>();
        foreach (char c in input)
        {
            Thread t;
            if (c == '1')
            {
                t = new Thread(foo.first);
                threadsTask2.Enqueue(t);
                t.Start();
            }
            if (c == '2')
            {
                t = new Thread(foo.second);
                threadsTask2.Enqueue(t);
                t.Start();
            }
            if (c == '3')
            {
                t = new Thread(foo.third);
                threadsTask2.Enqueue(t);
                t.Start();
            }
        }

        foreach (Thread t in threadsTask2)
            t.Join();
        


        // Task 4
        Console.WriteLine("\nTask 4");
        FileCalculator fileCalculator = new FileCalculator("C:/courses/c-sharp", 2);
        fileCalculator.RunCalculation();
    } 
}
