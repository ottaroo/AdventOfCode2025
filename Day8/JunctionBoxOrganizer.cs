
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;

namespace Day8;

using PointInSpace = (int x, int y, int z);

public class JunctionBoxOrganizer
{
    public List<JunctionBox> Boxes { get; } = [];
    public List<List<JunctionBox>> Connections { get; set; } = [];

    public void AddJunctionBox(JunctionBox junctionBox)
    {
        Boxes.Add(junctionBox);
        junctionBox.Name = $"Box{Boxes.Count + 1:D4}";
    }

    public void AddJunctionBox(int x, int y, int z) => AddJunctionBox(new JunctionBox(new PointInSpace(x, y, z)));
    

    public double GetDistance(PointInSpace p1, PointInSpace p2)
    {
        return Math.Sqrt(Math.Pow(p2.x - p1.x, 2) + Math.Pow(p2.y - p1.y, 2) + Math.Pow(p2.z - p1.z, 2));
    }

    public void Connect()
    {
        Connections.Clear();
        
        // Shortest distance
        var distanceList = new HashSet<Distance>();
        var calculated = new HashSet<JunctionBox>();

        for (var i = 0; i < Boxes.Count; i++)
        {
            for (var j = 0; j < Boxes.Count; j++)
            {
                if (Boxes[i].Equals(Boxes[j]))
                    continue;
                
                distanceList.Add(new Distance(Boxes[i], Boxes[j], Boxes[j].Distance(Boxes[i])));
            }
            
        }
        
        var dictionary = new Dictionary<JunctionBox, JunctionBox>();
        JunctionBox? last = null;
        foreach (var info in distanceList.OrderBy(x=>x.BoxA).ThenBy(x => x.Value))
        {
            if (!info.BoxA.Equals(last))
            {
                dictionary.Add(info.BoxA, info.BoxB);
                last = info.BoxA;
            }
        }
        
        
        var circuit = new List<Circuit>();
        var alreadyAdded = new HashSet<JunctionBox>();
        foreach(var key in dictionary.Keys)
        {
            if (alreadyAdded.Contains(key))
                continue;
            
            var value =  dictionary[key];
            var existing = circuit.FirstOrDefault(x => x.Boxes.Contains(value));
            if (existing != null)
            {
                if (existing.Connect(key, value))
                {
                    alreadyAdded.Add(key);
                    continue;
                }
            }
            alreadyAdded.Add(key);
            var newCircuit = new Circuit([key,value]);
            circuit.Add(newCircuit);
        }
        
        
        
        
    }
    
}

public class JunctionBox(PointInSpace location) : IEquatable<JunctionBox>, IComparable<JunctionBox>
{
    public string Name { get; set; }
    public PointInSpace Location { get; } = location;

    public bool Equals(JunctionBox? other)
    {
        if (other is null) return false;
        return Location.Equals(other.Location);
    }

    public int CompareTo(JunctionBox? other)
    {
        if (other is null) return 1;
        return GetHashCode().CompareTo(other.GetHashCode());
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj.GetType() != GetType()) return false;
        return Equals((JunctionBox) obj);
    }

    public override int GetHashCode()
    {
        return Location.GetHashCode();
    }

    public double Distance(JunctionBox other)
    {
        return Math.Sqrt(Math.Pow(other.Location.x - Location.x, 2) + Math.Pow(other.Location.y - Location.y, 2) + Math.Pow(other.Location.z - Location.z, 2));
    }
}

public class Distance(JunctionBox boxA, JunctionBox boxB,  double distance) : IEquatable<Distance>
{
    public double Value { get; } = distance;
    public JunctionBox BoxA { get; }  = boxA;
    public JunctionBox BoxB { get; } = boxB;

    public bool Equals(Distance? other)
    {
        if (other is null) return false;
        return (BoxA.Equals(other.BoxA) && BoxB.Equals(other.BoxB)) || (BoxA.Equals(other.BoxB) && BoxB.Equals(other.BoxA));
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Distance) obj);
    }

    public override int GetHashCode()
    {
        var hashcode1 = BoxA.Location.GetHashCode();
        var hashcode2 =  BoxB.Location.GetHashCode();
        if (hashcode1 < hashcode2)
            return hashcode1 ^ hashcode2;
        return hashcode2 ^ hashcode1;
    }
}
public class Circuit
{
    public Circuit(params JunctionBox[] boxes)
    {
        foreach (var box in boxes)
            Boxes.AddLast(box);
    }
    
    public LinkedList<JunctionBox> Boxes { get; } = [];

    public bool Connect(JunctionBox junctionBox, JunctionBox connectToBox)
    {
        if (!Boxes.Any())
        {
            Boxes.AddFirst(junctionBox);
            return true;
        }        
        if (Boxes.Last.Value.Equals(connectToBox))
        {
            Boxes.AddLast(junctionBox);
            return true;
        }

        if (Boxes.First.Value.Equals(connectToBox))
        {
            Boxes.AddFirst(junctionBox);
            return true;
        }

        return false;
    }
    
    public bool Connect(Distance distance)
    {
        if (!Boxes.Any())
        {
            Boxes.AddFirst(distance.BoxA);
            Boxes.AddLast(distance.BoxB);
            return true;
        }
        
        if (Boxes.Last.Value.Equals(distance.BoxA))
        {
            Boxes.AddLast(distance.BoxB);
            return true;
        }

        if (Boxes.Last.Value.Equals(distance.BoxB))
        {
            Boxes.AddLast(distance.BoxA);
            return true;
        } 

        if (Boxes.First.Value.Equals(distance.BoxA))
        {
            Boxes.AddFirst(distance.BoxB);
            return true;
        }

        if (Boxes.First.Value.Equals(distance.BoxB))
        {
            Boxes.AddFirst(distance.BoxA);
            return true;
        }

        return false;
    }
    
}