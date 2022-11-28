using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;
using System.Runtime.ExceptionServices;

public class Homework8
{
    public static void Main()
    {
        // Задание 2
        ExceptionDispatchInfo dispatchInfo = ExceptionDispatchInfo.Capture(new IOException("Task 2"));


        // Задание 1
        Console.WriteLine("Task 1");
        string s_r = "Привет мир";
        string s_j = "こんにちは世界";
        string s_d = "Hallo Welt";

        Encoding encodingUTF8 = Encoding.UTF8;
        byte[] encodedBytesJ = encodingUTF8.GetBytes(s_j);
        Console.WriteLine("Japanese string: " + BitConverter.ToString(encodedBytesJ));

        string s_j_d = encodingUTF8.GetString(encodedBytesJ);

        byte[] encodedBytesR = encodingUTF8.GetBytes(s_r);
        byte[] encodedBytesD = encodingUTF8.GetBytes(s_d);

        Console.WriteLine("Russian string: " + BitConverter.ToString(encodedBytesR));
        Console.WriteLine("German string: " + BitConverter.ToString(encodedBytesD));

        string s_r_d = encodingUTF8.GetString(encodedBytesR);
        string s_d_d = encodingUTF8.GetString(encodedBytesD);

        Console.WriteLine(s_j.Equals(s_j_d));
        Console.WriteLine(s_r.Equals(s_r_d));
        Console.WriteLine(s_d.Equals(s_d_d));

        // Задание 5
        Console.WriteLine("\nTask 5");
        Console.WriteLine($"sorting(\"eA2a1E\")->{sorting("eA2a1E")}");
        Console.WriteLine($"sorting(\"Re4r\")->{sorting("Re4r")}");
        Console.WriteLine($"sorting(\"6jnM31Q\")->{sorting("6jnM31Q")}");
        Console.WriteLine($"sorting(\"846ZIbo\")->{sorting("846ZIbo")}\n");

        // Задание 3
        Console.WriteLine("Task 3");
        Console.WriteLine($"\"Money\" and \"Monkey\": {differInOnePos("Money", "Monkey")}");
        Console.WriteLine($"\"wine\" and \"wine\": {differInOnePos("wine", "wine")}");
        Console.WriteLine($"\"\" and \"oop\": {differInOnePos("", "oop")}");

        // Задание 4
        Console.WriteLine("\nTask 4");
        Console.WriteLine(stringyFib(2));
        Console.WriteLine(stringyFib(3));
        Console.WriteLine(stringyFib(7));


        // Задание 2
        Console.WriteLine("\nTask 2");
        dispatchInfo.Throw();
    }

    public static string stringyFib(int n)
    {
        if (n <= 2)
            return "invalid";
        StringBuilder sb = new StringBuilder();

        string prev = "a";
        string cur = "ab";

        sb.Append("b, a, ab");

        for (int i = 3; i < n; ++i)
        {
            string next = cur + prev;
            sb.Append(", " + cur + prev);
            prev = cur;
            cur = next;
        }

        return sb.ToString();
    }

    public static bool differInOnePos(string s1, string s2)
    {
        int[,] d = new int[s1.Length + 1,s2.Length + 1];
        
        for (int i = 0; i < s1.Length + 1; ++i)
        {
            d[i, 0] = i;
        }
        for (int i = 0; i < s2.Length + 1; ++i)
        {
            d[0, i] = i;
        }
        for (int i = 1; i < s1.Length + 1; ++i)
        {
            for (int j = 1; j < s2.Length + 1; ++j)
            {
                if (s1[i - 1] == s2[j - 1])
                    d[i, j] = d[i - 1, j - 1];
                else
                    d[i, j] = Math.Min(Math.Min(d[i - 1, j], d[i, j-1]), d[i-1,j-1]) + 1;
            }
        }
        if (d[s1.Length, s2.Length] == 1)
        {
            return true;
        }
        return false;
    }

    public static string sorting(string text)
    {
        char[] s = text.ToCharArray();
        Array.Sort(s, new CharComparer());
        return new string(s);
    }

    public class CharComparer : IComparer<char>
    {
        public int Compare(char x, char y)
        {
            if (Char.IsLetter(x) && Char.IsDigit(y))
            {
                return -1;
            }
            if (Char.IsLetter(y) && Char.IsDigit(x))
            {
                return 1;
            }
            if (Char.IsLetter(x) && Char.IsLetter(y))
            {
                if (Char.ToUpper(x) == Char.ToUpper(y))
                {
                    if (Char.IsUpper(x) && Char.IsLower(y))
                    {
                        return 1;
                    }
                    else if (Char.IsUpper(y) && Char.IsLower(x))
                    {
                        return -1;
                    }
                }
                return Char.ToUpper(x).CompareTo(Char.ToUpper(y));
            }
            return x.CompareTo(y);
        }
    }
}
