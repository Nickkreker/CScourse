using System;
using System.Collections.Generic;
using System.Linq;

class Person
{
    private string _name;
    public Person(string name)
    {
        this._name = name;
    }

    public string Name
    {
        get
        {
            return _name;
        }
    }
           
}
class Program
{
    static string TranslateWithDict(string text, Dictionary<string, string> dict, int N)
    {
        return text.Split(" ")
            .Select((w, i) => new { Index = i, Word = dict[w].ToUpper() })
            .GroupBy(x => x.Index / N)
            .Select(x => x.Select(v => v.Word))
            .Select(x => x.Aggregate((current, next) => current + " " + next))
            .Aggregate((current, next) => current + "\n" + next);
           
    }

    static string[] bucketize(string text, int n)
    {
        string[] words = text.Split(" ");
        return words
            .Skip(1)
            .Aggregate((words.ElementAt(0), new List<string>() { words.ElementAt(0) }), (current, next) =>
            {
                if (current.Item1.Length > n)
                    return (current.Item1, new List<string>());
                if (current.Item1.Length + next.Length + 1 <= n)
                {
                    current.Item2.RemoveAt(current.Item2.Count() - 1);
                    current.Item1 += (" " + next);
                    current.Item2.Add(current.Item1);
                    return current;
                }
                else
                {
                    current.Item2.Add(next);
                    current.Item1 = next;
                    if (current.Item1.Length > n)
                        return (current.Item1, new List<string>());
                    return current;
                }
            })
            .Item2
            .ToArray();
    }
    static void Main(string[] args)
    {
        Person[] persons = { new Person("Jack"), new Person("Mary"), new Person("Ashley"), new Person("Timothy"),
                             new Person("Alexander"), new Person("Nickolay"), new Person("Ann"), new Person("Mark"), };

        // Task 1
        Console.WriteLine("Task 1:");
        char del = '-';
        string query_task1 = persons
            .Skip(4)
            .Aggregate(persons.ElementAt(3).Name,
                       (current, next) =>  current + del + next.Name);

        Console.WriteLine(query_task1);

        // Task 2
        Console.WriteLine("\nTask 2:");
        IEnumerable<Person> query_task2 = Enumerable
            .Zip(persons, Enumerable.Range(1, persons.Length))
            .Where(n => n.First.Name.Length > n.Second)
            .Select(n => n.First);
        foreach (Person p in query_task2)
            Console.Write($"{p.Name} ");
        Console.WriteLine();

        // Task 3
        Console.WriteLine("\nTask 3:");
        char[] delimiterChars = { ' ', ',', '.', ':', '\t', '-' };
        String sentence = "Это что же получается: ходишь, ходишь в школу, а потом бац - вторая смена";
        Console.WriteLine(sentence);
        var wordGroups = sentence.Split(delimiterChars)
            .GroupBy(n => n.Length)
            .Select(g => new
            {
                GroupWordLength = g.Key,
                GroupSize = g.Count(),
                Words = g.Select(w => w)
            })
            .Where(g => g.GroupWordLength != 0)
            .OrderByDescending(g => g.GroupSize);

        int groupId = 1;
        foreach (var wordGroup in wordGroups)
        {
            Console.Write($"Группа {groupId}. Длина {wordGroup.GroupWordLength}. Количество {wordGroup.GroupSize}\n");
            foreach (string word in wordGroup.Words)
                Console.WriteLine(word);
            Console.WriteLine();
            groupId++;
        }

        // Task 4
        Console.WriteLine("\nTask 4:");
        Dictionary<string, string> dict = new Dictionary<string, string>()
        {
            { "Hello", "Привет"}
        };
        string text = "Hello Hello Hello Hello Hello Hello Hello";
        Console.WriteLine(TranslateWithDict(text, dict, 3));

        text = "This dog eats";
        Dictionary<string, string> dictAdvanced = new Dictionary<string, string>()
        {
            { "This", "эта"},
            { "dog", "собака" },
            { "eats", "ест" }
        };
        Console.WriteLine(TranslateWithDict(text, dictAdvanced, 1));

        // Task 5
        Console.WriteLine("\nTask 5:");
        text = "она продает морские раковины у моря";
        string[] bucketizedText = bucketize(text, 16);
        foreach (string bucket in bucketizedText)
            Console.WriteLine(bucket);

        // should return empty array []    
        text = "она продает морские раковины у моряasdddddddddddddddddddddddddddddd";
        bucketizedText = bucketize(text, 16);
        foreach (string bucket in bucketizedText)
            Console.WriteLine(bucket);

        text = "a b c d e ";
        bucketizedText = bucketize(text, 2);
        foreach (string bucket in bucketizedText)
            Console.WriteLine(bucket);

        text = "мышь прыгнула через сыр";
        bucketizedText = bucketize(text, 8);
        foreach (string bucket in bucketizedText)
            Console.WriteLine(bucket);

    }
}
