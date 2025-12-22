using System.Text;

namespace Day4;

class Program
{
    static void Main(string[] args)
    {
        // var path = Path.GetFullPath(".\\TestData.txt");
        var path = Path.GetFullPath(".\\PuzzleData.txt");

        var input = File.ReadAllLines(path).Select(x => x.ToCharArray()).ToArray();
        var forklift = new Forklift();
        var points = forklift.FindAvailableRollsOfPaper(input);
        Console.WriteLine($"4a) Number of paper rolls available: {points.Count()}");

        var canRemoveInTotal = points.Length;
        forklift.MoveRollsOfPaper(ref input, points);

        while (true)
        {
            points = forklift.FindAvailableRollsOfPaper(input);
            if (!points.Any())
                break;
            canRemoveInTotal += points.Count();
            forklift.MoveRollsOfPaper(ref input, points);
        }

        Console.WriteLine($"4b) Total number of paper rolls movable: {canRemoveInTotal}");
    }
}