using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] Grid grid;
    [SerializeField] Mouse3D mousePointer;
    [SerializeField] Transform visualPointer;
    [SerializeField] FurnitureSO furnitureData;


    Vector3Int currentCellPosition;

    private void FixedUpdate()
    {
        currentCellPosition = grid.WorldToCell(mousePointer.transform.position);
        visualPointer.position = grid.CellToWorld(currentCellPosition);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {

        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {

        }
    }
}
