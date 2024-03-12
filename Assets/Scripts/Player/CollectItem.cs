using UnityEngine;

public class CollectItem : MonoBehaviour
{
    public string tagItem;
    public Transform objectTransform;

    private void OnTriggerStay(Collider other)
    {
        tagItem = other.gameObject.tag;
        objectTransform = other.gameObject.transform;
    }
    private void OnTriggerExit(Collider collision)
    {
        tagItem = null;
        objectTransform = null;
    }
}
