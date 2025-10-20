using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlasswareCounter : MonoBehaviour
{
    public enum CounterType
    {
        Glassware,
        Pour,
        Blender,
        Ingredients,
        Drugs// add more types later if needed
    }
    [SerializeField] private SelectedGlasswareCounter_Visual selectedGlasswareCounter_Visual;

    [Header("Counter Settings")]
    public CounterType counterType; // assign in Inspector
    public void Interact()
    {
        switch (counterType)
        {
            case CounterType.Glassware:
                HandleGlassware();
                break;

            case CounterType.Pour:
                HandlePour();
                break;

            case CounterType.Blender:
                HandleBlend();
                break;
            case CounterType.Ingredients:
                HandleIngredient();
                break;
            case CounterType.Drugs:
                HandleDrug();
                break;
        }
    }

    private void HandleGlassware()
    {
        Debug.Log("Glassware Interact");
        List<Drink> drinks = MixingManager.instance.GetAvailableDrinks();
        List<GlassType> glasses = MixingManager.instance.GetAvailableGlasses();
        Debug.Log("Drinks count: " + drinks.Count);
        Debug.Log("Glasses count: " + glasses.Count);
        MixingManager.instance.OpenGlassChoosingUI(drinks, glasses);
    }

    private void HandlePour()
    {
        Debug.Log("Pour Counter Interact");
        //MixingManager.instance.OpenMixingUI();
    }

    private void HandleBlend()
    {
        Debug.Log("Blend Counter Interact");
        //MixingManager.instance.OpenMixingUI();
    }

    private void HandleIngredient()
    {
        Debug.Log("Ingredient Counter Interact");
        MixingManager.instance.OpenIngredientsUI();
    }
    private void HandleDrug()
    {
        Debug.Log("Drug Counter Interact");
    }

    public void GlasswareHighlight(bool highlighted)
    {
        selectedGlasswareCounter_Visual.Highlight(highlighted);
    }

}
