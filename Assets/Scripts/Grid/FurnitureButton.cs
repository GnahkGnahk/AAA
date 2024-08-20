using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FurnitureButton : MonoBehaviour
{
    [SerializeField] Text furnitureName;
    public Furniture FurnitureData { get; set; }
    public GridManager GridManagerData { get; set; }

    public void SetupData(Furniture data, GridManager gridManager)
    {
        FurnitureData = data;
        GridManagerData = gridManager;

        furnitureName.text = FurnitureData.Name;
        gameObject.name = FurnitureData.Name;
    }

    public void OnClickBtn()
    {
        GridManagerData.SetCurrentCellIndex(FurnitureData.ID);
        GridManagerData.SetScaleVisualPointer(FurnitureData.Size, FurnitureData.Prefab.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().sharedMaterial);
    }
}
