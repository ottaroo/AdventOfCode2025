namespace Day4;



public class Forklift
{
    public const char PaperRoll = '@';
    public const char EmptySpace = '.';
    
    public Point[] FindAvailableRollsOfPaper(char[][] input, int maxNumberOfAdjacentPaperRolls = 3)
    {
        
        var points = new List<Point>();
        for (var y = 0; y < input.Length; y++)
        {
            for (var x = 0; x < input[y].Length; x++)
            {
                if (input[y][x] != PaperRoll)
                    continue;
                var current =  new Point(x, y);
                var adjacent = 0;
                foreach (var dir in Coordinates.Directions)
                {
                    var mapPoint = Coordinates.GetMapPoint(current, dir);
                    if (mapPoint.X < 0 || mapPoint.X > input[y].Length - 1 || mapPoint.Y < 0 || mapPoint.Y > input[y].Length - 1)
                        continue;
                    if (input[mapPoint.Y][mapPoint.X] == PaperRoll)
                        adjacent++;
                }
                if (adjacent <= maxNumberOfAdjacentPaperRolls)
                    points.Add(current);
            }

        }
        return points.ToArray();
    }

    public void MoveRollsOfPaper(ref char[][] input, Point[] points)
    {
        foreach(var point in points)
            input[point.Y][point.X] = EmptySpace;
    }
}