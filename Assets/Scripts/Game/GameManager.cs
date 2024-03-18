using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] GameObject itemTrade;

    private void Start()
    {
        itemTrade.SetActive(false);
    }

    public void OpenItemTrade(bool isOn = true)
    {
        itemTrade.SetActive(isOn);
    }
}
