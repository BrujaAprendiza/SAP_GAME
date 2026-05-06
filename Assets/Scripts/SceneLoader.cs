using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [Header("Scene Settings")]
    public string nextSceneName;

    [Header("Fade Settings")]
    public Image fadePanel;

    [Range(0.1f, 3f)]
    public float fadeDuration = 1f;

    private bool _isTransitioning = false;

    private void Start()
    {
        if (fadePanel == null)
        {
            Debug.LogError("[SceneLoader] Fade Panel is not assigned! Please assign it in the Inspector.");
            return;
        }

        SetFadeAlpha(0f);
        fadePanel.raycastTarget = false;
    }

    /// <summary>
    /// Call this method from a UI Button's OnClick() event to trigger the scene transition.
    /// </summary>
    public void LoadNextScene()
    {
        if (_isTransitioning)
            return;

        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogError("[SceneLoader] Next Scene Name is empty! Set it in the Inspector.");
            return;
        }

        if (fadePanel == null)
        {
            Debug.LogError("[SceneLoader] Fade Panel is not assigned! Loading scene without fade.");
            SceneManager.LoadScene(nextSceneName);
            return;
        }

        StartCoroutine(FadeAndLoadScene());
    }

    private IEnumerator FadeAndLoadScene()
    {
        _isTransitioning = true;
        fadePanel.raycastTarget = true;

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);
            SetFadeAlpha(alpha);
            yield return null;
        }

        SetFadeAlpha(1f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName);

        if (asyncLoad == null)
        {
            Debug.LogError($"[SceneLoader] Could not load scene \"{nextSceneName}\". Is it added to Build Settings?");
            _isTransitioning = false;
            yield break;
        }

        yield return asyncLoad;
    }

    private void SetFadeAlpha(float alpha)
    {
        Color color = fadePanel.color;
        color.a = alpha;
        fadePanel.color = color;
    }
}
