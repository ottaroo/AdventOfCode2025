using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;

namespace Day9;

public class MovieTheather(List<MapPoint> tiles)
{
    public List<MapPoint> Tiles { get; } = tiles;

    public void GetLargestArea(out MapPoint a, out MapPoint b, out long area)
    {
        area = 0;
        a = new MapPoint();
        b = new MapPoint();

        for (var i = 0; i < Tiles.Count; i++)
        {
            for (var j = 0; j < Tiles.Count; j++)
            {
                if (Tiles[i] == Tiles[j])
                    continue;

                var width = (long) Math.Abs(Tiles[i].X - Tiles[j].X) + 1; // inclusive
                var height = (long) Math.Abs(Tiles[i].Y - Tiles[j].Y) + 1; // inclusive
                long tmpArea = width * height;
                if (tmpArea > area)
                {
                    area = tmpArea;
                    a = Tiles[i];
                    b = Tiles[j];
                }
            }
        }
    }

    public void GetLargestArea2(out MapPoint a, out MapPoint b, out long area)
    {
        area = 0;
        a = new MapPoint();
        b = new MapPoint();

        GetLines(Tiles, out var vertical, out var horizontal);

        for (var i = 0; i < Tiles.Count; i++)
        {
            for (var j = 0; j < Tiles.Count; j++)
            {
                if (Tiles[i] == Tiles[j])
                    continue;

                var point1 = Tiles[i];
                var point2 = Tiles[j];


                var top = point1.Y <= point2.Y ? point1 : point2;
                var bottom = point1.Y > point2.Y ? point1 : point2;
                var left = top.X <= bottom.X ? top : bottom;
                var right = top.X > bottom.X ? top : bottom;


                var linesHorizontal = horizontal.Where(p =>
                (
                    (p.Start.X >= left.X && p.Start.X <= right.X) ||
                    (p.End.X >= left.X && p.End.X <= right.X) ||
                    (p.Start.X >= left.X && p.End.X <= right.X) ||
                    (p.Start.X < left.X && p.End.X > right.X)
                )).ToList();

                var linesVertical = vertical.Where(p =>
                (
                    (p.Start.Y > top.Y && p.Start.Y < bottom.Y) ||
                    (p.End.Y > top.Y && p.End.Y < bottom.Y) ||
                    (p.Start.Y > top.Y && p.End.Y < bottom.Y) ||
                    (p.Start.Y < top.Y && p.End.Y > bottom.Y)
                )).ToList();


                var linesAbove = linesHorizontal.Where(p => p.Start.Y <= top.Y).ToList();
                var linesBelow = linesHorizontal.Where(p => p.Start.Y >= bottom.Y).ToList();
                var badLines = linesVertical.Where(p => p.Start.X > left.X && p.Start.X < right.X).ToList();
                var linesPossibleIntersect = horizontal.Where(p => p.Start.Y > top.Y && p.Start.Y < bottom.Y
                                                                                     && ((p.Start.X < left.X && p.End.X > left.X) || (p.Start.X < right.X && p.End.X > right.X) ||
                                                                                         (p.Start.X < left.X && p.End.X > right.X))).ToList();

                if (linesAbove.Count == 0 || linesBelow.Count == 0 || linesPossibleIntersect.Count > 0 || badLines.Count > 0)
                    continue;

                var width = (long) Math.Abs(Tiles[i].X - Tiles[j].X) + 1; // inclusive
                var height = (long) Math.Abs(Tiles[i].Y - Tiles[j].Y) + 1; // inclusive

                long tmpArea = width * height;
                if (tmpArea > area)
                {
                    area = tmpArea;
                    a = Tiles[i];
                    b = Tiles[j];
                }
            }
        }
    }


    private void GetLines(List<MapPoint> polygonPoints, out List<Line> verticalLines, out List<Line> horizontalLines)
    {
        verticalLines = [];
        horizontalLines = [];
        var first = polygonPoints.OrderBy(p => p.Y).ThenBy(p => p.X).First();
        var previous = first;
        var current = first;
        var direction = Direction.Right;

        var directionPriority = new Direction[3];

        var getNext = (MapPoint?, Direction) (MapPoint currentPoint, Direction currentDirection) =>
        {
            switch (currentDirection)
            {
                case Direction.Right:
                    directionPriority[0] = Direction.Up;
                    directionPriority[1] = Direction.Right;
                    directionPriority[2] = Direction.Down;
                    break;
                case Direction.Up:
                    directionPriority[0] = Direction.Left;
                    directionPriority[1] = Direction.Up;
                    directionPriority[2] = Direction.Right;
                    break;
                case Direction.Down:
                    directionPriority[0] = Direction.Right;
                    directionPriority[1] = Direction.Down;
                    directionPriority[2] = Direction.Left;
                    break;
                case Direction.Left:
                    directionPriority[0] = Direction.Down;
                    directionPriority[1] = Direction.Left;
                    directionPriority[2] = Direction.Up;
                    break;
            }

            for (var n = 0; n < 3; ++n)
            {
                var points = polygonPoints.Where(p => p.IsNot(currentPoint) &&
                                                      (
                                                          (directionPriority[n] == Direction.Right && p.Y == currentPoint.Y && p.X > currentPoint.X) ||
                                                          (directionPriority[n] == Direction.Left && p.Y == currentPoint.Y && p.X < currentPoint.X) ||
                                                          (directionPriority[n] == Direction.Up && p.X == currentPoint.X && p.Y < currentPoint.Y) ||
                                                          (directionPriority[n] == Direction.Down && p.X == currentPoint.X && p.Y > currentPoint.Y)
                                                      )).ToList();
                if (points.Count == 0)
                    continue;
                var point = points.OrderBy(p => p.GetDistance(currentPoint)).First();
                return (point, directionPriority[n]);
            }

            return (null, currentDirection);
        };

        while (true)
        {
            var nextPointAndDirection = getNext.Invoke(current, direction);

            if (nextPointAndDirection.Item1 == null)
                throw new Exception();

            if (nextPointAndDirection.Item1.Value.Equals(first))
            {
                verticalLines.Add(new Line(current, nextPointAndDirection.Item1.Value));
                break;
            }

            if (nextPointAndDirection.Item2 == Direction.Up)
                verticalLines.Add(new Line(current, nextPointAndDirection.Item1.Value));
            if (nextPointAndDirection.Item2 == Direction.Down)
                verticalLines.Add(new Line(nextPointAndDirection.Item1.Value, current));
            if (nextPointAndDirection.Item2 == Direction.Left)
                horizontalLines.Add(new Line(nextPointAndDirection.Item1.Value, current));
            if (nextPointAndDirection.Item2 == Direction.Right)
                horizontalLines.Add(new Line(current, nextPointAndDirection.Item1.Value));

            current = nextPointAndDirection.Item1.Value;
            direction = nextPointAndDirection.Item2;
        }
    }
}