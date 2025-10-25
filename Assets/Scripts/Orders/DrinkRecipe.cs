using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDrinkRecipe", menuName = "BarGame/Drink Recipe")]
public class DrinkRecipe : ScriptableObject
{
    public string drinkName;
    public GameObject drinkPrefab;

    public List<Ingredient> requiredIngredients = new List<Ingredient>();

    public enum Ingredient
    {
        Tequila,
        Vodka,
        Gin,
        Rum,
        Lemon,
        Lime,
        Orange,
        Mint
    }
}
