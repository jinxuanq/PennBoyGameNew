using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SummonManager : MonoBehaviour
{
    public static SummonManager instance { get; private set; }
    [SerializeField] public List<DrinkRecipe> allDrinks;

    private void Awake()
    {
        if (instance) Destroy(this.gameObject);
        else instance = this;

        foreach (var drink in allDrinks)
        {
            Debug.Log(drink.drinkName);
        }
    }

    public GameObject CheckSummoning(List<DrinkRecipe.Ingredient> items)
    {
        Debug.Log("Prepare for checking");
        foreach (DrinkRecipe recipe in allDrinks)
        {
            Debug.Log(recipe.drinkName);
            if (IsMatch(recipe.requiredIngredients, items))
                return recipe.drinkPrefab;
        }
        return null;
    }

    private bool IsMatch(List<DrinkRecipe.Ingredient> requiredItems, List<DrinkRecipe.Ingredient> items)
    {
        foreach (DrinkRecipe.Ingredient item in requiredItems)
        {
            Debug.Log("Current item in the recipe: " + item);
            if (!ContainsItem(item, requiredItems))
            {
                return false;
            }
            Debug.Log("Table contains " + item);
        }
        return true;
    }

    private bool ContainsItem(DrinkRecipe.Ingredient key, List<DrinkRecipe.Ingredient> list)
    {
        int i = 0;
        foreach (DrinkRecipe.Ingredient item in list)
        {
            if (item == key)
            {
                i++;
            }
        }
        return (i > 0);
    }
}
