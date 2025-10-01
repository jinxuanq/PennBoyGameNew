using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DrinkRecipe : MonoBehaviour
{
    public string drinkName;
    public GameObject dishPrefab;
    public List<Ingredient> requiredIngredients;

    public enum Ingredient
    {
        Vodka,
        Mint,
        Syrup,
        Ice
    }
}
