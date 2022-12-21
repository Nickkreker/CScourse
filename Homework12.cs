using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

class Cylinder
{

}

abstract class Engine
{
    private Cylinder[] cylinders; // Composition

    public Engine(int numberOfCylinders)
    {
        cylinders = new Cylinder[numberOfCylinders];
        for (int i = 0; i < numberOfCylinders; ++i)
        {
            cylinders[i] = new Cylinder();
        }
    }

    public int GetNumberOfCylinders()
    {
        return cylinders.Length;
    }

    public abstract void MakeSound();
}

class V8Engine : Engine
{
    public V8Engine() : base(8)
    {
        
    }

    public override void MakeSound() // Polymorhism
    {
        Console.WriteLine("Engine makes cool agressive sound");
    }
}

class V4Engine : Engine
{
    public V4Engine() : base(4)
    {

    }

    public override void MakeSound() // Polymorhism
    {
        Console.WriteLine("Engine makes standard car sound");
    }
}

class CarBody
{
    private string color;
    private string id;

    public CarBody(string color, string id)
    {
        this.color = color;
        this.id = id;
    }

    public string GetColor() // Encapsulation
    {
        return color;
    }

    public string GetId()
    {
        return id;
    }
}

class Chassis
{

}

class StereoSystem
{

}

class Dashboard
{

}

class Transmission
{

}

class AutomaticTransmission : Transmission
{

}

class ManualTransmission : Transmission
{

}

class Car
{
    private Engine engine;
    private CarBody body;
    private Chassis chassis;
    private Transmission transmission;
    private Dashboard dashboard;
    private StereoSystem stereoSystem;

    public Car(Engine engine, Chassis chassis, Transmission transmission, string color)
    {
        this.engine = engine; // Aggregation
        this.chassis = chassis;
        this.transmission = transmission;

        Guid guid = Guid.NewGuid();
        string id = guid.ToString();
        body = new CarBody(color, id);

        dashboard = new Dashboard();
        stereoSystem = new StereoSystem();
    }

    public void MakeSound()
    {
        engine.MakeSound();
    }

    public string GetBodyId()
    {
        return body.GetId();
    }
}

// Task 2
[Flags]
enum Allergen
{
    Eggs            = 0b_0000_0001,
    Peanuts         = 0b_0000_0010,
    Shellfish       = 0b_0000_0100,
    Strawberries    = 0b_0000_1000,
    Tomatoes        = 0b_0001_0000,
    Chocolate       = 0b_0010_0000,
    Pollen          = 0b_0100_0000,
    Cats            = 0b_1000_0000,
}

class Allergies
{
    private string name;
    private Allergen allergies;

    public Allergies(string name, uint allergiesScore = 0)
    {
        this.name = name;

        if (allergiesScore > 257)
            throw new ArgumentOutOfRangeException($"allergiesScore should be <= 257, provided {allergiesScore}");
        allergies = (Allergen) allergiesScore;
    }

    public Allergies(string name, string allergens)
    {
        this.name = name;
        this.allergies = 0;

        foreach (string allergen in allergens.Split())
        {
            bool success = Enum.TryParse(allergen, out Allergen allergenEnum);
            if (!success)
                throw new ArgumentException($"No such allergen {allergen}");
            allergies |= allergenEnum;
        }
    }

    public int Score
    {
        get { return ((int)allergies); }
    }

    public string Name
    {
        get { return name; }
    }

    public override string ToString()
    {
        if (allergies == 0)
            return $"{name} has no allergies!";

        string[] allergens = allergies.ToString().Split(",");
        if (allergens.Length == 1)
            return $"{name} is allergic to {allergies}";
        else
        {
            string allergiesBegin = String.Join(",", allergens.Take(allergens.Length - 1));
            return $"{name} is allergic to {allergiesBegin} and{allergens[allergens.Length - 1]}";
        }
    }

    public bool IsAllergicTo(string allergen)
    {
        bool success = Enum.TryParse(allergen, out Allergen allergenEnum);
        if (!success)
            throw new ArgumentException($"allergen {allergen} does not exist");

        return (allergies & allergenEnum) == allergenEnum;
    }

    public bool IsAllergicTo(Allergen allergen)
    {
        return (allergies & allergen) == allergen;
    }

    public void AddAllergy(string allergen)
    {
        bool success = Enum.TryParse(allergen, out Allergen allergenEnum);
        if (!success)
            throw new ArgumentException($"allergen {allergen} does not exist");

        allergies |= allergenEnum;
    }

    public void AddAllergy(Allergen allergen)
    {
        allergies |= allergen;
    }

    public void DeleteAllergy(Allergen allergen)
    {
        if (!allergies.HasFlag(allergen))
            return;
        allergies -= allergen;
    }
}



public class Homework12
{
    public static void Main()
    {
        // Task 1
        Console.WriteLine("Task 1");

        Engine v8engine = new V8Engine();
        Engine v4engine = new V4Engine();
        Chassis chassis = new Chassis();
        Transmission manualTransmission = new ManualTransmission();
        Transmission automaticTransmission = new AutomaticTransmission();

        Car v8CarWithManualTransmission = new Car(v8engine, chassis, manualTransmission, "red");
        Console.WriteLine($"v8CarWithManualTransmission id: {v8CarWithManualTransmission.GetBodyId()}");
        v8CarWithManualTransmission.MakeSound();

        Car v4CarWithManualTransmission = new Car(v4engine, chassis, manualTransmission, "black");
        Console.WriteLine($"v4CarWithManualTransmission id: {v4CarWithManualTransmission.GetBodyId()}");
        v4CarWithManualTransmission.MakeSound();

        Car v4CarWithAutomaticTransmission = new Car(v4engine, chassis, automaticTransmission, "white");
        Console.WriteLine($"v4CarWithAutomaticTransmission id: {v4CarWithAutomaticTransmission.GetBodyId()}");
        v4CarWithManualTransmission.MakeSound();

        // Task 2
        Console.WriteLine("\nTask 2");

        var mary = new Allergies("Mary");
        var joe = new Allergies("Joe", 123);
        var jack = new Allergies("Jack", 65);
        var rob = new Allergies("Rob", "Peanuts Chocolate Cats Strawberries");
        Console.WriteLine(mary.ToString());
        Console.WriteLine(joe.ToString());
        Console.WriteLine(jack.ToString());
        Console.WriteLine(rob.ToString());

        rob.DeleteAllergy(Allergen.Peanuts);
        rob.DeleteAllergy(Allergen.Tomatoes);
        Console.WriteLine(rob.ToString());

        Console.WriteLine($"Is Mary allergic to Peanuts? {mary.IsAllergicTo("Peanuts")}");
        mary.AddAllergy(Allergen.Shellfish);
        Console.WriteLine($"Is Mary allergic to Shellfish? {mary.IsAllergicTo(Allergen.Shellfish)}");

        Console.WriteLine($"Jask score is: {jack.Score}");
    }
}
