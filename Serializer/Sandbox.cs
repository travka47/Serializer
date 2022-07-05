namespace Serializer;

public class Program
{
    public static void Main()
    {
        Serializer serializer = new Serializer();
        var group = new Group();
        Console.WriteLine(serializer.Serialize(new Dictionary<string, string>{{"hello", "world"}, {"foo", "bar"}}));
        Console.WriteLine(serializer.Serialize(group));
    }
}

public class Group
{
    public string GroupName { get; set; } = "PIKD";
    public string GroupNumber { get; set; } = "B9121-09.03.03";
    public Student[] Students { get; set; } = {
        new(),
        new() { Age = 18 },
        new() { IsGoodStudent = false, Name = "Alina" }
    };
}

public class Student
{
    public string Name { get; set; } = "Ivan";
    public bool IsGoodStudent { get; set; } = true;
    public int Age { get; set; } = 20;
    public double Rating { get; set; } = 4.935;
}