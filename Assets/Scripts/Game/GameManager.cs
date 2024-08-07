using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] ItemManager itemTrade;
    
    [HideInInspector] public bool isGamePaused = false;


    private void Start()
    {
        itemTrade.gameObject.SetActive(false);
    }

    public void OpenItemTrade(bool isOn = true)
    {
        if (!PlayerManager.Instance.isPickingItem) return;

        itemTrade.gameObject.SetActive(isOn);
        PauseGame(isOn);
    }

    public void PauseGame(bool isPause = true)
    {
        if (!isPause)
        {
            Time.timeScale = 1.0f;
            return;
        }

        Time.timeScale = 0;
    }
}
