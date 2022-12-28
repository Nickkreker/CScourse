using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

public class ZeroEvenOdd
{
    private int n;
    private object locker = new object();
    private int curNum;

    public ZeroEvenOdd(int n)
    {
        this.n = n;
        curNum = 0;
    }
    // printNumber(x) outputs "x", where x is an integer.
    public void Zero(Action<int> printNumber)
    {
        Monitor.Enter(locker);
        while (curNum < 2 * n)
        {
            while (curNum % 2 != 0)
            {
                Monitor.Exit(locker);
                Monitor.Enter(locker);
            }
            if (curNum >= 2 * n)
            {
                Monitor.Exit(locker);
                break;
            }
            printNumber(0);
            curNum += 1;
        }

        if (Monitor.IsEntered(locker))
            Monitor.Exit(locker);
    }

    public void Even(Action<int> printNumber)
    {
        Monitor.Enter(locker);
        while (curNum < 2 * n)
        {
            while (curNum % 4 != 3)
            {
                Monitor.Exit(locker);
                Monitor.Enter(locker);
                if (curNum >= 2 * n)
                {
                    break;
                }
            }
            if (curNum >= 2 * n)
            {
                Monitor.Exit(locker);
                break;
            }

            printNumber((curNum + 1) / 2);
            curNum += 1;
        }
        if (Monitor.IsEntered(locker))
            Monitor.Exit(locker);
    }
    public void Odd(Action<int> printNumber)
    {
        Monitor.Enter(locker);
        while (curNum < 2 * n)
        {
            while (curNum % 4 != 1)
            {
                Monitor.Exit(locker);
                Monitor.Enter(locker);
                if (curNum >= 2 * n)
                {
                    break;
                }
            }
            if (curNum >= 2 * n)
            {
                Monitor.Exit(locker);
                break;              
            }
            printNumber((curNum + 1) / 2);
            curNum += 1;
        }
        if (Monitor.IsEntered(locker))
            Monitor.Exit(locker);
    }
}

class MatrixMult
{
    public static double[,] Multiply(double[,] a, double[,] b, int numThreads)
    {
        if (a.GetLength(1) != b.GetLength(0))
            throw new ArgumentException("a and b cannot be multiplied");

        double[,] c = new double[a.GetLength(0), b.GetLength(1)];
        int lastCalculatedIndex = -1;
        int p = numThreads;

        Thread[] threads = new Thread[p];
        for (int i = 0; i < p; ++i)
        {
            threads[i] = new Thread(() =>
            {
                while (true)
                {
                    int workItem = Interlocked.Increment(ref lastCalculatedIndex);
                    int i = workItem / c.GetLength(0);
                    int j = workItem % c.GetLength(1);
                    if (i >= c.GetLength(0))
                        break;
                    double res = 0;
                    for (int k = 0; k < a.GetLength(1); ++k)
                    {
                        res += a[i, k] * b[k, j];
                    }
                    c[i, j] = res;
                    Thread.Sleep(50);
                }
            });
            threads[i].Start();
        }

        foreach (Thread thread in threads)
            thread.Join();

        return c;
    }
}


public class Homework16
{
    public static void Main()
    {
        // Task 1
        Console.WriteLine("Task 1");
        ZeroEvenOdd zeo = new ZeroEvenOdd(1);
        Thread t1 = new Thread(() => zeo.Even(Console.Write));
        Thread t2 = new Thread(() => zeo.Odd(Console.Write));
        Thread t3 = new Thread(() => zeo.Zero(Console.Write));

        t1.Start();
        t2.Start();
        t3.Start();
        t1.Join();
        t2.Join();
        t3.Join();

        Console.WriteLine();
        zeo = new ZeroEvenOdd(5);
        t1 = new Thread(() => zeo.Even(Console.Write));
        t2 = new Thread(() => zeo.Odd(Console.Write));
        t3 = new Thread(() => zeo.Zero(Console.Write));

        t1.Start();
        t2.Start();
        t3.Start();
        t1.Join();
        t2.Join();
        t3.Join();

        Console.WriteLine("\nTask 4");
        double[,] a = new double[3, 3] { { 13.1, 12,  0.008 }, { 11, 0.5, 3 }, { -9, 1.2, 1 } };
        double[,] b = new double[3, 3] { { -3, 5.6, 3.3 }, { 2, 2, 2.2 }, { 2, 1, 1 } };
        double[,] c = MatrixMult.Multiply(a, b, 5);
        for (int i = 0; i < c.GetLength(0); ++i)
        {
            for (int j = 0; j < c.GetLength(1); ++j)
                Console.Write($"{c[i, j]} ");
            Console.WriteLine();
        }
    } 
}
