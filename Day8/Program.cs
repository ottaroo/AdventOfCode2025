using System;
using System.IO;
using System.Linq;

namespace Day8;

class Program
{
    static void Main(string[] args)
    {
        // var path = Path.GetFullPath(".\\TestData.txt");
        var path = Path.GetFullPath(".\\PuzzleData.txt");

        var junctionBoxOrganizer = new JunctionBoxOrganizer();

        using var stream = File.OpenRead(path);
        using var reader = new StreamReader(stream);
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
                continue;
            var data = line.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            junctionBoxOrganizer.AddJunctionBox(data[0], data[1], data[2]);
        }

        junctionBoxOrganizer.Connect(1000);
        var largestCircuit = junctionBoxOrganizer.Circuits.OrderByDescending(x => x.Boxes.Count).Take(3);
        var sum = 1L;
        foreach (var circuit in largestCircuit)
        {
            sum *= circuit.Boxes.Count;
        }

        Console.WriteLine($"8a) Largest circuits multiplied: {sum}");

        junctionBoxOrganizer.Connect(0); // 0 - all
        var sudoDistance = (long) junctionBoxOrganizer.LastConnectedJunctionBoxes.Boxes.First().Location.X * (long) junctionBoxOrganizer.LastConnectedJunctionBoxes.Boxes.Last().Location.X;
        Console.WriteLine($"8b) Sudo distance of last two junction boxes: {sudoDistance}");
    }
}