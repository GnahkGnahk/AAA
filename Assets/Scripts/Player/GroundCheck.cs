using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] LayerMask platformLayerMask;
    //[HideInInspector] 
    public bool isGrounded;

    private void OnCollisionStay(Collision collision)
    {
        isGrounded = collision != null && (((1 << collision.gameObject.layer) & platformLayerMask) != 0);
    }
    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}
