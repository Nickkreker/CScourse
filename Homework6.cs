using System;
using System.Collections.Generic;
using System.Collections;
 
// Task 1
class Lake : IEnumerable<int>
{
    private readonly int[] stones;
    public Lake(int[] stones)
    {
        this.stones = new int[stones.Length];
        Array.Copy(stones, this.stones, stones.Length);
        Array.Sort(this.stones);
    }
    public IEnumerator<int> GetEnumerator()
    {
        return new Enumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    class Enumerator : IEnumerator<int>
    {
        private bool nextJumpEven;
        private int currentIndex = -1;
        Lake collection;

        public Enumerator(Lake lakeCollection)
        {
            collection = lakeCollection;
            nextJumpEven = false;
        }

        public int Current
        {
            get
            {
                if (currentIndex == -1)
                    throw new InvalidOperationException("Enumeration not started");
                if (currentIndex == collection.stones.Length)
                    throw new InvalidOperationException("Past end of list");
                return collection.stones[currentIndex];
            }
        }

        public bool MoveNext()
        {
            if (!nextJumpEven)
            {
                currentIndex++;
                while (true)
                {
                    if (currentIndex >= collection.stones.Length)
                    {
                        nextJumpEven = true;
                        break;
                    }
                    if (collection.stones[currentIndex] % 2 == 1)
                        return true;
                    currentIndex++;
                }
            }

            if (nextJumpEven)
            {
                currentIndex--;
                while (true)
                {
                    if (currentIndex == -1)
                    {
                        currentIndex = collection.stones.Length;
                        return false;
                    }
                    if (collection.stones[currentIndex] % 2 == 0)
                        return true;
                    currentIndex--;
                }
            }
            return false;
        }
    
        public void Reset() { currentIndex = -1; }

        void IDisposable.Dispose() { }

        object IEnumerator.Current
        {
            get { return Current; }
        }
    }
}

// Task 2
class Person
{
    public string name;
    public byte age;

    public Person(string name, byte age)
    {
        this.name = name;
        this.age = age;
    }
}

class NameComparer : IComparer<Person>
{
    public int Compare (Person x, Person y)
    {
        if (x.name.Length != y.name.Length)
            return x.name.Length.CompareTo(y.name.Length);

        char xfl = x.name.ToUpper()[0];
        char yfl = y.name.ToUpper()[0];
        if (xfl != yfl)
            return xfl.CompareTo(yfl);
        return 0;
    }
}

class AgeComparer : IComparer<Person>
{
    public int Compare(Person x, Person y)
    {
        if (x.age != y.age)
            return x.age.CompareTo(y.age);
        return 0;
    }
}

// Task 3
class CustomLinkedList<T> : IEnumerable<T> where T : IEquatable<T>
{
    ListNode<T> _begin;
    ListNode<T> _end;
    uint _size;

    public CustomLinkedList()
    {
        _begin = null;
        _end = null;
        _size = 0;
    }

    public void Add(T val)
    {
        if (_begin is null)
        {
            _begin = new ListNode<T>(val);
            _end = _begin;
            _size = 1;
            return;
        }

        ListNode<T> ln = new ListNode<T>(val);
        ln.Previous = _end;
        _end.Next = ln;
        _end = ln;
        _size++;
    }

    public bool Remove(T val)
    {
        if (_size == 0)
            return false;

        ListNode<T> ln = _begin;
        if (ln.Value.Equals(val))
        {
            if (_size == 1)
            {
                _begin = null;
                _end = null;
                _size = 0;
                return true;
            }

            ln.Next.Previous = null;
            _begin = ln.Next;
            _size--;
            return true;
        }


        while (ln.Next != null)
        {
            if (ln.Value.Equals(val))
            {
                ln.Previous.Next = ln.Next;
                ln.Next.Previous = ln.Previous;
                _size--;
                return true;
            }
            ln = ln.Next;
        }

        if (ln.Value.Equals(val))
        {
            ln.Previous.Next = null;
            _end = ln.Previous;
            _size--;
            return true;
        }
        return false;

    }

