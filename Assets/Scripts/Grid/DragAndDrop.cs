using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    [SerializeField] GridManager gridManager;

    //private void OnMouseDown()
    //{
    //    Debug.Log("OnMouseDown");
    //    //gridManager.currentCellPosition
    //}

    //private void OnMouseDrag()
    //{
    //    Debug.Log("OnMouseDrag");
    //    transform.position = gridManager.currentCellPosition;
    //}

    public void Setup(GridManager gridManager)
    {
        this.gridManager = gridManager;
    }


}
