using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemTrade : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] Image iconImg;
    [SerializeField] Text infoTxt;

    RectTransform rectTransform;
    CanvasGroup canvasGroup;

    public Transform rootParent;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetData(Transform rootP, Sprite iconSpr, string name, float durability, int quantity)
    {
        rootParent = rootP;
        iconImg.sprite = iconSpr;

        infoTxt.text = "Item name: ";
        infoTxt.text += name + "\nQuantity: " + quantity + "\nDurability: " + durability;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("Begin");
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        eventData.pointerDrag.transform.SetParent(rootParent.parent.parent);

        ItemManager.Instance.SetUpWhileDragging(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Drag");
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("End");
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        ItemManager.Instance.SetUpWhileDragging(false);
    }
}