    public uint Count()
    {
        return _size;
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (ListNode<T> ln = _begin; ln != null; ln = ln.Next)
        {
            yield return ln.Value;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

class ListNode<T>
{
    public T Value;
    public ListNode<T> Next;
    public ListNode<T> Previous;

    public ListNode(T val)
    {
        Value = val;
        Next = null;
        Previous = null;
    }
}

// Task 4
public class Joiner
{
    string pathToTableA;
    string pathToTableB;
    List<string> joinedTable;

    public Joiner(string pathA, string pathB)
    {
        if (!System.IO.File.Exists(pathA))
        {
            throw new System.IO.FileNotFoundException(String.Format("file {0} not found",pathA));
        }
        if (!System.IO.File.Exists(pathB))
        {
            throw new System.IO.FileNotFoundException(String.Format("file {0} not found", pathB));
        }
        pathToTableA = pathA;
        pathToTableB = pathB;

        string[] tableALines = System.IO.File.ReadAllLines(pathToTableA);
        string[] tableBLines = System.IO.File.ReadAllLines(pathToTableB);

        SortedDictionary<string, List<string>> dict = new SortedDictionary<string, List<string>>();
        foreach (string tableLine in tableBLines)
        {
            string key = tableLine.Split("\t")[0];
            string fields = String.Join("\t",tableLine.Split("\t")[1..]);
            if (!dict.ContainsKey(key))
            {
                dict.Add(key, new List<string>());
                dict[key].Add(fields);
            }
            else
                dict[key].Add(fields);
        }

        joinedTable = new List<string>();
        foreach (string tableALine in tableALines)
        {
            string key = tableALine.Split("\t")[0];
            if (!dict.ContainsKey(key))
                continue;
            foreach (string tableBLine in dict[key])
            {
                joinedTable.Add(tableALine + "\t" + tableBLine);
            }
        }
    }

    public static (string, string) GenerateExampleTables()
    {
        string fileA = "A\tw\tp\nB\tx\tq\nB\ty\tr\nC\tz\ts\n";
        string fileB = "A\t1\nA\t2\nB\t3\n";

        System.IO.File.WriteAllText("fileA.txt", fileA);
        System.IO.File.WriteAllText("fileB.txt", fileB);

        return ("fileA.txt", "fileB.txt");
    }

    public static void DeleteExampleTables()
    {
        System.IO.File.Delete("fileA.txt");
        System.IO.File.Delete("fileB.txt");
    }

    public void Display()
    {
        foreach (string tableLine in joinedTable)
        {
            Console.WriteLine(tableLine);
        }
    }
}

public class Homework6
{
    public static void Main()
    {
        Console.WriteLine("Task 1");
        int[] stones = { 1, 2, 3, 10, 4, 5, 6, 7, 8 };
        Console.Write("stones: ");
        foreach (int stone in stones)
        {
            Console.Write(String.Format("{0} ", stone));
        }

        Console.Write("\njump order: ");
        Lake lake = new Lake(stones);
        foreach (int stone in lake)
        {
            Console.Write("{0} ", stone);
        }
        Console.Write("\n\n");


        stones = new int[] { 2, 10, 4, 0, -8 };
        Console.Write("stones: ");
        foreach (int stone in stones)
        {
            Console.Write(String.Format("{0} ", stone));
        }
        Console.Write("\njump order: ");
        lake = new Lake(stones);
        foreach (int stone in lake)
        {
            Console.Write("{0} ", stone);
        }
        Console.Write("\n\n");


        stones = new int[] { 1 };
        Console.Write("stones: ");
        foreach (int stone in stones)
        {
            Console.Write(String.Format("{0} ", stone));
        }
        Console.Write("\njump order: ");
        lake = new Lake(stones);
        foreach (int stone in lake)
        {
            Console.Write("{0} ", stone);
        }
        Console.Write("\n\n");

        Console.WriteLine("Task 2");
        Person p1 = new Person("John", 45);
        Person p2 = new Person("john", 11);
        NameComparer nc = new NameComparer();
        Console.WriteLine(String.Format("Compare ('John', 45) to ('john', 11) using NameComparer(): result={0}",
                                        nc.Compare(p2, p1)));
        Console.WriteLine(String.Format("Compare ('John', 45) to ('Alex', 13) using NameComparer(): result={0}",
                                        nc.Compare(new Person("John", 45), new Person("Alex", 13))));
        Console.WriteLine(String.Format("Compare ('John', 45) to ('Gustav', 22) using NameComparer(): result={0}\n",
                                        nc.Compare(new Person("John", 45), new Person("Gustav", 22))));
        AgeComparer ac = new AgeComparer();
        Console.WriteLine(String.Format("Compare ('John', 45) to ('Gustav', 22) using AgeComparer(): result={0}",
                                        ac.Compare(new Person("John", 45), new Person("Gustav", 22))));
        Console.WriteLine(String.Format("Compare ('John', 45) to ('Joe', 45) using AgeComparer(): result={0}",
                                        ac.Compare(new Person("John", 45), new Person("Joe", 45))));
        Console.WriteLine(String.Format("Compare ('John', 45) to ('Michael', 67) using AgeComparer(): result={0}\n",
                                        ac.Compare(new Person("John", 45), new Person("Miachel", 67))));

        
        Console.WriteLine("Task 3");
        CustomLinkedList<int> ll = new CustomLinkedList<int>();
        ll.Add(11);
        ll.Add(12);
        ll.Add(89);
        ll.Add(12);
        ll.Remove(12);
        foreach (int val in ll)
        {
            Console.Write(String.Format("{0} ", val));
        }
        Console.WriteLine();

        ll = new CustomLinkedList<int>();
        ll.Add(11);
        ll.Remove(11);
        foreach (int val in ll)
        {
            Console.Write(String.Format("{0} ", val));
        }
        Console.WriteLine();

        ll = new CustomLinkedList<int>();
        ll.Add(11);
        ll.Add(12);
        ll.Add(122);
        ll.Remove(122);
        foreach (int val in ll)
        {
            Console.Write(String.Format("{0} ", val));
        }
        Console.WriteLine();

        Console.WriteLine("Task 4");
        (string pathA, string pathB) = Joiner.GenerateExampleTables();
        Console.WriteLine("First table:");
        Console.Write(System.IO.File.ReadAllText(pathA));
        Console.WriteLine("Second table:");
        Console.Write(System.IO.File.ReadAllText(pathB));
        Console.WriteLine("Their join:");
        Joiner joiner = new Joiner(pathA, pathB);
        joiner.Display();
        Joiner.DeleteExampleTables();
    }
}
