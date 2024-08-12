using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] Grid grid;
    [SerializeField] Mouse3D mousePointer;
    [SerializeField] Transform visualPointer;

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
            Debug.Log("mousePointer : " + mousePointer.transform.position);
            Debug.Log("cellPosition : " + currentCellPosition);
            Debug.Log("visualPointer : " + visualPointer.transform.position);
        }
    }
}
