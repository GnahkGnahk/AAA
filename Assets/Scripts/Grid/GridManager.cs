using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

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

    [SerializeField] Material selectedMate;

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

    BaseArchitecture currentArchitecture = null;

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

            #region Cancel selected Furniture
            if (currentArchitecture && grid.WorldToCell(currentArchitecture.transform.position) != currentCellPosition)
            {
                ReleaseDragItem();
            }
            #endregion
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //  Cancel place object
            OnExit.Invoke();
            if (currentArchitecture)
            {
                ReleaseDragItem();
            }
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
            dragDrop.Initialize(this, furnitureData.listFurniture[currentFurnitureID]);
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
    public void SetScaleVisualPointer(Vector2 scaleSize = default)
    {
        //
        float newScale_x = scaleSize.x;
        float newScale_z = scaleSize.y;
        currentArchitecture.LandVisual.transform.localScale = new Vector3(newScale_x, 1f, newScale_z);

        currentArchitecture.LandVisual.GetComponent<MeshRenderer>().material = selectedMate;
    }

    public void SetDragItem(BaseArchitecture item)
    {
        if (currentArchitecture != null)
        {
            ReleaseDragItem();
        }

        item.ShowLandVisual();
        currentArchitecture = item;
        item.DragAndDrop.canDrag = true;
        SetScaleVisualPointer(item.furniture.Size);
    }

    public void ReleaseDragItem()
    {
        currentArchitecture.DragAndDrop.canDrag = false;
        currentArchitecture.ShowLandVisual(false);

        #region Check new position is valid

        #endregion

        currentArchitecture = null;
    }
}
