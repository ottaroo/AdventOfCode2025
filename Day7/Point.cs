using System;

namespace Day7;

public class Point(int x, int y) : IEquatable<Point>
{
    public int X { get; } = x;
    public int Y { get; } = y;

    public bool Equals(Point other)
    {
        if (X == other.X && Y == other.Y)
            return true;
        return false;
    }

    public override bool Equals(object? obj)
    {
        return obj is Point other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}