namespace Day6;

class Program
{
    static void Main(string[] args)
    {
        // var path = Path.GetFullPath(".\\TestData.txt");
        var path = Path.GetFullPath(".\\PuzzleData.txt");
        var math = new MathHomework();
        math.Parse(path);
        Console.WriteLine($"6a) First answer is: {math.Calculations.Sum()}");
        
        var math2 = new MathHomework();
        math2.ParseVertical(path);
        Console.WriteLine($"6b) Second answer is: {math2.Calculations.Sum()}");
        
        
    }
}