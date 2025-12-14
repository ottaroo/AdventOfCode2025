using System;
using System.Collections.Generic;

namespace Day7;

public static class Coordinates
{
    public class Direction
    {
        private Direction() { }

        public Point Value { get; private init; } 

        public static Direction Left => new() {Value = new(-1, 0)};
        public static Direction Right => new() {Value = new(1, 0)};
        public static Direction Up => new() {Value = new(0, -1)};
        public static Direction Down => new() {Value = new(0, 1)};
        public static Direction LeftUp => new() {Value = new(-1, -1)};
        public static Direction LeftDown => new() {Value = new(-1, 1)};
        public static Direction RightUp => new() {Value = new(1, -1)};
        public static Direction RightDown => new() {Value = new(1, 1)};
    }

    public static Point GetMapPoint(Point source,  Direction direction, int distance = 1) => new(source.X + (direction.Value.X * Math.Abs( distance)), source.Y + (direction.Value.Y * Math.Abs(distance))); 

    public static IEnumerable<Direction> Directions
    {
        get
        {
            yield return Direction.Left;
            yield return Direction.LeftUp;
            yield return Direction.Up;
            yield return Direction.RightUp;
            yield return Direction.Right;
            yield return Direction.RightDown;
            yield return Direction.Down;
            yield return Direction.LeftDown;
        }
    }

    extension(Point point)
    {
        public Point UpdateMapPoint(Direction direction) => new (point.X + direction.Value.X, point.Y + direction.Value.Y);
    }
    
    
}