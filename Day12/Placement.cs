using Day12;

public sealed class Placement
{
    public Placement(Shape piece, int offsetX, int offsetY, int[] coveredCells)
    {
        Piece = piece;
        OffsetX = offsetX;
        OffsetY = offsetY;
        CoveredCells = coveredCells;
    }

    public Shape Piece { get; }
    public int OffsetX { get; }
    public int OffsetY { get; }
    public int[] CoveredCells { get; }

    public override string ToString()
        => $"{Piece} at ({OffsetX},{OffsetY})";
}