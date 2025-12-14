using System;
using System.Collections.Generic;
using System.Linq;

namespace Day7;

public class Node() : IEquatable<Node>
{
    public Node? ParentNode { get; set; }
    
    public Point Point { get; private init; }

   
    private long? _cachedPathCount = null;

    private void ResetCachedPathCount()
    {
        var current = this;
        while (current != null)
        {
            _cachedPathCount = null;
            current = current.ParentNode;
        }

    }
    
    public long PathCount
    {
        get
        {
            if (_cachedPathCount.HasValue)
                return _cachedPathCount.Value;

            if (Left == null && Right == null)
            {
                _cachedPathCount = 1L;
            }
            else
            {
                var total = 0L;
                if (Left != null) total += Left.PathCount;
                if (Right != null) total += Right.PathCount;
                _cachedPathCount = total;
            }

            return _cachedPathCount.Value;
        }
    }

    public static Node Create(int x, int y, Node? parentNode)
    {
        var node = new Node()
        {
            Point = new Point(x, y)
        };
        node.ParentNode = parentNode;

        return node;
    }

    public Node? Left
    {
        get;
        set
        {
            field = value;
            ResetCachedPathCount();
        }
    }

    public Node? Right
    {
        get;
        set
        {
            field = value;
            ResetCachedPathCount();
        }
    }


    public bool Equals(Node? other)
    {
        if (other is null) return false;
        return Point.Equals(other.Point);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj.GetType() != GetType()) return false;
        return Equals((Node) obj);
    }

    public override int GetHashCode()
    {
        return Point.GetHashCode();
    }
    
}