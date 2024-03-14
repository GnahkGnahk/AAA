using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] CinemachineVirtualCamera CM_TopDown, CM_Crouching;
    [SerializeField] GameObject miniCamera;

    private void Start()
    {
        SetUIPositionAndSize(miniCamera);
    }

    public void SetCameraOn(CameraType cam, bool isActive = true)
    {
        CinemachineVirtualCamera tempCam = cam == CameraType.TOP_DOWN ? CM_TopDown :
                                            cam == CameraType.CROUCHING ? CM_Crouching : null;

        tempCam.Priority = isActive ? 99 : 1;
    }

    public void SetPosition(Vector3 position)
    {
        transform.position += new Vector3(position.x, 0, position.z);
    }

    public void ActiveMiniCam(bool isActive = true)
    {
        miniCamera.SetActive(isActive);
    }
    void SetUIPositionAndSize(GameObject gameObject)
    {
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();

        float uiHeight = Screen.height / 2f;

        rectTransform.sizeDelta = new Vector2(uiHeight, uiHeight);

        rectTransform.anchorMin = new Vector2(1f, 0f);
        rectTransform.anchorMax = new Vector2(1f, 0f);
        rectTransform.pivot = new Vector2(1f, 0f);
        rectTransform.anchoredPosition = Vector2.zero;
    }
}
