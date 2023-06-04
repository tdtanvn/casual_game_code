using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get => _instance; }
    private static MenuManager _instance;
    public static string sceneToLoad = null;
    public static string loadingTxt = null;
    public static bool isInitScene = false;

    private void Awake()
    {
        if (_instance == null)
            _instance = this; 
    }

    public static void LoadScene(string scene, bool showLoading = false, string loadingText = null, bool isInit = false)
    {
        if (showLoading)
        {
            sceneToLoad = scene;
            loadingTxt = loadingText;
            isInitScene = isInit;

            SceneManager.LoadScene(BuiltInScenes.LoadingScene);
        }
        else
        {
            SceneManager.LoadScene(scene);
        }
    }
}
