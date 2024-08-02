using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : Singleton<Manager>
{
    [SerializeField] int width, height;
    [SerializeField] float cellSize;
    Grid grid;
    void Start()
    {
        InItGrid();
    }

    void InItGrid()
    {
        grid = new Grid(width, height, cellSize, default);
        grid.DrawGrid(parentTransform: transform);

    }
}
