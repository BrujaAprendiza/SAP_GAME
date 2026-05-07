using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] public List<int> IngredientOneCount = new List<int>();
    [SerializeField] public List<int> IngredientTwoCount = new List<int>();
    [SerializeField] public List<int> IngredientThreeCount = new List<int>();

    [SerializeField] private int ammountToWin = 3;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
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
        if (IngredientOneCount.Count+IngredientTwoCount.Count+IngredientThreeCount.Count == ammountToWin*3)
        {
            Win();
        }
    }

    private void Win()
    {
        print("You won!");
    }

}
