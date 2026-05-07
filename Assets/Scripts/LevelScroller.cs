using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeLevelScroller : MonoBehaviour
{
    [Header("Scroll View")]
    public RectTransform contentTransform;

    [Header("Button Prefab")]
    public GameObject buttonPrefab;

    [Header("Scene Loader")]
    public SceneLoader sceneLoader;

    [Header("Recipe Levels")]
    public List<RecipeLevel> recipeLevels = new List<RecipeLevel>();

    private const string IMAGE_NAME = "RecipeImage";
    private const string TITLE_NAME = "RecipeTitle";
    private const string STAR_CONTAINER = "StarContainer";
    private const string STAR_PREFIX = "Star";

    private ScrollRect _scrollRect;

    private void Awake()
    {
        //_scrollRect = GetComponentInParent<ScrollRect>();
        //if (_scrollRect == null)
        //    _scrollRect = FindFirstObjectByType<ScrollRect>();

        //if (_scrollRect != null)
        //{
        //    _scrollRect.movementType = ScrollRect.MovementType.Clamped;
        //    _scrollRect.horizontal = false;
        //    _scrollRect.vertical = true;
        //}
    }

    private void Start()
    {
        if (!ValidateReferences())
            return;

        BuildRecipeButtons();
    }

    private void BuildRecipeButtons()
    {
        foreach (Transform child in contentTransform)
            Destroy(child.gameObject);

        foreach (RecipeLevel recipe in recipeLevels)
        {
            if (string.IsNullOrEmpty(recipe.sceneName))
            {
                Debug.LogWarning("[RecipeLevelScroller] A recipe entry has no scene name and will be skipped.");
                continue;
            }

            GameObject buttonObj = Instantiate(buttonPrefab, contentTransform);
            buttonObj.name = $"Recipe_{recipe.sceneName}";

            ApplyRecipeImage(buttonObj, recipe);
            ApplyRecipeTitle(buttonObj, recipe);
            ApplyDifficultyStars(buttonObj, recipe);
            WireButtonClick(buttonObj, recipe);
        }
    }

    private void ApplyRecipeImage(GameObject buttonObj, RecipeLevel recipe)
    {
        Transform imageTransform = buttonObj.transform.Find(IMAGE_NAME);
        if (imageTransform == null)
        {
            Debug.LogWarning($"[RecipeLevelScroller] Could not find child named '{IMAGE_NAME}' on prefab.");
            return;
        }

        Image image = imageTransform.GetComponent<Image>();
        if (image == null)
        {
            Debug.LogWarning($"[RecipeLevelScroller] '{IMAGE_NAME}' has no Image component.");
            return;
        }

        if (recipe.recipeImage != null)
        {
            image.sprite = recipe.recipeImage;
            image.preserveAspect = true;
        }
    }

    private void ApplyRecipeTitle(GameObject buttonObj, RecipeLevel recipe)
    {
        Transform titleTransform = buttonObj.transform.Find(TITLE_NAME);
        if (titleTransform == null)
        {
            Debug.LogWarning($"[RecipeLevelScroller] Could not find child named '{TITLE_NAME}' on prefab.");
            return;
        }

        TextMeshProUGUI label = titleTransform.GetComponent<TextMeshProUGUI>();
        if (label == null)
        {
            Debug.LogWarning($"[RecipeLevelScroller] '{TITLE_NAME}' has no TextMeshProUGUI component.");
            return;
        }

        label.text = string.IsNullOrEmpty(recipe.displayTitle) ? recipe.sceneName : recipe.displayTitle;
    }

    private void ApplyDifficultyStars(GameObject buttonObj, RecipeLevel recipe)
    {
        Transform starContainer = buttonObj.transform.Find(STAR_CONTAINER);
        if (starContainer == null)
        {
            Debug.LogWarning($"[RecipeLevelScroller] Could not find child named '{STAR_CONTAINER}' on prefab.");
            return;
        }

        int clampedDifficulty = Mathf.Clamp(recipe.difficulty, 1, 4);

        for (int i = 1; i <= 4; i++)
        {
            Transform starTransform = starContainer.Find($"{STAR_PREFIX}{i}");
            if (starTransform == null)
            {
                Debug.LogWarning($"[RecipeLevelScroller] Could not find '{STAR_PREFIX}{i}' inside '{STAR_CONTAINER}'.");
                continue;
            }

            starTransform.gameObject.SetActive(i <= clampedDifficulty);
        }
    }

    private void WireButtonClick(GameObject buttonObj, RecipeLevel recipe)
    {
        Button button = buttonObj.GetComponent<Button>();
        if (button == null)
        {
            Debug.LogWarning($"[RecipeLevelScroller] Prefab is missing a Button component.");
            return;
        }

        string capturedScene = recipe.sceneName;
        button.onClick.AddListener(() =>
        {
            sceneLoader.nextSceneName = capturedScene;
            sceneLoader.LoadNextScene();
        });
    }

    private bool ValidateReferences()
    {
        if (contentTransform == null)
        {
            Debug.LogError("[RecipeLevelScroller] Content Transform is not assigned!");
            return false;
        }
        if (buttonPrefab == null)
        {
            Debug.LogError("[RecipeLevelScroller] Button Prefab is not assigned!");
            return false;
        }
        if (sceneLoader == null)
        {
            Debug.LogError("[RecipeLevelScroller] Scene Loader is not assigned!");
            return false;
        }
        if (recipeLevels == null || recipeLevels.Count == 0)
        {
            Debug.LogWarning("[RecipeLevelScroller] Recipe Levels list is empty. No buttons will be created.");
            return false;
        }
        return true;
    }
}

[System.Serializable]
public class RecipeLevel
{
    [Tooltip("Exact scene name as registered in Build Settings")]
    public string sceneName;

    [Tooltip("The recipe name shown on the button")]
    public string displayTitle;

    [Tooltip("The recipe photo shown on the button")]
    public Sprite recipeImage;

    [Tooltip("Difficulty rating: 1 = easy, 2 = medium, 3 = hard, 4 = very hard")]
    [Range(1, 4)]
    public int difficulty = 1;
}