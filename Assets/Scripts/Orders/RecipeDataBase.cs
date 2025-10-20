using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipeDatabase", menuName = "BarGame/Recipe Database")]
public class RecipeDataBase : ScriptableObject
{
    public List<DrinkRecipe> allRecipes;

    public DrinkRecipe FindExactMatch(List<DrinkRecipe.Ingredient> attempt)
    {
        foreach (DrinkRecipe recipe in allRecipes)
        {
            if (recipe.requiredIngredients.Count != attempt.Count)
                continue;

            bool match = true;
            for (int i = 0; i < attempt.Count; i++)
            {
                if (!attempt.Contains(recipe.requiredIngredients[i]))
                {
                    match = false;
                    break;
                }
                if (!recipe.requiredIngredients.Contains(attempt[i]))
                {
                    match = false; break;
                }
            }

            if (match) return recipe;
        }

        return null; // no match
    }
}