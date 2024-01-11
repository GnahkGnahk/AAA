using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleTest : MonoBehaviour
{
    Toggle toggle;
    Button btn;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();

        /*toggle.onValueChanged.AddListener((bool a) =>
        {
            if (a)
            {
                Debug.Log(gameObject.name +" ACTIVATED ");
            }
            else
            {
                Debug.Log(gameObject.name + " INACTIVATED ");
            }
        });*/
    }

    public void OnValueChange()
    {
        if (toggle.isOn)
        {
            Debug.Log(gameObject.name + " ACTIVATED ");
            return;
        }
        Debug.Log(gameObject.name + " INACTIVATED ");
    }
}
