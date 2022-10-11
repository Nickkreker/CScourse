using System;

// Task 2
interface IConverterA
{
    string Convert(string s);
}

interface IConverterB
{
    string Convert(string s);
}

abstract class AbstractConverter
{
    public abstract string Convert(string s);
}

class Converter : AbstractConverter, IConverterA, IConverterB
{
    private char suffixToAdd;
    public Converter(char suffix)
    {
        suffixToAdd = suffix;
    }
    public override string Convert(string s)
    {
        return s + suffixToAdd;
    }

    string IConverterA.Convert(string s)
    {
        return s.ToUpper();
    }

    string IConverterB.Convert(string s)
    {
        return s.ToLower();
    }
}



public delegate double Function(double x);
public class Integrator
{
    public static double Integrate(Function f, double a, double b)
    {
        const int NUM_STEPS = 10000000;
        const double EPS = 1e-7;
        double integral = 0.0;

        double step = Math.Abs(a - b) / NUM_STEPS;

        for (double x = a; x < b; x += step)
            integral += step * f(x + step / 2);

        if (Math.Abs(integral) < EPS)
            integral = 0;

        return integral;
    }
}

class Ticketer
{
    public static long LuckyTicket(int n)
    {
        n /= 2;

        // Решение методом динамического программирования
        long[,] d = new long[2 * n + 1, 9 * n + 1];
        d[0,0] = 1;
        for (int j = 1; j <= 9*n; ++j)
        {
            d[0,j] = 0;
        }
        for (int i = 1; i <= 2*n; ++i)
        {
            for (int j = 0; j <= 9*n; ++j)
            {
                d[i, j] = 0;
                for (int k = 0; k <= 9; ++k)
                {
                    if (j - k >= 0)
                    {
                        d[i, j] += d[i - 1, j - k];
                    }
                }
            }
        }

        long anwser = 0;
        for (int k = 0; k <= 9*n; ++k)
            anwser += (d[n, k] * d[n, k]);

        return anwser;

    }
}

public class Homework4
{
    public static void Main()
    {
        // Задание 2
        Console.WriteLine("Task 2");
        string s = "Hello World!";
        Converter converter = new Converter('<');
        Console.WriteLine(converter.Convert(s));
        Console.WriteLine(((IConverterA)converter).Convert(s));
        Console.WriteLine(((IConverterB)converter).Convert(s));
        Console.WriteLine();

        // Задание 3
        Console.WriteLine("Task 3");
        Console.WriteLine($"Integral of x^2 on [0, 2]: {Integrator.Integrate(x => x * x, 0, 2)}");
        Console.WriteLine($"Integral of cos(x) on [0,5]: {Integrator.Integrate(x => Math.Cos(x), 0, 5)}");
        Console.WriteLine($"Integral of x^3 + sin(x) on [-1,1]: {Integrator.Integrate(x => x*x*x + Math.Sin(x), -1, 1)}");
        Console.WriteLine();

        // Задание 5
        Console.WriteLine("Task 5");
        Console.WriteLine("Number of lucky tickets with");
        Console.WriteLine($"\t2  digits: {Ticketer.LuckyTicket(2)}");
        Console.WriteLine($"\t6  digits: {Ticketer.LuckyTicket(6)}");
        Console.WriteLine($"\t12 digits: {Ticketer.LuckyTicket(12)}");
        Console.WriteLine($"\t16 digits: {Ticketer.LuckyTicket(16)}");
    }
}
