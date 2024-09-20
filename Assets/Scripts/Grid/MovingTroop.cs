using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovingTroop : MonoBehaviour {
    [SerializeField] float speed = 1f;
    [SerializeField] float waypointThreshold = 0.01f;
    [SerializeField] TextureCreate textureCloundHandling;
    [SerializeField] GridManager gridManager;

    int offset = 0;

    private void Start()
    {
        offset = Mathf.Abs(gridManager.bottomLeftLocation.x);
        Debug.Log("offset: " + offset);
    }

    public void MoveToPositions(List<PathNod> targetPositions)
    {
        StartCoroutine(MoveObject(targetPositions));
    }

    IEnumerator MoveObject(List<PathNod> targetPositions)
    {
        foreach (PathNod singleTarget in targetPositions)
        {
            yield return StartCoroutine(MoveToSinglePosition(singleTarget));
        }
    }

    IEnumerator MoveToSinglePosition(PathNod targetPositionNode)
    {
        Vector3 targetPosition = NodeToV3(targetPositionNode);
        while (Vector3.Distance(transform.position, targetPosition) > waypointThreshold)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }

        textureCloundHandling.UpdateTexture(targetPositionNode.x + offset, targetPositionNode.y + offset);

        transform.position = targetPosition;
    }

    List<Vector3> NodeToV3(List<PathNod> pathNode)
    {
        List<Vector3> vector3s = new();
        for (int i = 0; i < pathNode.Count; i++)
        {
            vector3s.Add(NodeToV3(pathNode[i]));
        }

        return vector3s;
    }

    Vector3 NodeToV3(PathNod pathNode)
    {
        return new(pathNode.x, 1f, pathNode.y);
    }
}