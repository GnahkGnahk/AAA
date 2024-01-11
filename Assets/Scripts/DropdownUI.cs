using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownUI : MonoBehaviour
{
    public Dropdown dropdown;

    private void Start()
    {
        dropdown = GetComponent<Dropdown>();
    }

    public void OnSelectOptionDropdown(int i)
    {
        switch (i)
        {
            case 0:
                Debug.Log(dropdown.options[i].text);
                break;
            case 1:
                Debug.Log(dropdown.options[i].text);
                break;
            case 2:
                Debug.Log(dropdown.options[i].text);
                break;
            case 3:
                Debug.Log(dropdown.options[i].text);
                break;
            case 4:
                Debug.Log(dropdown.options[i].text);
                break;
            case 5:
                Debug.Log(dropdown.options[i].text);
                break;
            case 7:
                Debug.Log(dropdown.options[i].text);
                break;
            default:
                Debug.Log("Default");
                break;
        }
    }
}
