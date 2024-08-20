using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridManager : MonoBehaviour
{
    [SerializeField] Grid grid;
    [SerializeField] Mouse3D mousePointer;
    [SerializeField] Transform visualPointer, visualCellFurniture, btnHolder, furnitureHolder;
    [SerializeField] FurnitureSO furnitureData;
    [SerializeField] FurnitureButton selectFurnitureBtnPrefab;

    public event Action<int> OnClick;
    public event Action OnExit;
    public bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();

    Vector3Int currentCellPosition;
    int currentCellIndex = -1;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (currentCellIndex < 0)
            {
                return;
            }
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
        if (IsPointerOverUI())
        {
            return;
        }
        GameObject furniture = Instantiate(furnitureData.listFurniture[currentCellIndex].Prefab);
        furniture.transform.position = currentCellPosition;
    }

    void StopPlacement()
    {
        currentCellIndex = -1;
        visualPointer.gameObject.SetActive(false);
    }

    public void SetScaleVisualPointer(Vector2 newScale, Material material)
    {
        visualPointer.gameObject.SetActive(true);
        float newScale_x = newScale.x;
        float newScale_z = newScale.y;
        visualPointer.transform.localScale = new Vector3(newScale_x, 1f, newScale_z);

        visualPointer.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = material;
    }
}
