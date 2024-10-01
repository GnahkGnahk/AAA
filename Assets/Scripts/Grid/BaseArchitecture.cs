using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable]
public class BaseArchitecture : MonoBehaviour
{
    [SerializeField] ItemPopupInWorld popupOverview;
    [SerializeField] GridManager gridManager;
    [SerializeField] DragAndDrop dragAndDrop;
    [SerializeField] GameObject landVisual;

    public Furniture furniture = null;

    internal virtual void ActivePopupInWorld(Furniture furniture, bool active = true)
    {
        if (popupOverview == null || !active)
        {
            return;
        }
        popupOverview.SetTextUI(furniture.Name, furniture.Level.ToString(), furniture.Hp.ToString());
    }


    internal virtual void UpgradeArchitecture(Furniture furniture)
    {
        furniture.Level += 1;
        furniture.Hp += UnityEngine.Random.Range(1.0f, 10.0f);
    }
    public void Initialize(GridManager gridManager, Furniture furniture)
    {
        this.gridManager = gridManager;
        dragAndDrop.Setup(this);
        this.furniture = furniture;
    }

    internal void ShowLandVisual(bool isShow = true) => landVisual.SetActive(isShow);

    public GridManager GridManager => gridManager;
    public DragAndDrop DragAndDrop => dragAndDrop;
    public GameObject LandVisual => landVisual;
}

[Serializable]
public enum ArchitectureType
{
    MAIN_TOWNHALL,
    PRODUCTION_GOLD,
    PRODUCTION_TROOP,
    DEFENSE_FENCE,
    OTHER,
}
