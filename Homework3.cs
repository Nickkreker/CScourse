using System;

namespace homework
{
    internal static class Simplifier
    {
        public static string Simplify(string arg)
        {
            int numenator = Int32.Parse(arg.Split("/")[0]);
            int denumenator = Int32.Parse(arg.Split("/")[1]);
            if (denumenator == 0)
                throw new DivideByZeroException();

            long sign = (long)numenator * denumenator;

            numenator = Math.Abs(numenator);
            denumenator = Math.Abs(denumenator);

            int gcd = CalculateGCD(numenator, denumenator);
            numenator /= gcd;
            denumenator /= gcd;

            string result;
            if (denumenator == 1 || numenator == 0)
                result = numenator.ToString();
            else if (sign > 0)
                result = numenator.ToString() + "/" + denumenator.ToString();
            else
                result = "-" + numenator.ToString() + "/" + denumenator.ToString();
            return result;
        }

        private static int CalculateGCD(int a, int b)
        {
            while (b != 0)
            {
                int t = b;
                b = a % b;
                a = t; 
            }
            return a;
        }
    }
    
    class Homework3
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Task 3:");
            Console.WriteLine("12/4    = " + Simplifier.Simplify("12/4"));
            Console.WriteLine("123/369 = " + Simplifier.Simplify("123/369"));
            Console.WriteLine("5/9     = " + Simplifier.Simplify("5/9"));
            Console.WriteLine("0/10    = " + Simplifier.Simplify("0/10"));
            Console.WriteLine("-5/9    = " + Simplifier.Simplify("-5/9"));
            Console.WriteLine("5/-9    = " + Simplifier.Simplify("5/-9"));
            Console.WriteLine("-11/-33 = " + Simplifier.Simplify("-11/-33"));

            Console.ReadLine();
        }
    }
}
