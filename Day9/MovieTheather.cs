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
                if (Tiles[i] ==  Tiles[j])
                    continue;
                
                var width = (long)Math.Abs(Tiles[i].X - Tiles[j].X) + 1; // inclusive
                var height = (long)Math.Abs(Tiles[i].Y - Tiles[j].Y) + 1; // inclusive
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

    public bool IsPointInPolygon(MapPoint testPoint)
    {
        bool inside = false;
        int count = Tiles.Count;

        for (int i = 0, j = count - 1; i < count; j = i++)
        {
            var pi = Tiles[i];
            var pj = Tiles[j];

            bool intersect = ((pi.Y > testPoint.Y) != (pj.Y > testPoint.Y)) &&
                             (testPoint.X < (pj.X - pi.X) * (testPoint.Y - pi.Y) / (pj.Y - pi.Y) + pi.X);

            if (intersect)
                inside = !inside;
        }

        return inside;
    }
    
    public void GetLargestArea2(out MapPoint a, out MapPoint b, out long area)
    {
        area = 0;
        a = new MapPoint();
        b = new MapPoint();

        var minX = Tiles.Min(t => t.X);
        var minY = Tiles.Min(t => t.Y);
        var maxX = Tiles.Max(t => t.X);
        var maxY = Tiles.Max(t => t.Y);
        
        
        for (var i = 0; i < Tiles.Count; i++)
        {
            for (var j = 0; j < Tiles.Count; j++)
            {
                if (Tiles[i] ==  Tiles[j])
                    continue;
                
                // Must have a 3rd point
                var point1 = Tiles[i];
                var point2 = Tiles[j];
                
                var point3 = new MapPoint(point1.X, point2.Y);
                var point4 = new MapPoint(point2.X, point1.Y);

                MapPoint? pointToVerify = null;
                if (Tiles.Contains(point3))
                    pointToVerify = point4;
                if (pointToVerify == null && Tiles.Contains(point4))
                    pointToVerify = point3;
                
                if (pointToVerify != null)
                {
                    // the 4th point must be within the boundary points
                    if (IsPointInPolygon(pointToVerify.Value))
                    {

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
        }
        
    }
    
}