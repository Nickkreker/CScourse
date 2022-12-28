using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

public delegate void BusEvent(int id);
class EventBus
{
    private event BusEvent events = null; 
    public EventBus() { }

    public void Post(int id)
    {
        events?.Invoke(id);
    }

    public void Subscribe(BusEvent busEvent)
    {
        events += busEvent;
    }

    public void Unsubscribe(BusEvent busEvent)
    {
        events -= busEvent;
    }
}

class Reader
{
    private int id;
    public Reader(int id)
    {
        this.id = id;
    }

    public void OnEvent(int authorId)
    {
        Console.WriteLine($"{id} got message from {authorId}");
    }
}

class Writer
{
    private int id;
    private EventBus eventBus;

    public Writer(int id, EventBus eventBus)
    {
        this.id = id;
        this.eventBus = eventBus;
    }

    public void PostEvent()
    {
        eventBus.Post(id);
    }
}

class PseudoStack<T>
{
    List<Stack<T>> pseudoStack;
    private int maxSize;

    public PseudoStack(int maxSize)
    {
        pseudoStack = new List<Stack<T>>();
        this.maxSize = maxSize;
    }

    public void Push(T value)
    {
        if (pseudoStack.Count == 0 || pseudoStack.Last().Count == maxSize)
            pseudoStack.Add(new Stack<T>());
        pseudoStack.Last().Push(value);
    }

    public T Pop()
    {
        if (pseudoStack.Count == 0)
            throw new Exception("Stack empty");
        T res = pseudoStack.Last().Pop();
        if (pseudoStack.Last().Count == 0)
        {
            pseudoStack.RemoveAt(pseudoStack.Count - 1);
        }
        return res;
    }
}


public class Homework5
{
    public static void Main()
    {
        // Task 1
        Console.WriteLine("Task 1");
        EventBus eventBus = new EventBus();
        Writer writer1 = new Writer(123, eventBus);
        writer1.PostEvent();

        Reader reader1 = new Reader(321);
        eventBus.Subscribe(reader1.OnEvent);
        writer1.PostEvent();

        Reader reader2 = new Reader(345);
        eventBus.Subscribe(reader2.OnEvent);

        Writer writer2 = new Writer(777, eventBus);
        writer2.PostEvent();

        eventBus.Unsubscribe(reader1.OnEvent);
        writer2.PostEvent();

        // Task 2
        Console.WriteLine("Task 2");
        PseudoStack<int> stack = new PseudoStack<int>(3);
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);
        stack.Push(4);
        Console.WriteLine(stack.Pop()); // 4
        Console.WriteLine(stack.Pop()); // 3
        Console.WriteLine(stack.Pop()); // 2
    } 
}
