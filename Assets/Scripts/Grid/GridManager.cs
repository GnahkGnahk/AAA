using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridManager : MonoBehaviour
{
    [SerializeField] Grid grid;
    [SerializeField] Mouse3D mousePointer;
    [SerializeField] Transform visualPointer, btnHolder, furnitureHolder;
    [SerializeField] FurnitureSO furnitureData;
    [SerializeField] FurnitureButton selectFurnitureBtnPrefab;
    [SerializeField] Vector2 spawnAmount;
    [Range(0, 10)][SerializeField] int visionAtStart = 5;

    [SerializeField] MovingTroop troops;

    [SerializeField] TextureCreate textureCloundHandling;

    public event Action<int> OnClick;
    public event Action OnExit;
    public bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();

    Furniture currentFurnitureSelected = null;
    internal Vector3Int currentCellPosition;
    int currentFurnitureID = -1;

    GridData cloudGridData, furnitureGridData;
    bool isValidForPlace = false;

    internal Vector3Int bottomLeftLocation;

    PathFinding pathFinding;
    int startX, startY, endX, endY;
    int offset = 0;

    BaseArchitecture dragAndDropItem = null;

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
            #region Place Object
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
            #endregion
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //  Cancel place object
            OnExit.Invoke();
        }


        
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            #region Select destination for troop
            endX = currentCellPosition.x;
            endY = currentCellPosition.z;
            Debug.Log("endX : " + endX + ", endY : " + endY); 
            #endregion
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            #region Troop start going
            Vector3Int troopPos = grid.WorldToCell(troops.transform.position);
            startX = troopPos.x;
            startY = troopPos.z;

            Debug.Log("startX : " + startX + ", startY : " + startY);

            List<PathNod> path = pathFinding.FindPath(startX, startY, endX, endY);
            if (path != null)
            {
                troops.MoveToPositions(path);
            }
            else
            {
                Debug.Log("NO PATH CAN FIND");
            } 
            #endregion
        }

        if (dragAndDropItem)
        {
            Debug.Log("Dragging");
            dragAndDropItem.transform.position = currentCellPosition;
        }
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
        offset = Mathf.Abs(bottomLeftLocation.x);

        //SetUpCloud();

        pathFinding = new(bottomLeftLocation);

        textureCloundHandling.GenerateTexture();
        textureCloundHandling.ClearCloud(troops.transform.position.x + offset, troops.transform.position.z + offset, visionAtStart);
    }

    void SetUpCloud()
    {
        Vector3 tempPos = grid.CellToWorld(bottomLeftLocation);
        for (int i = 0; i < spawnAmount.x; i++)
        {
            for (int j = 0; j < spawnAmount.y; j++)
            {
                Vector3Int tmpPos = new((int)tempPos.x + i, 1, (int)tempPos.z + j);


                //  Spawn object
                //int tempID = furnitureData.listFurniture.FindIndex(f => f.Name == "Clound");
                //Furniture fur = furnitureData.listFurniture[tempID];
                //GameObject gameObject_Cloud = Instantiate(fur.Prefab, tmpPos, Quaternion.identity);
                //gameObject_Cloud.name = tmpPos.x + "_" + tmpPos.z;
                //gameObject_Cloud.transform.parent = clounHolder.transform;
                //cloudGridData.AddObjectAt(tmpPos, fur);

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

        if (furniture.TryGetComponent<BaseArchitecture>(out var dragDrop))
        {
            Debug.Log("FOUND");
            dragDrop.SetupData(this);
        }
        else
        {
            Debug.Log("NULL");
        }

        if (currentFurnitureSelected.CanPutItemOnSeft)
        {
            //cloudGridData.AddObjectAt(currentCellPosition, currentFurnitureSelected, pathFinding);            
        }
        else
        {
            if (furnitureGridData.AddObjectAt(currentCellPosition, currentFurnitureSelected, pathFinding))
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
        //
        visualPointer.gameObject.SetActive(true);
        float newScale_x = furniture.Size.x;
        float newScale_z = furniture.Size.y;
        visualPointer.transform.localScale = new Vector3(newScale_x, 1f, newScale_z);

        visualPointer.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = material;

        currentFurnitureSelected = furniture;
    }

    public void SetDragItem(PointerEventData eventData)
    {
        // Assume we have a way to get the BaseArchitecture item from the event data
        BaseArchitecture item = GetItemFromEventData(eventData);
        dragAndDropItem = item;
    }

    public void ReleaseDragItem()
    {
        dragAndDropItem = null;
    }
    private BaseArchitecture GetItemFromEventData(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            BaseArchitecture clickedItem = eventData.pointerCurrentRaycast.gameObject.GetComponent<BaseArchitecture>();
            if (clickedItem != null)
            {
                return clickedItem;
            }
        }

        // If the clicked object itself isn't a BaseArchitecture, check its parent
        GameObject clickedObject = eventData.pointerCurrentRaycast.gameObject;
        if (clickedObject != null)
        {
            BaseArchitecture parentItem = clickedObject.GetComponentInParent<BaseArchitecture>();
            if (parentItem != null)
            {
                return parentItem;
            }
        }

        // If no BaseArchitecture found, return null
        Debug.Log("No BaseArchitecture item found");
        return null;
    }
}
