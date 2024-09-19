using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNod
{
    public int x;
    public int y;
    public bool isWalkable;

    public int gCost;
    public int hCost;
    public int fCost;

    public PathNod cameFromNode;

    public PathNod(int x, int y, bool isWalkTable = true)
    {
        this.x = x;
        this.y = y;
        this.isWalkable = isWalkTable;
    }

    internal void CalculateFcost()
    {
        fCost = gCost + hCost;
    }


    //  ================== OVERRIDE ==================
    public override string ToString()
    {
        return "x: " + x + ", y: " + y;
    }
    public string ToStringFull()
    {
        return "x: " + x + ", y: " + y + ", cameFromNode: x" + cameFromNode.x + ", y: " + cameFromNode.y;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        PathNod other = (PathNod)obj;
        return x == other.x && y == other.y;
    }

    public override int GetHashCode()
    {
        return (x, y).GetHashCode();
    }

    public static bool operator ==(PathNod a, PathNod b)
    {
        if (ReferenceEquals(a, b)) return true;
        if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;
        return a.x == b.x && a.y == b.y;
    }

    public static bool operator !=(PathNod a, PathNod b)
    {
        return !(a == b);
    }
}
