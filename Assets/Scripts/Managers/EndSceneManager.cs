using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultsSceneDirector : MonoBehaviour
{
    [Header("Dish Reveals")]
    public List<CanvasGroup> dishGroups = new List<CanvasGroup>();

    [Header("Message Panel")]
    public CanvasGroup messageGroup;

    public TMPro.TextMeshProUGUI messageText;

    [TextArea]
    public string resultMessageText = "Well done!";

    [Header("Button Panel")]
    public CanvasGroup buttonGroup;

    public Button playMoreButton;

    public Button quitButton;

    [Header("Scene Loader")]
    public SceneLoader sceneLoader;

    public string levelSelectSceneName;

    [Header("Timings")]
    [Range(0f, 5f)]
    public float initialDelay = 2f;

    [Range(0f, 2f)]
    public float delayBetweenDishes = 0.4f;

    [Range(0f, 3f)]
    public float delayBeforeMessage = 0.8f;

    [Range(0f, 3f)]
    public float delayBeforeButtons = 1f;

    [Range(0.1f, 2f)]
    public float fadeDuration = 0.6f;

    // -------------------------------------------------------

    private void Start()
    {
        if (!ValidateReferences())
            return;

        InitialiseGroups();
        WireButtons();
        StartCoroutine(PlayRevealSequence());
    }

    private IEnumerator PlayRevealSequence()
    {
        yield return new WaitForSeconds(initialDelay);

        foreach (CanvasGroup dish in dishGroups)
        {
            if (dish != null)
            {
                yield return StartCoroutine(FadeIn(dish));
                yield return new WaitForSeconds(delayBetweenDishes);
            }
        }

        yield return new WaitForSeconds(delayBeforeMessage);

        if (messageText != null)
            messageText.text = resultMessageText;

        if (messageGroup != null)
            yield return StartCoroutine(FadeIn(messageGroup));

        yield return new WaitForSeconds(delayBeforeButtons);

        if (buttonGroup != null)
        {
            yield return StartCoroutine(FadeIn(buttonGroup));
            buttonGroup.interactable = true;
            buttonGroup.blocksRaycasts = true;
        }
    }

    private IEnumerator FadeIn(CanvasGroup group)
    {
        float elapsed = 0f;
        group.alpha = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            group.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        group.alpha = 1f;
    }

    private void WireButtons()
    {
        if (playMoreButton != null)
            playMoreButton.onClick.AddListener(OnPlayMorePressed);

        if (quitButton != null)
            quitButton.onClick.AddListener(OnQuitPressed);
    }

    private void OnPlayMorePressed()
    {
        if (string.IsNullOrEmpty(levelSelectSceneName))
        {
            Debug.LogWarning("[ResultsSceneDirector] Level Select Scene Name is not set.");
            return;
        }

        sceneLoader.nextSceneName = levelSelectSceneName;
        sceneLoader.LoadNextScene();
    }

    private void OnQuitPressed()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_IOS
        Debug.Log("[ResultsSceneDirector] Quit is not supported on iOS.");
#else
        Application.Quit();
#endif
    }

    private void InitialiseGroups()
    {
        foreach (CanvasGroup dish in dishGroups)
            SetGroupState(dish, alpha: 0f, interactive: false);

        SetGroupState(messageGroup, alpha: 0f, interactive: false);
        SetGroupState(buttonGroup, alpha: 0f, interactive: false);
    }

    private void SetGroupState(CanvasGroup group, float alpha, bool interactive)
    {
        if (group == null) return;
        group.alpha = alpha;
        group.interactable = interactive;
        group.blocksRaycasts = interactive;
    }

    private bool ValidateReferences()
    {
        if (sceneLoader == null)
        {
            Debug.LogError("[ResultsSceneDirector] Scene Loader is not assigned!");
            return false;
        }
        if (dishGroups.Count == 0)
            Debug.LogWarning("[ResultsSceneDirector] No dish groups assigned — dish reveal will be skipped.");
        if (messageGroup == null)
            Debug.LogWarning("[ResultsSceneDirector] Message Group not assigned — message reveal will be skipped.");
        if (buttonGroup == null)
            Debug.LogWarning("[ResultsSceneDirector] Button Group not assigned — buttons will not appear.");
        if (playMoreButton == null && quitButton == null)
            Debug.LogWarning("[ResultsSceneDirector] No buttons assigned — player will have no way to continue.");

        return true;
    }
}