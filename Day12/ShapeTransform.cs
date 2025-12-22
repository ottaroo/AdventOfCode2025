namespace Day12;

public static class ShapeTransform
{
    private static bool[,] FlipHorizontal(bool[,] src)
    {
        int h = src.GetLength(0);
        int w = src.GetLength(1);

        bool[,] dst = new bool[h, w];

        for (int y = 0; y < h; y++)
        for (int x = 0; x < w; x++)
            dst[y, w - x - 1] = src[y, x];

        return dst;
    }

    public static List<bool[,]> GenerateOrientations(bool[,] shape)
    {
        var result = new List<bool[,]>();

        // Original rotations
        foreach (var rot in GenerateRotations(shape))
            result.Add(rot);

        // Flipped shape rotations
        var flipped = FlipHorizontal(shape);
        foreach (var rot in GenerateRotations(flipped))
            result.Add(rot);

        // Remove duplicates (symmetry handling)
        return result.Distinct(new BoolArrayComparer()).ToList();
    }

    private static List<bool[,]> GenerateRotations(bool[,] shape)
    {
        var rotations = new List<bool[,]>();

        bool[,] current = shape;
        for (int i = 0; i < 4; i++)
        {
            rotations.Add(current);
            current = Rotate90(current);
        }

        return rotations;
    }

    private static bool[,] Rotate90(bool[,] src)
    {
        int h = src.GetLength(0);
        int w = src.GetLength(1);

        bool[,] dst = new bool[w, h];

        for (int y = 0; y < h; y++)
        for (int x = 0; x < w; x++)
            dst[x, h - y - 1] = src[y, x];

        return dst;
    }
}