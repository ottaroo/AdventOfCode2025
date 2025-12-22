using System;
using System.Numerics;

namespace Day8;

public class JunctionBox(Vector3 location) : IEquatable<JunctionBox>, IComparable<JunctionBox>
{
    public Vector3 Location { get; } = location;

    public int CompareTo(JunctionBox? other)
    {
        if (other is null) return 1;

        if (Math.Abs(Location.X - other.Location.X) > float.Epsilon)
            return Location.X.CompareTo(other.Location.X);
        if (Math.Abs(Location.Y - other.Location.Y) > float.Epsilon)
            return Location.Y.CompareTo(other.Location.Y);
        if (Math.Abs(Location.Z - other.Location.Z) <= float.Epsilon)
            return 0;
        return Location.Z.CompareTo(other.Location.Z);
    }

    public bool Equals(JunctionBox? other)
    {
        if (other is null) return false;
        if (Math.Abs(Location.X - other.Location.X) > float.Epsilon)
            return false;
        if (Math.Abs(Location.Y - other.Location.Y) > float.Epsilon)
            return false;
        if (Math.Abs(Location.Z - other.Location.Z) <= float.Epsilon)
            return true;
        return false;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj.GetType() != GetType()) return false;
        return Equals((JunctionBox) obj);
    }

    public override int GetHashCode()
    {
        return Location.GetHashCode();
    }
}