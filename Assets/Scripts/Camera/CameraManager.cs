using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] CinemachineVirtualCamera CM_TopDown, CM_Crouching;

    public void SetCameraOn(CameraType cam, bool isActive = true)
    {
        CinemachineVirtualCamera tempCam = cam == CameraType.TOP_DOWN ? CM_TopDown :
                                            cam == CameraType.CROUCHING ? CM_Crouching : null;

        tempCam.Priority = isActive ? 99 : 1;
    }

    public void SetPosition(Vector3 position)
    {
        transform.position += position;
    }
}
