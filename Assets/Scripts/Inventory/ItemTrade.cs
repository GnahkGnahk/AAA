using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTrade : MonoBehaviour
{
    [SerializeField] Image iconImg;
    [SerializeField] Text infoTxt;

    void SetData(Sprite iconSpr, string name, float durability, int quantity)
    {
        iconImg.sprite = iconSpr;

        infoTxt.text = "Item name: ";
        infoTxt.text += name + "\nQuantity: " + quantity + "\nDurability: " + durability;
    }
}
