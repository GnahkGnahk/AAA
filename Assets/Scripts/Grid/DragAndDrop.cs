using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    [SerializeField] GridManager gridManager;
    [SerializeField] BaseArchitecture baseArchitecture;

    internal bool canDrag = false;
    private Vector3 initialMousePosition;

    private void OnMouseDown()
    {
        initialMousePosition = Input.mousePosition;
    }

    private void OnMouseDrag()
    {
        if (canDrag)
        {
            transform.position = gridManager.NewPositionArchitect();
        }
    }

    private void OnMouseUp()
    {
        if (initialMousePosition != Input.mousePosition) return;

        if (!canDrag)
        {
            gridManager.SetDragItem(baseArchitecture);
        }
    }

    public void Setup(BaseArchitecture baseArchitecture)
    {
        this.baseArchitecture = baseArchitecture;
        gridManager = baseArchitecture.GridManager;
    }


}
