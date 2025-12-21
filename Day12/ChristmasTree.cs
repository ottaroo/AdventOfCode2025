namespace Day12;

public class ChristmasTree
{
    public int Width { get; init; }
    public int Length { get; init; }
    
    // Key: Shape ID Value: number of packages of shape
    public Dictionary<string, int> AllocatedPackageSlots { get; } = [];

    public bool TestIfPackagesCanFit(List<Shape> shapes)
    {
        var totalAreaNeeded = AllocatedPackageSlots.Values.Sum() * 7; // All shapes takes up 7 units of space
        if (totalAreaNeeded > Width * Length)
            return false;

        // do a check to fit in shapes
        var shapesInUse = new List<Shape>();
        foreach(var key in AllocatedPackageSlots.Keys)
        {
            if (AllocatedPackageSlots[key] > 0)
            {
                var shape = shapes.First(x=>x.Id == key);
                shape.ClearCopyCounter();
                for(var n = 0; n < AllocatedPackageSlots[key]; n++)
                    shapesInUse.Add(shape.Copy());
            }
        }
   
        var solver = new PackageAllocationSolver(Width, Length, shapesInUse);
        var solution = solver.FindOneSolution();
        return solution != null;


    }
    
    public static ChristmasTree Create(string line)
    {
        var width = int.Parse(line.Substring(0, line.IndexOf('x')));
        var length = int.Parse(line.Substring(line.IndexOf('x') + 1,  line.IndexOf(':') - (line.IndexOf('x') + 1)));
        var allotments = line.Substring(line.IndexOf(':') + 1).Trim().Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        var tree = new ChristmasTree
        {
            Width = width,
            Length = length
        };
        for (var n = 0; n < allotments.Length; n++)
        {
            tree.AllocatedPackageSlots.Add(n.ToString(), allotments[n]);
        }

        return tree;
    }
}