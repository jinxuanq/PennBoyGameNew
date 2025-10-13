using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MixingManager : MonoBehaviour
{
    public static MixingManager instance;

    [Header("References")]
    public RecipeDataBase recipeDatabase;
    public GameObject randomDrinkPrefab;
    public GameObject mixingUI;  // assign your mixing canvas here

    private List<DrinkRecipe.Ingredient> currentMix = new List<DrinkRecipe.Ingredient>();
    private bool isMixing = false;

    private void Awake()
    {
        if (instance) Destroy(gameObject);
        else instance = this;
    }

    public void OpenMixingUI()
    {
        GameInput.instance.LockInput(true);

        if (isMixing) return;
        isMixing = true;
        currentMix.Clear();
        mixingUI.SetActive(true);
        Debug.Log("Mixing UI opened");
    }

    public void CloseMixingUI()
    {
        GameInput.instance.LockInput(false);

        isMixing = false;
        mixingUI.SetActive(false);
        currentMix.Clear();
        Debug.Log("Mixing UI closed");
    }

    public void AddIngredient(DrinkRecipe.Ingredient ingredient)
    {
        if (!isMixing) return;
        currentMix.Add(ingredient);
        Debug.Log($"Added {ingredient}");
    }

    public void Mix()
    {
        if (currentMix.Count == 0) return;

        DrinkRecipe matchedRecipe = recipeDatabase.FindExactMatch(currentMix);

        if (matchedRecipe != null)
        {
            Debug.Log("Created: " + matchedRecipe.drinkName);
            Instantiate(matchedRecipe.drinkPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("Created random drink!");
            Instantiate(randomDrinkPrefab, transform.position, Quaternion.identity);
        }
        currentMix.Clear(); // reset ingredients
        Debug.Log("Mix complete, UI still open");

    }
}
