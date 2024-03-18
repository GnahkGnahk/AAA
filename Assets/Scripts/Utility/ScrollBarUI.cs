using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBarUI : MonoBehaviour
{
    Scrollbar scrollbar;

    void Start()
    {
        scrollbar = GetComponent<Scrollbar>();
        StartCoroutine(ChangeScrollBarSize());
    }

    IEnumerator ChangeScrollBarSize()
    {
        yield return null;
        scrollbar.size = 0.1f;
        scrollbar.value = 1f;
    }

}
