namespace Day12;

class Program
{
    static void Main(string[] args)
    {
        // var path = Path.GetFullPath(".\\TestData.txt");
        var path = Path.GetFullPath(".\\PuzzleData.txt");
        var lines = File.ReadAllLines(path);
        var shapes = new List<Shape>();
        var treeAllocations = new List<ChristmasTree>();

        for (var n = 0; n < lines.Length; n++)
        {
            var line = lines[n];
            if (string.IsNullOrWhiteSpace(line)) continue;

            if (line.Contains(':') && line.Contains('x'))
            {
                treeAllocations.Add(ChristmasTree.Create(line, n));
                continue;
            }

            if (!line.Trim().EndsWith(":"))
                continue;
            var id = line.Trim(':');
            var map = new char[3][];
            line = lines[++n];
            map[0] = line.Trim().ToCharArray();
            line = lines[++n];
            map[1] = line.Trim().ToCharArray();
            line = lines[++n];
            map[2] = line.Trim().ToCharArray();
            shapes.Add(new Shape(id, id, map));
        }

        foreach (var tree in treeAllocations)
        {
            var area = tree.Width * tree.Length;
            var packages = tree.AllocatedPackageSlots.Values.Sum() * 7;
            if (area == packages)
            {
                Console.WriteLine($"{tree.ParsedFromLine}");
            }
        }

        var validRegions = 0;
        var index = 0;
        foreach (var tree in treeAllocations)
        {
            if (tree.TestIfPackagesCanFit(shapes))
                validRegions++;
        }

        Console.WriteLine($"12a) Number of valid regions: {validRegions}");
    }
}