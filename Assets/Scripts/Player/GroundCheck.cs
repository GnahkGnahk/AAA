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
        if (collision.gameObject.layer == Helper.TAG_PLATFORM)
        {
            isGrounded = true;
            Debug.Log("Stay: " + isGrounded);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == Helper.TAG_PLATFORM)
        {
            isGrounded = false;
            Debug.Log("Exit : " + isGrounded);
        }
    }
}