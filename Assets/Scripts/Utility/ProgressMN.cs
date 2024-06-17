using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProgressMN : MonoBehaviour
{
    [SerializeField] Slider progressSlider;
    [SerializeField] Text progressValue;

    private void Start()
    {
        progressSlider.gameObject.SetActive(false);
    }

    public void OnClickLoadBtn(int sceneIndex)
    {
        progressSlider.gameObject.SetActive(true);
        StartCoroutine(LoadAsynchronous(sceneIndex));
    }

    IEnumerator LoadAsynchronous(int sceneIndex)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        while (!asyncOperation.isDone)
        {

            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            Debug.Log(progress);

            progressSlider.value = progress;
            progressValue.text = progress.ToString();

            yield return null;
        }
    }
}
