using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNod
{
    public int x;
    public int y;
    public bool isWalkTable;

    public int gCost;
    public int hCost;
    public int fCost;

    public PathNod cameFromNode;

    public PathNod(int x, int y, bool isWalkTable = true)
    {
        this.x = x;
        this.y = y;
        this.isWalkTable = isWalkTable;
    }

    internal void CalculateFcost()
    {
        fCost = gCost + hCost;
    }
}
