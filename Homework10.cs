using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class BlackBox
{
    private int innerValue;
    private BlackBox(int innerValue)
    {
        this.innerValue = 0;
    }
    private BlackBox()
    {
        // this.innerValue = DefaultValue;
        this.innerValue = default;
    }
    private void Add(int addend)
    {
        this.innerValue += addend;
    }
    private void Subtract(int subtrahend)
    {
        this.innerValue -= subtrahend;
    }
    private void Multiply(int multiplier)
    {
        this.innerValue *= multiplier;
    }
    private void Divide(int divider)
    {
        this.innerValue /= divider;
    }
}

[AttributeUsage(AttributeTargets.All)]
public class CustomAttribute : System.Attribute
{
    private string author;
    private int revision;
    private string description;
    private string[] reviewers;
    public CustomAttribute(string author, int revision, string description, params string[] reviewers)
    {
        this.author = author;
        this.revision = revision;
        this.description = description;
        this.reviewers = reviewers;
    }

    public string GetAuthor()
    {
        return author;
    }

    public int GetRevision()
    {
        return revision;
    }

    public string GetDescription()
    {
        return description;
    }

    public string[] GetReviewers()
    {
        return reviewers;
    }
}

[Custom("Joe", 2, "Class to work with health data.", "Arnold", "Bernard")]
public class HealthScore
{
    private long v;
    public HealthScore(long value)
    {
        v = value;
    }

    [Custom("Andrew", 3, "Method to collect health data.", "Sam", "Alex")]
    public static long CalcScoreData()
    {
        return 10L;
    }

    public static void DoSomething()
    {

    }
}


public class Homework10
{
    public static void Main()
    {
        // Task 1
        Type ctorArgument = Type.GetType("System.Int32");
        ConstructorInfo ctor = typeof(BlackBox).GetTypeInfo().DeclaredConstructors.First(
            c => c.GetParameters()[0].ParameterType == ctorArgument);
        Object blackBox = ctor.Invoke(new object[] { 0 });
        string input;
        while (!string.IsNullOrEmpty(input = Console.ReadLine()))
        {
            string command = input.Split("(")[0];
            int value = int.Parse(input.Split("(")[1].Split(")")[0]);

            MethodInfo mi = blackBox.GetType().GetTypeInfo().GetDeclaredMethod(command);
            mi.Invoke(blackBox, new object[] { value });

            FieldInfo fi = blackBox.GetType().GetTypeInfo().GetDeclaredField("innerValue");
            Console.WriteLine(fi.GetValue(blackBox));
        }

        // Task 2
        Console.WriteLine("Task 2\nClass Attribute:");
        ShowAttributes(typeof(HealthScore));
        Console.WriteLine("\nMethod attribute:");
        var members = typeof(HealthScore).GetTypeInfo().DeclaredMembers.OfType<MethodBase>();
        foreach (MemberInfo member in members)
        {
            if (member.IsDefined(typeof(CustomAttribute)))
                ShowAttributes(member);
        }
    }

    private static void ShowAttributes(MemberInfo attributeTarget)
    {
        var attributes = attributeTarget.GetCustomAttributes<Attribute>();
        foreach (Attribute attribute in attributes)
        {
            if (attribute is CustomAttribute)
            {
                CustomAttribute custom = (CustomAttribute) attribute;
                Console.WriteLine($"author: {custom.GetAuthor()}");
                Console.WriteLine($"revision: {custom.GetRevision()}");
                Console.WriteLine($"description: {custom.GetDescription()}");
                Console.WriteLine($"reviewers: {string.Join(",", custom.GetReviewers())}");
            }
        }
    }
}
