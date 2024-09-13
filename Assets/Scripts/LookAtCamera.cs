using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public enum CameraLookMode
    {
        LookAt,
        LookAtInverted,

    }

    [SerializeField] CameraLookMode mode;

    private void LateUpdate()
    {
        switch (mode)
        {
            case CameraLookMode.LookAt:
                transform.LookAt(Camera.main.transform);
                break;

            case CameraLookMode.LookAtInverted:
                Vector3 dirFromCam = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + dirFromCam);
                break;

            default:
                break;
        }
        
    }
}
