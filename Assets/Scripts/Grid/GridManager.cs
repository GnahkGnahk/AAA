using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridManager : MonoBehaviour
{
    [SerializeField] Grid grid;
    [SerializeField] Mouse3D mousePointer;
    [SerializeField] Transform visualPointer, btnHolder, furnitureHolder;
    [SerializeField] FurnitureSO furnitureData;
    [SerializeField] FurnitureButton selectFurnitureBtnPrefab;

    public event Action<int> OnClick;
    public event Action OnExit;
    public bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();

    Furniture currentFurnitureSelected = null;
    Vector3Int currentCellPosition;
    int currentCellIndex = -1;

    GridData floorGridData, furnitureGridData;
    bool isValidForPlace = false;

    private void Update()
    {
        if (currentFurnitureSelected != null)
        {
            if (currentFurnitureSelected.CanPutItemOnSeft)
            {
                isValidForPlace = floorGridData.CalculateoccupiedGrid(
                    currentCellPosition,
                    currentFurnitureSelected,
                    out _);
            }
            else
            {
                isValidForPlace = furnitureGridData.CalculateoccupiedGrid(
                    currentCellPosition,
                    currentFurnitureSelected,
                    out _);
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("Index / valid _ " + currentCellIndex + " / " + isValidForPlace);
            if (currentCellIndex < 0 || !isValidForPlace)
            {
                return;
            }
            Debug.Log("Invoke click");
            OnClick.Invoke(currentCellIndex);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnExit.Invoke();
        }

    }

    private void FixedUpdate()
    {
        currentCellPosition = grid.WorldToCell(mousePointer.transform.position);
        visualPointer.position = grid.CellToWorld(currentCellPosition);
    }

    private void Start()
    {
        OnClick += StartPlacement;
        OnExit += StopPlacement;

        InstantiateFurnitereBtn();

        visualPointer.gameObject.SetActive(false);

        floorGridData = new GridData();
        furnitureGridData = new GridData();
    }

    void InstantiateFurnitereBtn()
    {
        foreach (var furniture in furnitureData.listFurniture)
        {
            FurnitureButton btn = Instantiate(selectFurnitureBtnPrefab, btnHolder);
            btn.SetupData(furniture, this);            
        }
    }

    public void SetCurrentCellIndex(int ID)
    {
        currentCellIndex = furnitureData.listFurniture.FindIndex(f => f.ID == ID);
    }

    public void StartPlacement(int ID)
    {
        if (IsPointerOverUI()) return;

        GameObject furniture = Instantiate(furnitureData.listFurniture[currentCellIndex].Prefab, furnitureHolder);
        furniture.transform.position = currentCellPosition;

        if (currentFurnitureSelected.CanPutItemOnSeft)
        {
            floorGridData.AddObjectAt(currentCellPosition, currentFurnitureSelected);
        }
        else
        {
            furnitureGridData.AddObjectAt(currentCellPosition, currentFurnitureSelected);
        }
    }

    void StopPlacement()
    {
        currentFurnitureSelected = null;
        currentCellIndex = -1;
        visualPointer.gameObject.SetActive(false);
    }

    public void SetScaleVisualPointer(Furniture furniture, Material material)
    {
        visualPointer.gameObject.SetActive(true);
        float newScale_x = furniture.Size.x;
        float newScale_z = furniture.Size.y;
        visualPointer.transform.localScale = new Vector3(newScale_x, 1f, newScale_z);

        visualPointer.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = material;

        currentFurnitureSelected = furniture;
    }
}
