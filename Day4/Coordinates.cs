namespace Day4;

public static class Coordinates
{
    public static Point Left =>  new(-1, 0);
    public static Point Right => new(1, 0);
    public static Point Up => new(0, -1);
    public static Point Down => new(0, 1);
    public static Point LeftUp => new(-1, -1);
    public static Point LeftDown => new(-1, 1);
    public static Point RightUp => new(1, -1);
    public static Point RightDown => new(1, 1);
    
    public static Point GetMapPoint(Point source,  Point target) => new Point(source.X - target.X, source.Y - target.Y);

    public static IEnumerable<Point> Directions
    {
        get
        {
            yield return Left;
            yield return LeftUp;
            yield return Up;
            yield return RightUp;
            yield return Right;
            yield return RightDown;
            yield return Down;
            yield return LeftDown;
        }
    }
    
}