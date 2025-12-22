using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Day8;

public class JunctionBoxOrganizer
{
    public List<JunctionBox> Boxes { get; } = [];
    public List<Circuit> Circuits { get; set; } = [];
    public Circuit LastConnectedJunctionBoxes { get; set; }

    public void AddJunctionBox(JunctionBox junctionBox)
    {
        Boxes.Add(junctionBox);
    }

    public void AddJunctionBox(int x, int y, int z) => AddJunctionBox(new JunctionBox(new Vector3(x, y, z)));


    public void Connect(int numberOfShortestPaths = 10)
    {
        Circuits.Clear();
        // Shortest distance
        var allTheShortestPath = new List<ShortestPath>();

        for (var i = 0; i < Boxes.Count - 1; i++)
        {
            for (var j = i + 1; j < Boxes.Count; j++)
            {
                allTheShortestPath.Add(new ShortestPath(Boxes[i], Boxes[j], Vector3.DistanceSquared(Boxes[i].Location, Boxes[j].Location)));
            }
        }

        foreach (var path in allTheShortestPath.OrderBy(x => x.Distance).Take(numberOfShortestPaths > 0 ? numberOfShortestPaths : allTheShortestPath.Count))
        {
            var existingA = Circuits.FirstOrDefault(x => x.Boxes.Contains(path.A));
            var existingB = Circuits.FirstOrDefault(x => x.Boxes.Contains(path.B));
            if (existingA != null && existingB != null)
            {
                if (ReferenceEquals(existingA, existingB))
                    continue; // both exist, skip

                // merge
                foreach (var box in existingB.Boxes)
                    existingA.Boxes.Add(box);
                Circuits.Remove(existingB);
                if (existingA.Boxes.Count == Boxes.Count)
                {
                    LastConnectedJunctionBoxes = new Circuit(path.A, path.B);
                    break;
                }

                continue;
            }

            if (existingA != null)
            {
                existingA.Boxes.Add(path.B);
                if (existingA.Boxes.Count == Boxes.Count)
                {
                    LastConnectedJunctionBoxes = new Circuit(path.A, path.B);
                    break; // all connected
                }

                continue;
            }

            if (existingB != null)
            {
                existingB.Boxes.Add(path.A);
                if (existingB.Boxes.Count == Boxes.Count)
                {
                    LastConnectedJunctionBoxes = new Circuit(path.A, path.B);
                    break; // all connected
                }

                continue;
            }

            Circuits.Add(new Circuit(path.A, path.B));
        }
    }
}