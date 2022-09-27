using System;
using System.Text;

namespace myFirstProject
{
    class WaterCollector
    {
        private int[] heights;

        public WaterCollector(int[] heights)
        {
            this.heights = (int[])heights.Clone();
        }

        public (int, int) FindLargest(int l, int r)
        {
            int largestIndex = -1;
            int largest = -1;
            for (int i = l; i < r; ++i)
            {
                if (largest < heights[i])
                {
                    largest = heights[i];
                    largestIndex = i;
                }
            }

            return (largest, largestIndex);
        }
        public Int64 Solve()
        {
            var m = FindLargest(0, heights.Length);
            var t = FindLargest(m.Item2 + 1, heights.Length);
            Int64 waterSum = 0;
            while (m.Item2 != heights.Length - 1)
            {
                int waterHeight = Math.Min(m.Item1, t.Item1);
                for (int i = m.Item2 + 1; i < t.Item2; ++i)
                    waterSum += (waterHeight - heights[i]);
                m = t;
                t = FindLargest(m.Item2 + 1, heights.Length);
            }

            m = FindLargest(0, heights.Length);
            t = FindLargest(0, m.Item2);
            while (m.Item2 != 0)
            {
                int waterHeight = Math.Min(m.Item1, t.Item1);
                for (int i = t.Item2 + 1; i < m.Item2; ++i)
                    waterSum += (waterHeight - heights[i]);
                m = t;
                t = FindLargest(0, m.Item2);
            }
            return waterSum;
        }

    }

    class CustomImmutable
    {
        private int[] storage;

        public CustomImmutable(int[] arr)
        {
            storage = (int[])arr.Clone();
        }

        public int this[int i]
        {
            get
            {
                return storage[i];
            }
        }

        public int Length
        {
            get
            {
                return storage.Length;
            }
        }

        public CustomImmutable Replace(int oldInt, int newInt)
        {
            int[] newStorage = (int[])storage.Clone();
            for (int i = 0; i < newStorage.Length; ++i)
            {
                if (newStorage[i] == oldInt)
                    newStorage[i] = newInt;
            }
            return new CustomImmutable(newStorage);
        }

        public CustomImmutable Insert(int startIndex, CustomImmutable value)
        {
            int[] newStorage = (int[])storage.Clone();
            for (int i = startIndex; i < startIndex + value.Length; ++i)
            {
                if (i >= newStorage.Length)
                    break;
                newStorage[i] = value[i - startIndex];
            }
            return new CustomImmutable(newStorage);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("CustomImmutable([");
            for (int i = 0; i < storage.Length; ++i)
            {
                sb.Append(storage[i]);
                sb.Append(",");
            }
            sb.Append("])");
            return sb.ToString();
        }

    }

    class Dice
    {
        static public int diceRoll(int numDices, int result)
        {
            if (result <= 0)
                return 0;
            if (numDices == 1 && result <= 6)
                return 1;


            return diceRoll(numDices - 1, result - 1) + diceRoll(numDices - 1, result - 2) + diceRoll(numDices - 1, result - 3) +
                   diceRoll(numDices - 1, result - 4) + diceRoll(numDices - 1, result - 5) + diceRoll(numDices - 1, result - 6);
        }

    }
    class Homework2
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Dice.diceRoll(2, 5));

            Console.ReadLine();
        }
    }
}
