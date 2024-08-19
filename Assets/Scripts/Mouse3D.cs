using UnityEngine;

public class Mouse3D : MonoBehaviour
{
    [SerializeField] Camera _camera;
    [SerializeField] LayerMask _layerMask;
    [SerializeField] bool isSpecificLayerMask;


    private void Update()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        bool hit;

        if (isSpecificLayerMask)
        {
            hit = Physics.Raycast(ray, out hitInfo, float.MaxValue, _layerMask);
        }
        else
        {
            hit = Physics.Raycast(ray, out hitInfo);
        }

        if (hit)
        {
            transform.position = hitInfo.point;
        }
    }
}
