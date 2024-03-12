using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectItem : MonoBehaviour
{
    public string tagItem;
    private void OnTriggerStay(Collider other)
    {
        tagItem = other.gameObject.tag;
    }
    private void OnTriggerExit(Collider collision)
    {
        tagItem = null;
    }
}
