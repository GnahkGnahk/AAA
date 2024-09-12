using System;
using UnityEngine;

[Serializable]
public class BaseArchitecture : MonoBehaviour
{
    [SerializeField] ItemPopupInWorld popupOverview;

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
}

[Serializable]
public enum ArchitectureType
{
    MAIN_TOWNHALL,
    PRODUCTION_GOLD,
    PRODUCTION_TROOP,
    DEFENSE_FENCE
}
