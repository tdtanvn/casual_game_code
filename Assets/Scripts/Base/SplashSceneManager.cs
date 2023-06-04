using UnityEngine;

public class SplashSceneManager : MonoBehaviour
{
    void Start()
    {
        MenuManager.LoadScene(BuiltInScenes.Login, showLoading: true);
    }
}
