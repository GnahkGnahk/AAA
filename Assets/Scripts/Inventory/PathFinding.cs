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
        int count1 = 0, count2 = 0;
        //  Generate data path node for all grid
        for (int x = bottomLeftLocation.x; x < (bottomLeftLocation.x * -1f); x++)
        {
            count1++;
            for (int y = bottomLeftLocation.z; y < (bottomLeftLocation.z * -1f); y++)
            {
                PathNod pathNod = new(x, y);
                pathNod.gCost = int.MaxValue;
                pathNod.CalculateFcost();
                pathNod.cameFromNode = null;

                listAllNodeGrid.Add(pathNod);
                count2++;
            }
        }
    }

    public List<PathNod> FindPath(int startX = 0, int startY = 0, int endX = 1, int endY = 1)
    {
        //Debug.Log(startX + " , " + startY + " , " + endX + " , " + endY);

        PathNod startNode = new(startX, startY);
        PathNod endNode = new(endX, endY);

        openList = new() { startNode };
        closedList = new();


        startNode.gCost = 0;
        startNode.hCost = CalculateDistance(startNode, endNode);
        startNode.CalculateFcost();

        //int count3 = 0;
        while (openList.Count > 0)
        {
            //count3 += 1;
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
        int tempX = bottomLeftLocation.x,  tempY = bottomLeftLocation.z;
        List<PathNod> neighbourList = new();

        //Debug.Log("Get neibor for: " + currentNode.ToString());

        if (currentNode.x - 1 >= tempX)
        {
            //Debug.Log("left");
            // Left
            neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));
            // Left Down
            if (currentNode.y - 1 >= tempY)
            {
                neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y - 1));
                //Debug.Log("Left Down");
            }
            // Left Up
            if (currentNode.y + 1 < tempY * -1)
            {
                neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y + 1));
                //Debug.Log("Left Up");

            }
        }

        if (currentNode.x + 1 < tempX *-1)
        {
            // Right
            //Debug.Log("right");
            neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y));
            // Right Down
            if (currentNode.y - 1 >= tempY)
            {
                neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y - 1));

                //Debug.Log("Right down");
            }
            // Right Up
            if (currentNode.y + 1 < tempY * -1)
            {
                neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
                //Debug.Log("Right up");
            }
        }

        // Down
        if (currentNode.y - 1 >= tempY)
        {
            neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));
            //Debug.Log("Down");
        }
        // Up
        if (currentNode.y + 1 < tempY * -1)
        {
            neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1));
            //Debug.Log("Up");
        }

        //Debug.Log("=====Count neibor: " + neighbourList.Count);
        //foreach (var item in neighbourList)
        //{
        //    Debug.Log(item.ToString());
        //}
        return neighbourList;
    }

    internal PathNod GetNode(int x, int y)
    {
        Debug.Log("listAllNodeGrid: " + listAllNodeGrid.Count);
        Debug.Log("X: " + x + ", " + y);
        // Get node from grid
        PathNod node = listAllNodeGrid.Find(node => node.x == x && node.y == y);
        if (node == null)
        {
            Debug.Log("Null");
        }
        else
        {
            Debug.Log("Node: " + node.ToString());
        }
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
