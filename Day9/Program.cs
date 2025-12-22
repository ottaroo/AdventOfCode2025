namespace Day9;

class Program
{
    static void Main(string[] args)
    {
        // var path = Path.GetFullPath(".\\TestData.txt");
        var path = Path.GetFullPath(".\\PuzzleData.txt");

        var tiles = new List<MapPoint>();

        using var stream = File.OpenRead(path);
        using var reader = new StreamReader(stream);
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
                continue;
            var values = line.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(int.Parse).ToArray();
            tiles.Add(new MapPoint(values[0], values[1]));
        }

        var theather = new MovieTheather(tiles);
        theather.GetLargestArea(out var a, out var b, out var area);

        Console.WriteLine($"9a) Largest area is: {area} [{a.X},{a.Y} - {b.X},{b.Y}]");

        theather.GetLargestArea2(out a, out b, out area);
        Console.WriteLine($"9b) Largest area is: {area} [{a.X},{a.Y} - {b.X},{b.Y}]");
    }
}