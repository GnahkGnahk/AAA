using System;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable]
public class BaseArchitecture : MonoBehaviour
{
    [SerializeField] ItemPopupInWorld popupOverview;
    [SerializeField] GridManager gridManager;
    [SerializeField] EventTrigger eventTrigger;

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

    public void SetupData(GridManager gridManager)
    {
        Debug.Log("SetupData");
        this.gridManager = gridManager;
        eventTrigger = gameObject.GetComponent<EventTrigger>();

        if (eventTrigger)
        {
            Debug.Log("eventTrigger set up");

            EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry();
            pointerDownEntry.eventID = EventTriggerType.PointerDown;
            pointerDownEntry.callback.AddListener((data) => { gridManager.SetDragItem((PointerEventData)data); });
            eventTrigger.triggers.Add(pointerDownEntry);

            // Set up Pointer Up event
            EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry();
            pointerUpEntry.eventID = EventTriggerType.PointerUp;
            pointerUpEntry.callback.AddListener((data) => { gridManager.ReleaseDragItem(); });
            eventTrigger.triggers.Add(pointerUpEntry);

        }
        else
        {
            Debug.Log("eventTrigger NULL");
        }
    }
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
