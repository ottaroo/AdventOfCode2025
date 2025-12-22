using System.Collections;
using Day12;


public sealed class PackageAllocationSolver
{
    private readonly int _boardH;
    private readonly int _boardW;
    private readonly List<Placement> _currentSolution;

    private readonly BitArray _occupied; // board cells used
    private readonly List<Shape> _pieces;
    private readonly Dictionary<string, List<bool[,]>> _shapeOrientations;

    public PackageAllocationSolver(int boardW, int boardH, List<Shape> pieces)
    {
        _boardW = boardW;
        _boardH = boardH;
        _pieces = pieces;

        _occupied = new BitArray(boardW * boardH);
        _currentSolution = new List<Placement>();

        // Cache orientations per shape
        _shapeOrientations = new Dictionary<string, List<bool[,]>>();
        foreach (var piece in pieces.Where(p => p.Id.EndsWith("_0")))
            _shapeOrientations[piece.ShapeId] = ShapeTransform.GenerateOrientations(piece.Data);
    }

    private bool CanPlace(Placement placement)
    {
        foreach (var idx in placement.CoveredCells)
        {
            if (_occupied[idx]) return false;
        }

        return true;
    }

    public IReadOnlyList<Placement>? FindOneSolution()
    {
        var remaining = _pieces.ToList();
        return Search(remaining) ? _currentSolution.AsReadOnly() : null;
    }

    private IEnumerable<Placement> GeneratePlacementsForPiece(Shape piece)
    {
        var orientations = _shapeOrientations[piece.ShapeId];

        foreach (var ori in orientations)
        {
            var h = ori.GetLength(0);
            var w = ori.GetLength(1);

            for (var oy = 0; oy <= _boardH - h; oy++)
            {
                for (var ox = 0; ox <= _boardW - w; ox++)
                {
                    var cells = new List<int>();

                    for (var y = 0; y < h; y++)
                    {
                        for (var x = 0; x < w; x++)
                        {
                            if (!ori[y, x]) continue;
                            var boardX = ox + x;
                            var boardY = oy + y;

                            var idx = boardY * _boardW + boardX;
                            cells.Add(idx);
                        }
                    }

                    if (cells.Count == 0)
                        continue;

                    yield return new Placement(piece, ox, oy, cells.ToArray());
                }
            }
        }
    }

    private void Place(Placement placement)
    {
        foreach (var idx in placement.CoveredCells)
            _occupied[idx] = true;
    }

    private bool Search(List<Shape> remainingPieces)
    {
        if (remainingPieces.Count == 0)
            return true; // all pieces placed

        // Heuristic: choose next piece (could sort by shape complexity)
        var piece = remainingPieces[0];
        remainingPieces.RemoveAt(0);

        foreach (var placement in GeneratePlacementsForPiece(piece))
        {
            if (!CanPlace(placement))
                continue;

            Place(placement);
            _currentSolution.Add(placement);

            if (Search(remainingPieces))
                return true;

            _currentSolution.RemoveAt(_currentSolution.Count - 1);
            Unplace(placement);
        }

        remainingPieces.Insert(0, piece);
        return false;
    }

    private void Unplace(Placement placement)
    {
        foreach (var idx in placement.CoveredCells)
            _occupied[idx] = false;
    }
}