using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovingTroop : MonoBehaviour {
    [SerializeField] float speed = 1f;
    [SerializeField] float waypointThreshold = 0.01f;

    public void MoveToPositions(List<Vector3> targetPositions)
    {
        StartCoroutine(MoveObject(targetPositions));
    }

    IEnumerator MoveObject(List<Vector3> targetPositions)
    {
        int count = 1;
        foreach (Vector3 singleTarget in targetPositions)
        {
            Debug.Log(count);
            count += 1;
            yield return StartCoroutine(MoveToSinglePosition(singleTarget));
        }
    }

    IEnumerator MoveToSinglePosition(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > waypointThreshold)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPosition;
    }
}