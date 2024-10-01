using System;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding
{
    const int MOVE_STRAIGHT_COST = 10;
    const int MOVE_DIAGONAL_COST = 14;

    Vector3Int bottomLeftLocation;
    int offset = 0;

    List<PathNod> openList;
    List<PathNod> closedList;
    List<PathNod> listAllNodeGrid;
    
    public PathFinding(Vector3Int bottomLeftLocation)
    {
        this.bottomLeftLocation = bottomLeftLocation;
        offset = Mathf.Abs(bottomLeftLocation.x);

        GenerateNodeData();
    }

    void GenerateNodeData()
    {
        listAllNodeGrid = new();
        //  Generate data path node for all grid
        for (int x = bottomLeftLocation.x; x < (bottomLeftLocation.x * -1f); x++)
        {
            for (int y = bottomLeftLocation.z; y < (bottomLeftLocation.z * -1f); y++)
            {
                PathNod pathNod = new(x, y);
                pathNod.gCost = int.MaxValue;
                pathNod.CalculateFcost();
                pathNod.cameFromNode = null;

                listAllNodeGrid.Add(pathNod);
            }
        }
    }

    public List<PathNod> FindPath(int startX = 0, int startY = 0, int endX = 1, int endY = 1)
    {
        Debug.Log(" __________ Start finding . . .");
        //Debug.Log(startX + " , " + startY + " , " + endX + " , " + endY);

        ResetNodes();

        PathNod startNode = GetNode(startX, startY);
        PathNod endNode = GetNode(endX, endY);

        if (startNode == null || endNode == null || !startNode.isWalkable || !endNode.isWalkable)
        {
            Debug.Log("Invalid start or end node");
            return null;
        }

        openList = new() { startNode };
        closedList = new();

        startNode.gCost = 0;
        startNode.hCost = CalculateDistance(startNode, endNode);
        startNode.CalculateFcost();

        int count3 = 0;
        while (openList.Count > 0)
        {
            count3 += 1;
            //Debug.Log("         ====== STEP  " + count3);
            PathNod currentNode = GetTheLowestFCostNode(openList);

            //Debug.Log(currentNode.ToString() + " ==? " + endNode.ToString());

            if (currentNode == endNode)
            {
                //  Reach the final node
                Debug.Log("Reach the final node");
                return CalculatePath(currentNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNod neiborNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neiborNode))
                {
                    continue;
                }

                if (!neiborNode.isWalkable)
                {
                    //Debug.Log("neiborNode not isWalkable: " + neiborNode.ToString());
                    closedList.Add(neiborNode);
                    continue;
                }

                int tentativeGCost = currentNode.gCost + CalculateDistance(currentNode, neiborNode);

                if (tentativeGCost < neiborNode.gCost)
                {
                    neiborNode.cameFromNode = currentNode;
                    //Debug.Log("cameFromNode: " + neiborNode.cameFromNode.ToString());
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

    void ResetNodes()
    {
        foreach (var node in listAllNodeGrid)
        {
            node.gCost = int.MaxValue;
            node.CalculateFcost();
            node.cameFromNode = null;
        }
    }

    private List<PathNod> CalculatePath(PathNod endNode)
    {
        List<PathNod> path = new(){ endNode };
        PathNod currentNode = endNode;

        //Debug.Log(" ========== CalculatePath ");
        //Debug.Log(currentNode.ToStringFull());
        //int count4 = 0;

        while (currentNode.cameFromNode != null)
        {
            //Debug.Log("count 4 = " + count4);
            //Debug.Log("currentNode.cameFromNode = " + currentNode.cameFromNode);
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }

        path.Reverse();
        return path;
    }


    private List<PathNod> GetNeighbourList(PathNod currentNode)
    {
        List<PathNod> neighbourList = new();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                int checkX = currentNode.x + x;
                int checkY = currentNode.y + y;

                PathNod neighbourNode = GetNode(checkX, checkY);
                if (neighbourNode != null && neighbourNode.isWalkable)
                {
                    neighbourList.Add(neighbourNode);
                }
            }
        }

        return neighbourList;
    }

    internal PathNod GetNode(int x, int y)
    {
        // Get node from grid
        PathNod node = listAllNodeGrid.Find(node => node.x == x && node.y == y);
        return node;
    }
    internal PathNod GetNodeWithOffset(int x, int y)
    {
        return GetNode(x + offset, y + offset);
    }

    private PathNod GetTheLowestFCostNode(List<PathNod> list)
    {
        PathNod lowestCostNode = list[0];
        for (int i = 1; i < list.Count; i++)
        {
            if (list[i].fCost < lowestCostNode.fCost)
            {
                lowestCostNode = list[i];
            }
        }

        return lowestCostNode;
    }

    int CalculateDistance(PathNod a, PathNod b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);

        return MOVE_DIAGONAL_COST*Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }
}