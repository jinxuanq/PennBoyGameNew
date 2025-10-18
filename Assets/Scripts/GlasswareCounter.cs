using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlasswareCounter : MonoBehaviour
{
    public enum CounterType
    {
        Glassware,
        Pour,
        Other // add more types later if needed
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

            case CounterType.Other:
                Debug.Log("Other counter interacted");
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
        MixingManager.instance.OpenMixingUI();
    }

    public void GlasswareHighlight(bool highlighted)
    {
        selectedGlasswareCounter_Visual.Highlight(highlighted);
    }

}
