using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // The cleanest approach is to ask every SlotController in the scene if it's full, 
    // rather than counting in GameManager. 
    // That way it doesn't matter how many slots we place in the scene.

    public static GameManager Instance;

    [SerializeField] public List<int> IngredientOneCount = new List<int>();
    [SerializeField] public List<int> IngredientTwoCount = new List<int>();
    [SerializeField] public List<int> IngredientThreeCount = new List<int>();
    //[SerializeField] private int ammountToWin;

    [Header("Scene Transition")]
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private string nextSceneName;

    // All SlotControllers in the scene, found automatically at start
    private SlotController[] _allSlots;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            IngredientOneCount.Clear();
            IngredientTwoCount.Clear();
            IngredientThreeCount.Clear();
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        // Find every slot in the scene once at load time
        _allSlots = FindObjectsByType<SlotController>(FindObjectsSortMode.None);

        if (_allSlots.Length == 0)
            Debug.LogWarning("[GameManager] No SlotControllers found in the scene.");
    }

    public void AddToSlot(int ingredientNumber)
    {
        if (ingredientNumber == 1)
        {
            IngredientOneCount.Add(ingredientNumber);
        } else if (ingredientNumber == 2)
        {
            IngredientTwoCount.Add(ingredientNumber);

        }
        else if (ingredientNumber == 3)
        {
            IngredientThreeCount.Add(ingredientNumber);
        }

        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        //if (IngredientOneCount.Count+IngredientTwoCount.Count+IngredientThreeCount.Count >= ammountToWin*3)
        //{
        //    Win();
        //}

        foreach (SlotController slot in _allSlots)
        {
            if (slot.ingredientAmmountInSlot < slot.slotCapacity)
                return; // At least one slot still has room — not done yet
        }

        Win();
    }

    private void Win()
    {
        print("You won!");

        if (sceneLoader == null)
        {
            Debug.LogWarning("[GameManager] SceneLoader is not assigned — cannot transition scene.");
            return;
        }

        sceneLoader.nextSceneName = nextSceneName;
        sceneLoader.LoadNextScene();
    }

}
