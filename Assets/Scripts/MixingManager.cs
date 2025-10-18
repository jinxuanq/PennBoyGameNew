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

    [Header("UI References")]
    public GlassChoosingUI glassChoosingUI;  // drag the Canvas object here

    [Header("Data")]
    public List<GlassType> availableGlasses;  // assign in Inspector or populate dynamically

    private List<Drink> createdDrinks = new List<Drink>(); // store all drinks created by Mix()

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

    public void OpenGlassChoosingUI(List<Drink> drinks, List<GlassType> glasses)
    {
        Debug.Log($"OpenGlassChoosingUI called. Drinks count: {drinks.Count}, Glasses count: {glasses.Count}");

        if (glassChoosingUI != null)
        {
            glassChoosingUI.Open(drinks, glasses);
            Debug.Log("GlassChoosingUI opened");
        }
        else
        {
            Debug.LogWarning("GlassChoosingUI reference not set in MixingManager!");
        }
    }

    public void AddIngredient(DrinkRecipe.Ingredient ingredient)
    {
        if (!isMixing) return;
        currentMix.Add(ingredient);
        Debug.Log($"Added ingredient: {ingredient}");
    }

    public void Mix()
    {
        if (currentMix.Count == 0)
        {
            Debug.LogWarning("Mix() called but currentMix is empty!");
            return;
        }

        DrinkRecipe matchedRecipe = recipeDatabase.FindExactMatch(currentMix);
        GameObject drinkGO;

        if (matchedRecipe != null)
        {
            Debug.Log("Created drink: " + matchedRecipe.drinkName);
            drinkGO = Instantiate(matchedRecipe.drinkPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("Created random drink!");
            drinkGO = Instantiate(randomDrinkPrefab, transform.position, Quaternion.identity);
        }

        Drink drinkComponent = drinkGO.GetComponent<Drink>();
        if (drinkComponent != null)
        {
            createdDrinks.Add(drinkComponent);
            Debug.Log($"Drink tracked: {drinkComponent.name}, total drinks: {createdDrinks.Count}");
        }
        else
        {
            Debug.LogWarning("Spawned drink prefab has no Drink component!");
        }

        currentMix.Clear(); // reset ingredients
        Debug.Log("Mix complete, UI still open");
    }

    /// <summary>
    /// Returns a list of drinks currently available for glass assignment
    /// </summary>
    public List<Drink> GetAvailableDrinks()
    {
        List<Drink> availableDrinks = createdDrinks.FindAll(d => !d.HasGlassAssigned());
        Debug.Log($"GetAvailableDrinks called. Returning {availableDrinks.Count} drinks.");
        return availableDrinks;
    }

    /// <summary>
    /// Returns a list of all available glass types
    /// </summary>
    public List<GlassType> GetAvailableGlasses()
    {
        Debug.Log($"GetAvailableGlasses called. Returning {availableGlasses.Count} glasses.");
        return availableGlasses;
    }
}