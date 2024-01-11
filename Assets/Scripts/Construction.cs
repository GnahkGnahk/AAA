using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Construction : MonoBehaviour
{
    [SerializeField] Material constructionMaterial;
    [SerializeField] bool constructe;
    [SerializeField] float speed;

    const string PROGRESS = "_Progress";
    const float minProgress = -0.05f;
    const float maxProgress = 1f;

    float currentProgress;

    private void Awake()
    {
        constructe = false;

        currentProgress = minProgress;
    }

    private void Update()
    {
        constructionMaterial.SetFloat("_Progress", currentProgress);
        if (!constructe)
            return;

        if (currentProgress < maxProgress && currentProgress >= minProgress)
        {
            currentProgress += speed * Time.deltaTime;
        }
        else if (currentProgress > maxProgress)
        {
            currentProgress = minProgress;
        }

    }
}
