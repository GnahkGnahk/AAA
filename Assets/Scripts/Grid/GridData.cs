using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridData
{
    Dictionary<Vector3Int, PlacementData> cellPlacedData = new();

    public bool AddObjectAt(Vector3Int gridPosition, Furniture furniture, PathFinding pathFinding)
    {
        bool isCanPutItemOnSeft = furniture.CanPutItemOnSeft;

        bool temp = CalculateoccupiedGrid(gridPosition, furniture, out List<Vector3Int> occupiedPosition);

        //Debug.Log("AddObjectAt : " + temp);

        if (!temp)
        {
            return false;   // guess this will never happend
        }

        PlacementData placementData;

        //Debug.Log("count = " + occupiedPosition.Count);
        foreach (var position in occupiedPosition)
        {
            //Debug.Log("cell(x:y) = " + position);
            if (cellPlacedData.ContainsKey(position) && !isCanPutItemOnSeft)
            {
                throw new Exception($"This ({position}) cell is occupied");     // guess this will never happend
            }
            else
            {
                placementData = new(occupiedPosition, furniture);

                cellPlacedData[position] = placementData;
                //Debug.Log("Add cell done");

                pathFinding.GetNode(position.x, position.z).SetWalkable(false);

            }
        }

        return true;
    }

    public bool CalculateoccupiedGrid(Vector3Int gridOffsetPosition, Furniture furniture, out List<Vector3Int> returnList)
    {
        //  1. Check if cell is occupied return false else true and calculate (2)
        //  2. Calculate cell need to occupied

        returnList = new List<Vector3Int>();

        for (int x = 0; x < furniture.Size.x; x++)
        {
            for (int y = 0; y < furniture.Size.y; y++)
            {
                Vector3Int cell = gridOffsetPosition + new Vector3Int(x, 0, y);
                if (cellPlacedData.ContainsKey(cell) && !furniture.CanPutItemOnSeft)
                {
                    //Debug.Log("Calculate : false");
                    returnList.Clear();
                    return false;
                }
                returnList.Add(cell);
            }
        }
        //Debug.Log("Calculate : true");
        return true;
    }

    public void RemoveObjectAt(List<Vector3Int> occupiedPosition, PathFinding pathFinding)
    {
        // Assume cellPlacedData and occupiedPosition are populated
        Debug.Log("occupiedPosition: " + occupiedPosition.Count)

        // Remove key-value pairs based on occupiedPosition list
        foreach (var position in occupiedPosition)
        {
            if (cellPlacedData.ContainsKey(position))
            {
                Debug.Log("Removing: " + position);
                // Remove the key-value pair
                cellPlacedData.Remove(position);

                // If you're using pathFinding, you might want to set the node back to walkable
                pathFinding.GetNode(position.x, position.z).SetWalkable(true);
            }
        }

        // Optional: Print the remaining items in cellPlacedData
        //foreach (var kvp in cellPlacedData)
        //{
        //    Debug.Log($"Remaining position: {kvp.Key}, Data: {kvp.Value}");
        //}
    }
}

internal class PlacementData
{
    public List<Vector3Int> occupiedCell;

    public Furniture FurnitureData { get; private set; }
    //public int PlacedObjIndex { get; private set; }

    public PlacementData(List<Vector3Int> occupiedCell, Furniture furniture/*, int placedObjIndex*/)
    {
        this.occupiedCell = occupiedCell;
        FurnitureData = furniture;
        //PlacedObjIndex = placedObjIndex;
    }
}