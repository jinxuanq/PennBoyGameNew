using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RecipeList : MonoBehaviour
{
    // Start is called before the first frame update
    public RecipeDataBase recipeDatabase;
    public TextMeshProUGUI recipeListText;
    void OnEnable()
    {
        UpdateRecipeList();
    }

    void UpdateRecipeList()
    {
        string text = "<b>Recipes</b>\n";

        foreach (var recipe in recipeDatabase.allRecipes)
        {
            text += $"\n<b>{recipe.drinkName}</b>\n";

            foreach (var ing in recipe.requiredIngredients)
            {
                text += $"• {ing}\n";
            }
        }

        recipeListText.text = text;
    }
}
