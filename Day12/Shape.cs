namespace Day12;

public class Shape
{
    private int _copy = 0;
    private bool _isCopy = false;

    public Shape(string id, string shapeId, char[][] shape)
    {
        Id = id;
        ShapeId = shapeId;
        Data = new bool[shape.Length, shape[0].Length];
        for (var y = 0; y < shape[0].Length; y++)
        for (var x = 0; x < shape[0].Length; x++)
            Data[y, x] = shape[y][x] == '#';
    }

    public Shape(string id, string shapeId, bool[,] data)
    {
        Id = id;
        ShapeId = shapeId;
        Data = data;
    }

    public string Id { get; }
    public string ShapeId { get; }

    public bool[,] Data { get; }

    public void ClearCopyCounter()
    {
        _copy = 0;
    }

    public Shape Copy()
    {
        var shape = new Shape($"{Id}_{_copy}", ShapeId, Data)
        {
            _isCopy = true
        };
        _copy++;
        return shape;
    }

    public override string ToString()
    {
        return $"{Id}";
    }

    // private readonly Vector2[] _originalShape;
    // private Vector2[] _testShape;
    // private readonly char[][] _originalShapeChars;
    // private char[][] _testChars;
    // public Shape(int id, char[][] shape)
    // {
    //     Id = id;
    //     _originalShapeChars = shape;
    //     _testChars = new char[3][]; // we know the shapes are maximum 3x3
    //     // initialize array
    //     for (var y = 0; y < _testChars.Length; y++)
    //     {
    //         _testChars[y] = new char[3];
    //         for (var x = 0; x < _testChars[y].Length; x++)
    //             _testChars[y][x] = '.';
    //     }
    //     
    //     
    //
    //     _originalShape = MapToVector(ref shape);
    //     Vertices = MapToVector(ref shape);
    //     _testShape = MapToVector(ref _testChars);
    // }
    //
    // private void ClearMap(ref char[][] map)
    // {
    //     foreach(var y in map)
    //         for (var x = 0; x < y.Length; x++)
    //             y[x] = '.';
    // }
    // private Vector2[] MapToVector(ref char[][] shape)
    // {
    //     var vertices = new List<Vector2>();
    //     for (var y = 0; y < shape.Length; y++)
    //         for (var x = 0; x < shape[y].Length; x++)
    //             if (shape[y][x] == '#')
    //                 vertices.Add(new Vector2(x, y));
    //     
    //     return vertices.ToArray();
    // }
    //
    // public Shape Copy() =>  new Shape(Id, _originalShapeChars);
    //
    // public int Id { get; }
    //
    //
    // public Vector2[] Vertices { get; private set; }
    //
    // public char[][] VectorToMap(ref Vector2[] vector)
    // {
    //     var map = new char[3][];
    //     for (var y = 0; y < 3; y++)
    //     {
    //         map[y] = new char[3];
    //         for (var x = 0; x < 3; x++)
    //         {
    //             map[y][x] = '.';
    //         }
    //     }
    //
    //     foreach (var v in vector)
    //         map[(int) v.Y][(int) v.X] = '#';
    //
    //     return map;
    // }
    //
    // // All shapes are in a 3x3 grid - so I made an assumption - rotating around center point
    //
    // public void RotateLeft()
    // {
    //     Vertices = Vertices.Select(v => new Vector2(2 - v.Y, v.X)).ToArray();
    // }
    //
    // public void RotateRight()
    // {
    //     Vertices = Vertices.Select(v => new Vector2(v.Y, 2-v.X)).ToArray();
    // }
    //
    // public void Flip()
    // {
    //     Vertices = Vertices.Select(v => new Vector2(2-v.X, 2-v.Y)).ToArray();
    // }
    //
    // public void Reset()
    // {
    //     Vertices = _originalShape.ToArray();
    //     _testShape = [];
    //     ClearMap(ref _testChars);
    // }
    //
    // public void RotateLeftTestShape()
    // {
    //     _testShape = _testShape.Select(v => new Vector2(2 - v.Y, v.X)).ToArray();
    // }
    //
    // public void FlipTestShape()
    // {
    //     _testShape = _testShape.Select(v => new Vector2(2-v.X, 2-v.Y)).ToArray();
    // }
    //
    //
    // private void GetShapeFromMap(ref char[][] map, int left, int top)
    // {
    //     const int width = 3;
    //     const int height = 3;
    //     
    //     if (top >= map.Length)
    //         throw new ArgumentOutOfRangeException(nameof(top));
    //     if (left < 0)
    //         throw new ArgumentOutOfRangeException(nameof(left));
    //     if (left + width >= map[0].Length)
    //         throw new ArgumentOutOfRangeException(nameof(left));
    //     if (top + height >= map.Length)
    //         throw new ArgumentOutOfRangeException(nameof(top));
    //     
    //     
    //     for(var y = 0; y < _testChars.Length; y++)
    //     for (var x = 0; x < _testChars[y].Length; x++)
    //         _testChars[y][x] = map[y+top][x+left];
    //     
    //     _testShape = MapToVector(ref _testChars);
    // }
    //
    // public bool ContainsMyShape(int left, int top, ref char[][] map)
    // {
    //     GetShapeFromMap(ref map, left, top);
    //
    //     for (var n = 0; n < 8; n++)
    //     {
    //         if (_originalShape.IsEqual(_testShape))
    //             return true;
    //         if (n == 4)
    //             FlipTestShape();
    //         else
    //             RotateLeftTestShape(); // Try next shape
    //     }
    //     return false;
    // }
    //
    // public void PrintToMap(int left, int top, ref char[][] map)
    // {
    //     for(var y = 0;  y < _originalShapeChars.Length; y++)
    //         for (var x = 0; x <  _originalShapeChars[y].Length; x++)
    //             map[y+top][x+left] = '.';
    //     
    //     foreach (var v in Vertices)
    //     {
    //         map[top + (int) v.Y][left + (int) v.X] = '#';
    //     }
    // }
    //
    // public void PrintToScreen(ref char[][] map)
    // {
    //     for(var y = 0; y <  map.Length; y++)
    //         for(var x =  0; x <  map[y].Length; x++)
    //             Console.WriteLine(map[y][x]);
    // }
}

// public static class ShapeExtensions
// {
//     extension(Vector2[] vectorArray)
//     {
//         public bool IsEqual(Vector2[] vector)
//         {
//             if (vector.Length != vectorArray.Length)
//                 return false;
//             for (var n = 0; n < vectorArray.Length; n++)
//             {
//                 if(Math.Abs(vectorArray[n].X - vector[n].X) > float.Epsilon)
//                     return false;
//                 if(Math.Abs(vectorArray[n].Y - vector[n].Y) > float.Epsilon)
//                     return false;
//             }
//             return true;
//         }
//     }
// }