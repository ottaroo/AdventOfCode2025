namespace Day12;

public class BoolArrayComparer : IEqualityComparer<bool[,]>
{
    public bool Equals(bool[,]? a, bool[,]? b)
    {
        if (a == null && b == null)
            return true;
        if (a == null || b == null)
            return false;
        if (ReferenceEquals(a, b))
            return true;

        if (a.GetLength(0) != b.GetLength(0) ||
            a.GetLength(1) != b.GetLength(1))
            return false;

        for (var y = 0; y < a.GetLength(0); y++)
        for (var x = 0; x < a.GetLength(1); x++)
            if (a[y, x] != b[y, x])
                return false;

        return true;
    }

    public int GetHashCode(bool[,] obj)
    {
        var hash = 17;
        foreach (var v in obj)
            hash = hash * 31 + (v ? 1 : 0);
        return hash;
    }
}