using System.Collections.Concurrent;

namespace Day10;

class Program
{
    static void Main(string[] args)
    {
        // var path = Path.GetFullPath(".\\TestData.txt");
        var path = Path.GetFullPath(".\\PuzzleData.txt");

        var machines = new List<Machine>();

        using var stream = new StreamReader(path);
        while (!stream.EndOfStream)
        {
            var line = stream.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
                continue;
            machines.Add(Machine.Create(line));
        }


        var sum = 0L;
        Parallel.ForEach(machines, machine =>
        {
            var n = (long) machine.PushButtons();
            Interlocked.Add(ref sum, n);
        });
        Console.WriteLine($"10a) Number of buttons pushed: {sum}");
        
        var sumJolts = 0L;
        Parallel.ForEach(machines, machine =>
            {
                var n = (long) machine.FindTheLeastAmountOfButtonsToPush();

                Interlocked.Add(ref sumJolts, n);

            }
        );

        Console.WriteLine($"10b) Number of buttons pushed: {sumJolts}");
    }
}