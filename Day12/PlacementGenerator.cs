namespace Day12;

public static class PlacementGenerator
{
    public static IEnumerable<(int pieceIndex, bool[,] orientation, int offsetX, int offsetY)>
        GeneratePlacements(List<Shape> pieces, int boardW, int boardH)
    {
        for (var p = 0; p < pieces.Count; p++)
        {
            var shape = pieces[p];

            var orientations = ShapeTransform.GenerateOrientations(shape.Data);

            foreach (var ori in orientations)
            {
                var h = ori.GetLength(0);
                var w = ori.GetLength(1);

                for (var oy = 0; oy <= boardH - h; oy++)
                {
                    for (var ox = 0; ox <= boardW - w; ox++)
                    {
                        yield return (p, ori, ox, oy);
                    }
                }
            }
        }
    }
}