namespace Day10;

class Program
{
    static void Main(string[] args)
    {
        var path = Path.GetFullPath(".\\TestData.txt");
        // var path = Path.GetFullPath(".\\PuzzleData.txt");
        
        var machines =new List<Machine>();
        
        using var stream = new StreamReader(path);
        while (!stream.EndOfStream)
        {
            var line = stream.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
                continue;
            machines.Add(Machine.Create(line));
        }
        
        
        
    }
}