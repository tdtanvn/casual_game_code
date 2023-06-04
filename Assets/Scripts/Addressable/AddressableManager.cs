using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class AddressableManager : MonoBehaviour
{
    public static AddressableManager Instance;
    public AssetReference sceneReference;

    private AsyncOperationHandle<SceneInstance> sceneLoadHandle;
    private float sizeToDownload = -1;

    public static UnityEvent<float> ProgressEvent;
    public static UnityEvent<bool> CompletionEvent;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
        ProgressEvent = new UnityEvent<float>();
        CompletionEvent = new UnityEvent<bool>();
        // Addressables.ClearDependencyCacheAsync(sceneReference);
        Addressables.CheckForCatalogUpdates();
        StartCoroutine(GetDownloadSize());
    }

    public void OnPlayButtionClick()
    {
        if (IsAssetDownloaded())
        {
            StartCoroutine(LoadScene());
        }
        else
        {
            var data = new CommonPopup.PopupData(title: "DOWNLOAD", description: $"Download {sizeToDownload.ToString("0.##")} MB", null, "OK", () => DownloadScene(), "Cancel");
            GameManager.Instance.commonPopup.PushPopup(data);
        }
    }

    public IEnumerator GetDownloadSize()
    {
        AsyncOperationHandle<long> getDownloadSize = Addressables.GetDownloadSizeAsync(sceneReference);
        yield return getDownloadSize;
        sizeToDownload = getDownloadSize.Result / 1024f / 1024f;
        Debug.Log("Download size in MB: " + sizeToDownload.ToString("0.##"));
    }

    public bool IsAssetDownloaded()
    {
        return sizeToDownload == 0;
    }

    public void DownloadScene()
    {
        StartCoroutine(DownloadAndCacheScene());
    }

    private IEnumerator DownloadAndCacheScene()
    {
        AsyncOperationHandle downloadHandle = Addressables.DownloadDependenciesAsync(sceneReference);

        downloadHandle.Completed += (AsyncOperationHandle) =>
        {
            Debug.Log("Downloading asset complete");
            StartCoroutine(GetDownloadSize());
        };

        while (downloadHandle.PercentComplete < 1 && !downloadHandle.IsDone)
        {
            var progress = downloadHandle.GetDownloadStatus().Percent;
            Debug.Log("Downloading asset: " + progress.ToString());
            ProgressEvent.Invoke(progress);
            yield return null;
        }

        CompletionEvent.Invoke(downloadHandle.Status == AsyncOperationStatus.Succeeded);
        Addressables.Release(downloadHandle);
    }

    public IEnumerator LoadScene()
    {
        sceneLoadHandle = Addressables.LoadSceneAsync(sceneReference, LoadSceneMode.Single);
        sceneLoadHandle.Completed += OnSceneLoaded;

        while (sceneLoadHandle.PercentComplete < 1 && !sceneLoadHandle.IsDone)
        {
            Debug.Log("Loading asset: " + sceneLoadHandle.GetDownloadStatus().Percent.ToString());
            yield return null;
        }
    }

    private void OnSceneLoaded(AsyncOperationHandle<SceneInstance> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            SceneInstance sceneInstance = handle.Result;
            //SceneManager.LoadScene(sceneInstance.Scene.name);
        }
        else
        {
            Debug.LogError("Failed to load scene. Error: " + handle.OperationException.Message);
        }

        sceneLoadHandle.Completed -= OnSceneLoaded;
    }
}
