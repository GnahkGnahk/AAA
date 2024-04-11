using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : Singleton<ItemManager>
{
    [SerializeField] Inventory gameInventory, playerInventory;
    [SerializeField] Button closeBtn;

    public bool IsDropValidPos { get; set; }

    public void OpenItemManager(bool isOpen)    //  is been using by OnClick CloseBtn 
    {
        GameManager.Instance.OpenItemTrade(isOpen);
    }

    public void SetUpWhileDragging(bool isDragging)
    {
        closeBtn.gameObject.SetActive(!isDragging);

        PlayerInputSys.Instance.SetEscape(!isDragging);
    }
}
