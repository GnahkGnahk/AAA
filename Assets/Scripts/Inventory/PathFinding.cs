using System;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding
{
    const int MOVE_STRAIGHT_COST = 10;
    const int MOVE_DIAGONAL_COST = 14;

    [SerializeField] GridManager gridManager;
    Vector3Int bottomLeftLocation;

    List<PathNod> openList;
    List<PathNod> closedList;
    
    public PathFinding()
    {
        bottomLeftLocation = gridManager.bottomLeftLocation;
    }


    List<PathNod> FindPath(int startX, int startY, int endX, int endY)
    {
        Debug.Log(startX + " , " + startY + " , " + endX + " , " + endY);
        Debug.Log("bottomLeftLocation: " + bottomLeftLocation);

        PathNod startNode = new(startX, startY);
        PathNod endNode = new(endX, endY);

        openList = new() { startNode };
        closedList = new();

        //  Generate data path node for all grid
        for (int x = bottomLeftLocation.x; x < (bottomLeftLocation.x * -1f); x++)
        {
            for (int y = bottomLeftLocation.z; y < (bottomLeftLocation.z * -1f); y++)
            {
                PathNod pathNod = new(x, y);
                pathNod.gCost = int.MaxValue;
                pathNod.CalculateFcost();
                pathNod.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistance(startNode, endNode);
        startNode.CalculateFcost();

        while (openList.Count > 0)
        {
            PathNod currentNode = GetTheLowestFCostNode(openList);
            if (currentNode == endNode)
            {
                //  Reach the final node
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNod neiborNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neiborNode))
                {
                    continue;
                }

                if (!neiborNode.isWalkTable)
                {
                    closedList.Add(neiborNode);
                    continue;
                }

                int tentativeGCost = currentNode.gCost + CalculateDistance(currentNode, neiborNode);

                if (tentativeGCost < neiborNode.gCost)
                {
                    neiborNode.cameFromNode = currentNode;
                    neiborNode.gCost = tentativeGCost;
                    neiborNode.hCost = CalculateDistance(neiborNode, endNode);
                    neiborNode.CalculateFcost();

                    if (!openList.Contains(neiborNode))
                    {
                        openList.Add(neiborNode);
                    }
                }
            }
        }

        //  Out of node in openList
        Debug.Log("Out of node in openList");
        return null;
    }

    private List<PathNod> CalculatePath(PathNod endNode)
    {
        List<PathNod> path = new(){ endNode };
        PathNod currentNode = endNode;

        while (currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }

        path.Reverse();
        return path;
    }


    private List<PathNod> GetNeighbourList(PathNod currentNode)
    {
        List<PathNod> neighbourList = new List<PathNod>();

        if (currentNode.x - 1 >= 0)
        {
            // Left
            neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));
            // Left Down
            if (currentNode.y - 1 >= 0)
                neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y - 1));
            // Left Up
            if (currentNode.y + 1 < bottomLeftLocation.z)
                neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y + 1));
        }

        if (currentNode.x + 1 < bottomLeftLocation.x)
        {
            // Right
            neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y));
            // Right Down
            if (currentNode.y - 1 >= 0)
                neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y - 1));
            // Right Up
            if (currentNode.y + 1 < bottomLeftLocation.z)
                neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
        }

        // Down
        if (currentNode.y - 1 >= 0)
            neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));
        // Up
        if (currentNode.y + 1 < bottomLeftLocation.z)
            neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1));

        return neighbourList;
    }

    private PathNod GetNode(int v, int y)
    {
        return null;
    }

    private PathNod GetTheLowestFCostNode(List<PathNod> list)
    {
        PathNod resultNode = list[0];
        for (int i = 1; i < list.Count; i++)
        {
            if (resultNode.fCost > list[i].fCost)
            {
                resultNode = list[i];
            }
        }

        return resultNode;
    }

    int CalculateDistance(PathNod a, PathNod b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);

        return MOVE_DIAGONAL_COST*Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }
}
