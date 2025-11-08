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

    [Header("UI References")]
    public GameObject glassChoosingUI;  // drag the Canvas object here
    public GameObject pourmixingUI;  // assign your mixing canvas here
    public GameObject mixGameUI;
    public GameObject ingredientUI;
    public GameObject drugUI;

    public DrugChoosingUI drugChoosingUIScript;

    [Header("Data")]
    public List<GlassType> availableGlasses;  // assign in Inspector or populate dynamically

    public List<Drink> createdDrinks = new List<Drink>(); // store all drinks created by Mix()

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
        pourmixingUI.SetActive(true);
        Debug.Log("Mixing UI opened");

        Inventory.instance.SetUI(false);
    }

    public void CloseMixingUI()
    {
        GameInput.instance.LockInput(false);

        isMixing = false;
        pourmixingUI.SetActive(false);
        mixGameUI.SetActive(false);
        currentMix.Clear();
        Debug.Log("Mixing UI closed");

        Inventory.instance.SetUI(true);
    }

    public void OpenGlassChoosingUI(List<Drink> drinks)
    {
        GameInput.instance.LockInput(true);
        glassChoosingUI.GetComponent<GlassChoosingUI>().Open(drinks);
        Debug.Log("GlassChoosingUI opened");
        glassChoosingUI.SetActive(true);

        Inventory.instance.SetUI(false);

    }
    public void CloseGlassUI()
    {
        GameInput.instance.LockInput(false);
        glassChoosingUI.GetComponent<GlassChoosingUI>().Close();
        Debug.Log("GlassChoosingUI closed");

        glassChoosingUI.SetActive(false);
        Inventory.instance.SetUI(true);

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
            drinkGO = Instantiate(matchedRecipe.drinkPrefab, new Vector3 (2000,2000,2000), Quaternion.identity);
        }
        else
        {
            Debug.Log("Created random drink!");
            drinkGO = Instantiate(randomDrinkPrefab, transform.position, Quaternion.identity);
        }

        Drink drinkComponent = drinkGO.GetComponent<Drink>();
        if (drinkComponent != null)
        {
            if (matchedRecipe != null)
            {
                drinkComponent.AssignDrink(matchedRecipe); 
            }
            createdDrinks.Add(drinkComponent);
            Debug.Log($"Drink tracked: {drinkComponent.name}, total drinks: {createdDrinks.Count}");

            drinkComponent.PrintStatus();
        }
        else
        {
            Debug.LogWarning("Spawned drink prefab has no Drink component!");
        }

        currentMix.Clear(); // reset ingredients
        Debug.Log("Mix complete, UI still open");

        mixGameUI.SetActive(true);
        mixGameUI.GetComponent<MixGame>().Begin(drinkComponent);
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
    public List<Drink> GetAvailableDrinksWithGlass()
    {
        List<Drink> availableDrinks = createdDrinks.FindAll(d => d.HasGlassAssigned());
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
    
    public void OpenIngredientsUI()
    {
        GameInput.instance.LockInput(true);

        ingredientUI.SetActive(true);
        ingredientUI.GetComponentInChildren<IngredientZone>().PopulateDrinks(GetAvailableDrinksWithGlass());
        Inventory.instance.SetUI(false);

    }
    public void CloseIngredientsUI()
    {
        GameInput.instance.LockInput(false);

        ingredientUI.SetActive(false);
        Inventory.instance.SetUI(true);
    }

    public void OpenDrugsUI()
    {
        GameInput.instance.LockInput(true);

        var drinks = GetAvailableDrinksWithGlass();
        drugChoosingUIScript.Open(drinks);
        Debug.Log("DrugChoosingUI opened");
        drugUI.SetActive(true); 
        Inventory.instance.SetUI(false);
    }
    public void CloseDrugsUI()
    {
        GameInput.instance.LockInput(false);

        if (drugChoosingUIScript != null)
            drugChoosingUIScript.Close();

        drugUI.SetActive(false);

        Inventory.instance.SetUI(true);
    }

}


