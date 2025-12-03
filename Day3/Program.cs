namespace Day3;

class Program
{
    static async Task Main(string[] args)
    {
        // var path = Path.GetFullPath(".\\TestData.txt");
        var path = Path.GetFullPath(".\\PuzzleData.txt");
        var batteries = new Batteries();
        var sum = await batteries.GetJoltage(path);
        Console.WriteLine($"3a) answer: {sum}");
        var megaBattiers = new MegaBatteries();
        var megaSum = await megaBattiers.GetJoltage(path);
        Console.WriteLine($"3b) answer: {megaSum}");
        
        
        
    }
}