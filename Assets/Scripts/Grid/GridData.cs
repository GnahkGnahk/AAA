using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridData
{
    Dictionary<Vector3Int, PlacementData> cellPlacedData = new();

    public void AddObjectAt(Vector3Int gridPosition, Furniture furniture, int index = 0)
    {
        bool isCanPutItemOnSeft = furniture.CanPutItemOnSeft;

        if (!CalculateoccupiedGrid(gridPosition, furniture, out List<Vector3Int> occupiedPosition))
        {
            return; 
        }

        PlacementData placementData;

        foreach (var position in occupiedPosition)
        {
            if (cellPlacedData.ContainsKey(position) && !isCanPutItemOnSeft)
            {
                throw new Exception($"This ({position}) cell is occupied");     // guess this will never happend
            }
            else
            {
                placementData = new(occupiedPosition, furniture, index);

                cellPlacedData[gridPosition] = placementData;
                Debug.Log("Add cell done"); //  not log ?
            }
        }
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
                    Debug.Log("false");
                    returnList.Clear();
                    return false;
                }
                returnList.Add(cell);
            }
        }
        Debug.Log("true");
        return true;
    }
}

internal class PlacementData
{
    public List<Vector3Int> occupiedCell;

    public Furniture FurnitureData { get; private set; }
    public int PlacedObjIndex { get; private set; }

    public PlacementData(List<Vector3Int> occupiedCell, Furniture furniture, int placedObjIndex)
    {
        this.occupiedCell = occupiedCell;
        FurnitureData = furniture;
        PlacedObjIndex = placedObjIndex;
    }
}