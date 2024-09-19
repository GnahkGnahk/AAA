using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridManager : MonoBehaviour
{
    [SerializeField] Grid grid;
    [SerializeField] Mouse3D mousePointer;
    [SerializeField] Transform visualPointer, btnHolder, furnitureHolder, clounHolder;
    [SerializeField] FurnitureSO furnitureData;
    [SerializeField] FurnitureButton selectFurnitureBtnPrefab;
    [SerializeField] Vector2 spawnAmount;

    [SerializeField] MovingTroop troops;

    public event Action<int> OnClick;
    public event Action OnExit;
    public bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();

    Furniture currentFurnitureSelected = null;
    Vector3Int currentCellPosition;
    int currentFurnitureID = -1;

    GridData cloudGridData, furnitureGridData;
    bool isValidForPlace = false;

    internal Vector3Int bottomLeftLocation;

    PathFinding pathFinding;
    int startX, startY, endX, endY;

    private void Update()
    {
        mousePointer.RayCastPointer();

        currentCellPosition = grid.WorldToCell(mousePointer.transform.position);
        visualPointer.position = grid.CellToWorld(currentCellPosition);

        if (currentFurnitureSelected != null)
        {
            if (currentFurnitureSelected.CanPutItemOnSeft)
            {
                isValidForPlace = cloudGridData.CalculateoccupiedGrid(
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
            //Debug.Log("Index / valid _ " + currentCellIndex + " / " + isValidForPlace);
            if (currentFurnitureID < 0 || !isValidForPlace)
            {
                //return;
            }
            else
            {
                //Debug.Log("Invoke click");
                OnClick.Invoke(currentFurnitureID);
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnExit.Invoke();
        }


        
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            endX = currentCellPosition.x;
            endY = currentCellPosition.z;
            Debug.Log("endX : " + endX + ", endY : " + endY);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3Int troopPos = grid.WorldToCell(troops.transform.position);
            startX = troopPos.x;
            startY = troopPos.z;

            Debug.Log("startX : " + startX + ", startY : " + startY);
            List<PathNod> path = pathFinding.FindPath(startX, startY, endX, endY);
            troops.MoveToPositions(NodeToV3(path));
        }
    }

    List<Vector3> NodeToV3(List<PathNod> pathNode)
    {
        List<Vector3> vector3s = new();
        for (int i = 0; i < pathNode.Count; i++)
        {
            vector3s.Add(new(pathNode[i].x, 1f, pathNode[i].y));
        }

        return vector3s;
    }

    private void FixedUpdate()
    {
        //Debug.Log("visualPointer : " + visualPointer.position);
    }

    private void Start()
    {
        OnClick += StartPlacement;
        OnExit += StopPlacement;

        InstantiateFurnitereBtn();

        visualPointer.gameObject.SetActive(false);

        cloudGridData = new GridData();
        furnitureGridData = new GridData();

        bottomLeftLocation = new((int)grid.transform.localScale.x * -5, (int)grid.transform.localScale.y * -5, (int)grid.transform.localScale.z * -5);

        //SetUpCloud();

        pathFinding = new(bottomLeftLocation);
    }

    void SetUpCloud()
    {
        Vector3 tempPos = grid.CellToWorld(bottomLeftLocation);
        for (int i = 0; i < spawnAmount.x; i++)
        {
            for (int j = 0; j < spawnAmount.y; j++)
            {
                Vector3Int tmpPos = new((int)tempPos.x + i, 1, (int)tempPos.z + j);

                int tempID = furnitureData.listFurniture.FindIndex(f => f.Name == "Clound");
                Furniture fur = furnitureData.listFurniture[tempID];

                GameObject gameObject_Cloud = Instantiate(fur.Prefab, tmpPos, Quaternion.identity);
                gameObject_Cloud.name = tmpPos.x + "_" + tmpPos.z;
                gameObject_Cloud.transform.parent = clounHolder.transform;
                cloudGridData.AddObjectAt(tmpPos, fur);

            }
        }
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
        currentFurnitureID = furnitureData.listFurniture.FindIndex(f => f.ID == ID);
    }

    public void StartPlacement(int ID)
    {
        if (IsPointerOverUI()) return;

        GameObject furniture = Instantiate(furnitureData.listFurniture[currentFurnitureID].Prefab, furnitureHolder);
        furniture.transform.position = currentCellPosition;

        if (currentFurnitureSelected.CanPutItemOnSeft)
        {
            cloudGridData.AddObjectAt(currentCellPosition, currentFurnitureSelected);            
        }
        else
        {
            if (furnitureGridData.AddObjectAt(currentCellPosition, currentFurnitureSelected))
            {
                // Success place object
                //float distance = Vector3.Distance(currentCellPosition, bottomLeftLocation);   
            }
        }
    }

    void StopPlacement()
    {
        currentFurnitureSelected = null;
        currentFurnitureID = -1;
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
