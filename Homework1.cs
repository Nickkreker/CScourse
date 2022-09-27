using System;
using System.Collections.Generic;

namespace myFirstProject
{
    class PasswordGenerator
    {
        public static string GeneratePassword()
        {
            Random rnd = new Random();
            string letters = "abcdefghijklmnopqrstuvwxyz";
            string big_lettes = letters.ToUpper();
            string digits = "0123456789";
            string alphabet = letters + big_lettes + digits;


            int password_length = rnd.Next(6, 21);
            int underscore_pos = rnd.Next(password_length);
            int num_upper_letters = 0;
            int num_digits = 0;

            char[] password = new char[password_length];

            for (int i = 0; i < password_length; ++i)
            {
                if (i == underscore_pos)
                    password[i] = '_';
                else
                {
                    char c = alphabet[rnd.Next(alphabet.Length)];
                    password[i] = c;
                }

                // Forbids 2 consecutive digits;
                if (char.IsDigit(password[i]) && char.IsDigit(password[i - 1]))
                {
                    i--;
                    continue;
                }

                if (char.IsDigit(password[i]))
                {
                    if (num_digits < 5)
                    {
                        num_digits++;
                        continue;
                    }
                    else
                    {
                        i--;
                        continue;
                    }
                }


                if (char.IsUpper(password[i]))
                    num_upper_letters++;
            }

            while (num_upper_letters < 2)
            {
                int upper_letter_pos = rnd.Next(password_length);
                if (upper_letter_pos == underscore_pos)
                    continue;
                password[upper_letter_pos] = big_lettes[rnd.Next(big_lettes.Length)];
                num_upper_letters++;
            }

            return new string(password);
        }

    }

    class HashTable<T>
    {
        private int _max_size;
        private List<T>[] _container;

        public HashTable(int max_size)
        {
            _max_size = max_size;
            _container = new List<T>[_max_size];
            for (int i = 0; i < _max_size; ++i)
            {
                _container[i] = new List<T>();
            }
        }

        public void Add(T s)
        {
            int hash_code = GetHash(s);
            if (!_container[hash_code].Contains(s))
                _container[hash_code].Add(s);
        }

        public void Erase(T s)
        {
            int hash_code = GetHash(s);
            _container[hash_code].Remove(s);
        }

        public bool Contains(T s)
        {
            int hash_code = GetHash(s);
            return _container[hash_code].Contains(s);
        }

        private int GetHash(T s)
        {
            return Math.Abs(s.GetHashCode()) % _max_size;
        }

    }

    public class MinValueStack
    {
        private int _max_size = 50000;
        private int _stack_top;
        private int _min_val;
        private int[] _stack;
        private int[] _min_values;

        public MinValueStack()
        {
            _stack_top = 0;
            _stack = new int[_max_size];
            _min_values = new int[_max_size];
        }

        public void Add(int value)
        {
            if (_stack_top == 0)
                _min_val = value;
            else if (_min_val > value)
                _min_val = value;

            _stack[_stack_top] = value;
            _min_values[_stack_top] = _min_val;
            _stack_top++;
        }

        public int GetMin()
        {
            return _min_values[_stack_top - 1];
        }

        public void Pop()
        {
            _stack_top--;
            if (_stack_top == 0)
                return;
            _min_val = _min_values[_stack_top - 1];
        }

        public int Top()
        {
            return _stack[_stack_top - 1];
        }


    }
    class Homework1
    {
        static void Main(string[] args)
        {
            HashTable<string> s = new HashTable<string>(100);

            Console.WriteLine(PasswordGenerator.GeneratePassword());
            Console.ReadLine();
        }
    }
}
