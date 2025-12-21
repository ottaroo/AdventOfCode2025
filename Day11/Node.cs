using System;
using System.Collections.Generic;
using System.Linq;

namespace Day11;

public class Node() : IEquatable<Node>
{
    public Node? ParentNode { get; set; }
    
    public string Name { get; private init; }

   
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

            if (!_children.Any())
            {
                _cachedPathCount = 1L;
            }
            else
            {
                var total = 0L;
                foreach(var child in _children)
                    total += child.PathCount;
                _cachedPathCount = total;
            }

            return _cachedPathCount.Value;
        }
    }

    public static Node Create(string name, Node? parentNode = null)
    {
        
        var node = new Node
        {
            Name = name,
            ParentNode = parentNode
        };

        return node;
    }

    private readonly List<Node> _children = [];
    
    public void AddChild(Node child)
    {
        if (_children.Contains(child))
            return;
        _children.Add(child);
        if (child.ParentNode != null)
        {
            int p = 0;
        }

        child.ParentNode = this;
        // ResetCachedPathCount();
    }
    
    public IReadOnlyList<Node> Children => _children;


    public bool Equals(Node? other)
    {
        if (other is null) return false;
        return Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj.GetType() != GetType()) return false;
        return Equals((Node) obj);
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
    
}