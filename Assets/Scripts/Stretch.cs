using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Stretch : MonoBehaviour
{
    [SerializeField] Transform worldAnchor, displayArrow;

    Camera mainCamera;
    Vector3 initPosition, initialRotation, initialScale;
    float camZDistance;

    private void Start()
    {

        initPosition = transform.localPosition;
        //initialRotation = transform.localRotation;
        initialScale = transform.localScale;

        mainCamera = Camera.main;
        camZDistance = mainCamera.WorldToScreenPoint(transform.position).z;

        displayArrow.gameObject.SetActive(false);
    }

    private void Update()
    {
    }

    private void OnMouseDrag()
    {
        displayArrow.gameObject.SetActive(true);

        Vector3 mouseScreenPos = new(Input.mousePosition.x, Input.mousePosition.y, camZDistance);
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);

        // Transform
        float distance = Vector3.Distance(worldAnchor.position, mouseWorldPos);
        transform.localScale = new(initialScale.x, initialScale.y, distance/8f); //scale

        Vector3 middlePoint = (worldAnchor.position + mouseWorldPos) / 2f;
        transform.position = middlePoint;  //Position

        Vector3 rotateDir = new(mouseWorldPos.x - worldAnchor.position.x,
            mouseWorldPos.y - worldAnchor.position.y,
            mouseWorldPos.z - worldAnchor.position.z);
        transform.forward = rotateDir;  // Rotation

    }

    private void OnMouseExit()
    {
        //reset transform
        transform.localPosition = initPosition;
        transform.localScale = initialScale;


        displayArrow.gameObject.SetActive(false);
    }
}
