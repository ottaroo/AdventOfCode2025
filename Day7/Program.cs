using System;
using System.IO;

namespace Day7;

class Program
{
    static void Main(string[] args)
    {
        // var path = Path.GetFullPath(".\\TestData.txt");
        var path = Path.GetFullPath(".\\PuzzleData.txt");

        var teleporter = Teleporter.Create(path);
        var splits = teleporter.CalculateBeamSplits();

        Console.WriteLine($"7a) Total number of beam splits: {splits}");

        var timelines = teleporter.FindTimeLines();
        Console.WriteLine($"7b) Total number of time lines: {timelines}");
    }
}