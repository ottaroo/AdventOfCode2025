using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Threading;

namespace Day7;

public class Teleporter
{
    public const char Start = 'S';
    public const char EmptySpace = '.';
    public const char Splitter = '^';
    public const char Beam = '|';
    private string _path;

    private char[][] _schema;

    public Point[] TachyonStart = [];

    private Teleporter()
    {
    }

    private Queue<Point> Beams { get; set; } = new();


    public int CalculateBeamSplits()
    {
        Reset();
        Beams = new Queue<Point>(TachyonStart);
        var splits = 0;
        while (Beams.TryDequeue(out var beamStart))
        {
            splits += MoveTachyon(beamStart);
        }

        return splits;
    }

    public static Teleporter Create(string path)
    {
        var teleport = new Teleporter()
        {
            _path = path
        };
        teleport.Reset();

        return teleport;
    }


    public long FindTimeLines()
    {
        _ = CalculateBeamSplits();

        var firstSplitterX = _schema[0].IndexOf(Start);
        var firstSplitterY = 0;
        for (var y = 0; y < _schema.Length; y++)
        {
            if (_schema[y][firstSplitterX] == Splitter)
            {
                firstSplitterY = y;
                break;
            }
        }

        var root = Node.Create(firstSplitterX, firstSplitterY - 1, null);
        var left = Node.Create(firstSplitterX - 1, firstSplitterY, root);
        var right = Node.Create(firstSplitterX + 1, firstSplitterY, root);
        root.Left = left;
        root.Right = right;
        var xpoints = new Dictionary<int, List<Node>>();
        xpoints.Add(firstSplitterX - 1, [left]);
        xpoints.Add(firstSplitterX + 1, [right]);

        for (var y = firstSplitterY; y < _schema.Length; y++)
        {
            for (var x = 0; x < _schema[y].Length; x++)
            {
                if (_schema[y][x] == Splitter)
                {
                    // Beam hitting splitter
                    if (!xpoints.TryGetValue(x, out var parentBeams))
                        continue; // splitter not in use
                    xpoints.Remove(x);

                    var parentBeam = Node.Create(x, y - 1, parentBeams.Last());
                    foreach (var node in parentBeams)
                    {
                        var parent = node.ParentNode;
                        if (parent == null)
                            continue; // should not happen
                        // replace leafs
                        if (ReferenceEquals(parent.Left, node))
                            parent.Left = parentBeam;
                        if (ReferenceEquals(parent.Right, node))
                            parent.Right = parentBeam;
                    }

                    var l = Node.Create(x - 1, y, parentBeam);
                    var r = Node.Create(x + 1, y, parentBeam);
                    parentBeam.Left = l;
                    parentBeam.Right = r;

                    if (xpoints.TryGetValue(x - 1, out var nodeAboveLeft))
                        nodeAboveLeft.Add(l);
                    else
                        xpoints.Add(x - 1, [l]);

                    if (xpoints.TryGetValue(x + 1, out var nodeAboveRight))
                        nodeAboveRight.Add(r);
                    else
                        xpoints.Add(x + 1, [r]);
                }
            }
        }


        return root.PathCount;
    }

    private bool IsBeam(Point point) => IsValidPoint(point) && (_schema[point.Y][point.X] == Beam || IsStart(point));
    private bool IsEmptySpace(Point point) => IsValidPoint(point) && _schema[point.Y][point.X] == EmptySpace;

    private bool IsSplitter(Point point) => IsValidPoint(point) && _schema[point.Y][point.X] == Splitter;


    private bool IsStart(Point point) => _schema[point.Y][point.X] == Start;

    private bool IsValidPoint(Point point)
    {
        if (point.Y < 0 || point.Y >= _schema.Length)
            return false;
        if (point.X < 0 || point.X >= _schema[point.Y].Length)
            return false;
        return true;
    }

    private int MoveTachyon(Point point)
    {
        var beamCount = 0;

        if (!IsBeam(point))
            return beamCount;

        var current = point;

        while (true)
        {
            current = Coordinates.GetMapPoint(current, Coordinates.Direction.Down);
            if (!IsEmptySpace(current))
                break;


            SetMapChar(current, Beam);
        }

        if (IsSplitter(current))
        {
            var left = Coordinates.GetMapPoint(current, Coordinates.Direction.Left);
            var right = Coordinates.GetMapPoint(current, Coordinates.Direction.Right);
            var above = Coordinates.GetMapPoint(current, Coordinates.Direction.Up);
            if (IsEmptySpace(left))
            {
                SetMapChar(left, Beam);
                Beams.Enqueue(left);
            }

            if (IsEmptySpace(right))
            {
                SetMapChar(right, Beam);
                Beams.Enqueue(right);
            }

            if (IsBeam(above))
                beamCount++;
        }

        return beamCount;
    }

    private void Reset()
    {
        _schema = File.ReadAllLines(_path).Select(x => x.ToArray()).ToArray();
        var startPoints = new List<Point>();
        for (var y = 0; y < _schema.Length; y++)
        for (var x = 0; x < _schema[y].Length; x++)
        {
            if (_schema[y][x] == Start)
            {
                startPoints.Add(new Point(x, y));
            }
        }

        TachyonStart = startPoints.ToArray();
    }

    private void SetMapChar(Point point, char c) => _schema[point.Y][point.X] = c;
}