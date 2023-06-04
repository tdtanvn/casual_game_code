using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public TextMeshProUGUI progressText;
    public Slider progressSlider;
    public TextMeshProUGUI loadingText;

    public float minFakeLoadTime = 0;
    private float loadTime;
    private float lastProgress;
    private bool isWaiting;

    private void Start()
    {
        loadTime = 0;
        lastProgress = 0;
        if (string.IsNullOrEmpty(MenuManager.loadingTxt))
            loadingText.gameObject.SetActive(false);
        else
            loadingText.text = MenuManager.loadingTxt;

        if (MenuManager.isInitScene)
        {
            isWaiting = true;
            Messenger.OnInitialized += OnInitialized;
        }
        else
        {
            isWaiting = false;
        }

        UpdateAssetLoadProgress(0);

        StartCoroutine(LoadSceneAsync(MenuManager.sceneToLoad));
    }

    private void OnInitialized()
    {
        isWaiting = false;
    }

    IEnumerator LoadSceneAsync(string sceneToLoad)
    {
        yield return null;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad);
        asyncOperation.allowSceneActivation = false;

        while (loadTime < minFakeLoadTime)
        {
            loadTime += Time.deltaTime;

            UpdateAssetLoadProgress(loadTime / minFakeLoadTime * asyncOperation.progress);
            yield return null;
        }

        while (asyncOperation.progress < 0.9f)
        {
            UpdateAssetLoadProgress(asyncOperation.progress);
            yield return null;
        }

        while (isWaiting)
        {
            if (MenuManager.isInitScene)
                loadingText.text = "Loading game data";
            yield return null;
        }

        asyncOperation.allowSceneActivation = true;
    }


    private void UpdateAssetLoadProgress(float progress)
    {
        progress /= .9f;

        if (lastProgress > progress) return;

        lastProgress = progress;

        int intProg = Mathf.Clamp(Mathf.RoundToInt(progress * 100.0f), 0, 100);
        progressText.text = intProg + "%";        

        progressSlider.value = progress;
    }
}
