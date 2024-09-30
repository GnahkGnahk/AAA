using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridMnTest : MonoBehaviour
{
    [SerializeField] Grid grid;
    [SerializeField] Mouse3D mousePointer;

    public event Action<Vector3Int> OnLeftMouseClick;
    public event Action OnExit;

    Vector3Int currentCellPosition;

    private void Update()
    {
        mousePointer.RayCastPointer();

        currentCellPosition = grid.WorldToCell(mousePointer.transform.position);
        mousePointer.SetupVisual(grid.CellToWorld(currentCellPosition));

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            OnLeftMouseClick.Invoke(currentCellPosition);
        }
    }

    private void Start()
    {
        OnLeftMouseClick += LeftMouseClick;
    }

    public void LeftMouseClick(Vector3Int position)
    {
        Debug.Log("currentCellPosition: " + position);
    }
}
