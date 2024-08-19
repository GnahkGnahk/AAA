using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridManager : MonoBehaviour
{
    [SerializeField] Grid grid;
    [SerializeField] Mouse3D mousePointer;
    [SerializeField] Transform visualPointer, visualCellFurniture, btnHolder;
    [SerializeField] FurnitureSO furnitureData;
    [SerializeField] FurnitureButton selectFurnitureBtnPrefab;

    public event Action OnClick, OnExit;
    public bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();

    Vector3Int currentCellPosition;
    int currentCellIndex = -1;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //OnClick.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //OnExit.Invoke();
        }
    }

    private void FixedUpdate()
    {
        currentCellPosition = grid.WorldToCell(mousePointer.transform.position);
        visualPointer.position = grid.CellToWorld(currentCellPosition);
    }

    private void Start()
    {
        StopPlacement();
        InstantiateFurnitereBtn();
    }

    void InstantiateFurnitereBtn()
    {
        foreach (var furniture in furnitureData.listFurniture)
        {
            FurnitureButton btn = Instantiate(selectFurnitureBtnPrefab, btnHolder);
            btn.SetupData(furniture, this);            
        }
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        currentCellIndex = furnitureData.listFurniture.FindIndex(f => f.ID == ID);
        if (currentCellIndex < 0)
        {
            Debug.LogError($"ID '{ID}' is not found.");
            return;
        }

        OnClick += PlaceStructure;
        OnExit += StopPlacement;
    }

    void StopPlacement()
    {
        OnClick -= PlaceStructure;
        OnExit -= StopPlacement;

    }

    private void PlaceStructure()
    {
        if (IsPointerOverUI())
        {
            return;
        }
        GameObject furniture = Instantiate(furnitureData.listFurniture[currentCellIndex].Prefab);
        furniture.transform.position = currentCellPosition;
    }
}
