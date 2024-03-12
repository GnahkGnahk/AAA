using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] LayerMask platformLayerMask;
    //[HideInInspector] 
    public bool isGrounded;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == Helper.LAYER_PLATFORM)
        {
            isGrounded = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == Helper.LAYER_PLATFORM)
        {
            isGrounded = false;
        }
    }
}