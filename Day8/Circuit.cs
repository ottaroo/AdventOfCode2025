using System.Collections.Generic;

namespace Day8;

public class Circuit
{
    public Circuit(params JunctionBox[] boxes)
    {
        foreach (var box in boxes)
            Boxes.Add(box);
    }

    public HashSet<JunctionBox> Boxes { get; } = [];
}