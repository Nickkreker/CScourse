using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
class Group
{
    public decimal GroupId { get; set; }
    public string Name { get; set; }
    public List<Student> Students { get; set; }
    // no need to serialize this
    [field: NonSerialized]
    public int StudentsCount { get; set; }
}

[Serializable]
class Student
{
    public decimal StudentId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public Group Group { get; set; }
}


public class Homework9
{
    public static void Main()
    {
        // Task 1
        using (FileStream fs = File.Create("test.txt"))
        using (TextWriter writer = new StreamWriter(fs, Encoding.ASCII))
        {
            Random rnd = new Random();
            int upperBound = 100;
            int numLength = (upperBound - 1).ToString().Length;
            for (int i = 0; i < upperBound; ++i)
            {
                writer.WriteLine(i.ToString().PadLeft(numLength, '0'));
            }
            writer.Flush();

            int count = upperBound - 1;
            int lineSepLength = Environment.NewLine.Length;
            byte[] buf = new byte[numLength];
            for (int i = 0; i < upperBound; ++i)
            {
                int index = rnd.Next(0, count);
                fs.Seek((numLength + lineSepLength) * index, SeekOrigin.Begin);
                fs.Read(buf, 0, numLength);
                int v1 = int.Parse(System.Text.Encoding.ASCII.GetString(buf, 0, numLength));

                fs.Seek((numLength + lineSepLength) * count, SeekOrigin.Begin);
                fs.Read(buf, 0, numLength);
                int v2 = int.Parse(System.Text.Encoding.ASCII.GetString(buf, 0, numLength));

                fs.Seek((numLength + lineSepLength) * index, SeekOrigin.Begin);
                fs.Write(Encoding.ASCII.GetBytes(v2.ToString().PadLeft(numLength, '0')));

                fs.Seek((numLength + lineSepLength) * count, SeekOrigin.Begin);
                fs.Write(Encoding.ASCII.GetBytes(v1.ToString().PadLeft(numLength, '0')));
                count--;
            }
        }

        // Task 2
        Group g = new Group();
        g.Name = "Math group";
        g.GroupId = 1;
        g.StudentsCount = 2;
        g.Students = new List<Student>();

        Student s1 = new Student
        {
            StudentId = 1,
            FirstName = "John",
            LastName = "Peters",
            Age = 22,
            Group = g
        };

        Student s2 = new Student
        {
            StudentId = 2,
            FirstName = "Mike",
            LastName = "Douglas",
            Age = 21,
            Group = g
        };

        g.Students.Add(s1);
        g.Students.Add(s2);

        Group unmarshalledGroup = null;
        using (FileStream fs = File.Create("group.txt"))
        {
            BinaryFormatter binaryFormatter =  new BinaryFormatter();
            binaryFormatter.Serialize(fs, g);
            fs.Flush();
            fs.Seek(0, SeekOrigin.Begin);
            unmarshalledGroup = (Group) binaryFormatter.Deserialize(fs);
        }
        Console.WriteLine(unmarshalledGroup.GroupId);
        Console.WriteLine(unmarshalledGroup.Name);
        Console.WriteLine(unmarshalledGroup.StudentsCount);
        Console.WriteLine();
        Console.WriteLine(unmarshalledGroup.Students[0].StudentId);
        Console.WriteLine(unmarshalledGroup.Students[0].FirstName);
        Console.WriteLine(unmarshalledGroup.Students[0].LastName);
        Console.WriteLine(unmarshalledGroup.Students[0].Age);
        Console.WriteLine(unmarshalledGroup.Students[0].Group.Name);
        Console.WriteLine();
        Console.WriteLine(unmarshalledGroup.Students[1].StudentId);
        Console.WriteLine(unmarshalledGroup.Students[1].FirstName);
        Console.WriteLine(unmarshalledGroup.Students[1].LastName);
        Console.WriteLine(unmarshalledGroup.Students[1].Age);
        Console.WriteLine(unmarshalledGroup.Students[1].Group.Name);


        // Task 4
        Console.WriteLine(findFile("Program.cs"));
    }

    public static string findFile(string file)
    {
        List<String> pathsToFile = new List<String>();

        foreach (DriveInfo d in DriveInfo.GetDrives())
        {
            if (!d.IsReady)
                continue;
            DirectoryInfo rootDir = d.RootDirectory;
            WalkDirectoryTree(rootDir, file, pathsToFile);
        }

        if (pathsToFile.Count == 0)
            return null;
        else
            return pathsToFile[0];
    }

    private static void WalkDirectoryTree(DirectoryInfo root, string file, List<String> pathsToFile)
    {
        List<FileInfo> files = new List<FileInfo>();
        try
        {
            files.AddRange(root.GetFiles($"{file}.*"));
            files.AddRange(root.GetFiles($"{file}"));
        }
        catch (UnauthorizedAccessException e)
        { }
        catch (DirectoryNotFoundException e)
        { }

        if (files.Count != 0)
        {
            pathsToFile.Add(files[0].FullName);
            return;
        }
        try
        {
            DirectoryInfo[] subDirs = root.GetDirectories();
            foreach (DirectoryInfo dirInfo in subDirs)
            {
                WalkDirectoryTree(dirInfo, file, pathsToFile);
                if (pathsToFile.Count != 0)
                    return;
            }
        }
        catch (Exception e) { }
    }
}
