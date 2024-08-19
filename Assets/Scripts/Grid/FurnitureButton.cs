using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FurnitureButton : MonoBehaviour
{
    [SerializeField] Text furnitureName;
    public Furniture furnitureData { get; set; }
    public GridManager gridManagerData { get; set; }

    public void SetupData(Furniture data, GridManager gridManager)
    {
        furnitureData = data;
        gridManagerData = gridManager;

        furnitureName.text = furnitureData.Name;
        gameObject.name = furnitureData.Name;

        gameObject.GetComponent<Button>().onClick.AddListener(() => gridManagerData.StartPlacement(furnitureData.ID));
    }
}
