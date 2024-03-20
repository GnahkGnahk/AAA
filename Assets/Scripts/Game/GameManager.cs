using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] ItemManager itemTrade;

    private void Start()
    {
        //itemTrade.gameObject.SetActive(false);
    }

    public void OpenItemTrade(bool isOn = true)
    {
        itemTrade.gameObject.SetActive(isOn);
    }
}
