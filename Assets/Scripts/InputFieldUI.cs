using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputFieldUI : MonoBehaviour
{
    // When enter 1 character -> OnValueChange
    public void OnValueChange(string str)
    {
        Debug.Log("OnValueChange : " + str);
    }

    // When enter -> OnSubmid -> OnEndEdit
    public void OnSubmid(string str)
    {
        Debug.Log("OnSubmid : " + str);
    }

    // When click outside (not focus on this text input) -> OnEndEdit
    public void OnEndEdit(string str)
    {
        Debug.Log("OnEndEdit : " + str);
    }

}
