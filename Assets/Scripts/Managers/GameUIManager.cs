using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    [Header("Scene Loader")]
    public SceneLoader sceneLoader;

    [Header("Scene Names")]
    public string mainMenuSceneName;

    public void RetryLevel()
    {
        if (sceneLoader == null)
        {
            Debug.LogWarning("[GameUIManager] Scene Loader not assigned. Retrying without fade.");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }

        sceneLoader.nextSceneName = SceneManager.GetActiveScene().name;
        sceneLoader.LoadNextScene();
    }

    public void GoToMainMenu()
    {
        if (string.IsNullOrEmpty(mainMenuSceneName))
        {
            Debug.LogWarning("[GameUIManager] Main Menu Scene Name is not set in the Inspector.");
            return;
        }

        if (sceneLoader == null)
        {
            Debug.LogWarning("[GameUIManager] Scene Loader not assigned. Loading without fade.");
            SceneManager.LoadScene(mainMenuSceneName);
            return;
        }

        sceneLoader.nextSceneName = mainMenuSceneName;
        sceneLoader.LoadNextScene();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_IOS
    Debug.Log("[GameUIManager] Quit is not supported on iOS.");
#else
    Application.Quit(); // Works on Android and other platforms
#endif
    }
}
