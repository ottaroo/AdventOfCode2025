using System.Collections;
using System.Text;

namespace Day9;

public record struct MapPoint(int X, int Y);

public record struct Line(MapPoint Start, MapPoint End);

public static class MapPointExtensions
{
    extension(MapPoint p)
    {
        public double GetDistance(MapPoint target)
        {
            return Math.Sqrt(Math.Pow(target.X - p.X, 2) + Math.Pow(target.Y - p.Y, 2));
        }

        public bool Is(MapPoint target)
        {
            return p.X == target.X && p.Y == target.Y;
        }

        public bool IsNot(MapPoint target)
        {
            return p.X != target.X || p.Y != target.Y;
        }
    }
}

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}