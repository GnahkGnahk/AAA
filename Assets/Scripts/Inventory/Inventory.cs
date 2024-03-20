using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour, IDropHandler
{
    [SerializeField] Transform holderTransform;
    [SerializeField] ItemTrade itemTradePrefab;
    [SerializeField] Sprite tempSpr;

    private void Start()
    {
        for (int i = 1; i <= 3; i++)
        {
            ItemTrade item = Instantiate(itemTradePrefab, holderTransform);
            item.SetData(holderTransform.transform, tempSpr, "Name_" + i, 100f - i, i);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Drop");
        if (eventData.pointerDrag != null)
        {
            if (eventData.pointerDrag.gameObject.tag != Helper.TAG_ITEM_TRADE) { return; }
            eventData.pointerDrag.transform.SetParent(holderTransform);
        }
    }
}
