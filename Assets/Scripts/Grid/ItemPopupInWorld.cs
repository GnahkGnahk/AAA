using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPopupInWorld : MonoBehaviour
{
    [SerializeField] Text nameTxt, lvlTxt, hpTxt;
    [SerializeField] GameObject overview;

    public void SetTextUI(string nameStr, string lvlStr, string hpStr)
    {
        overview.SetActive(true);
        nameTxt.text = nameStr;
        lvlTxt.text = lvlStr;
        hpTxt.text = hpStr;
    }
}
