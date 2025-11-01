using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;

public class DrinkThumb : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    [SerializeField] private Image drinkImage; // optional: show sprite/thumbnail
    [SerializeField] private TextMeshProUGUI nameText;

    public Drink linkedDrink;
    private Action<Drink, GlassType> onAssignedGlassCallback;
    private Action<Drink, DraggableIngredients> onAssignedIngredientCallback;

    /// <summary>
    /// Initialize with a real Drink reference and a callback to notify when a glass is assigned.
    /// </summary>
    public void Init(Drink drink, Action<Drink, GlassType> assignedCallback)
    {
        linkedDrink = drink;
        onAssignedGlassCallback = assignedCallback;
        if (nameText != null)
            nameText.text = drink.name;

        // Optionally set drinkImage sprite from drink data if you have one
        // if (drink.thumbSprite != null && drinkImage) drinkImage.sprite = drink.thumbSprite;
    }
    public void Init(Drink drink, Action<Drink, DraggableIngredients> assignedCallback)
    {
        linkedDrink = drink;
        onAssignedIngredientCallback = assignedCallback;
        if (nameText != null)
            nameText.text = drink.name;
    }

    public void OnDrop(PointerEventData eventData)
    {
        var glass = eventData.pointerDrag?.GetComponent<DraggableGlass>();
        if (glass != null && linkedDrink != null)
        {
            linkedDrink.AssignGlass(glass.glassType);
            onAssignedGlassCallback?.Invoke(linkedDrink, glass.glassType);
            Debug.Log($"Dropped glass {glass.glassType} onto drink {linkedDrink.name}");
            return; //Stop here ¡ª it was a glass
        }

        // ========== INGREDIENT / DRUG DROP ========== 
        var ingredient = eventData.pointerDrag?.GetComponent<DraggableIngredients>();
        if (ingredient != null && linkedDrink != null)
        {
            // Check if ingredient should be treated as a drug
            if (IsDrug(ingredient.ingredientType))
            {
                // Drug path
                linkedDrink.AssignDrug(ConvertToDrugType(ingredient.ingredientType));
                onAssignedIngredientCallback?.Invoke(linkedDrink, ingredient);
                Debug.Log($"Dropped drug {ingredient.ingredientType} onto drink {linkedDrink.name}");
            }
            else
            {
                // Regular garnish path
                linkedDrink.AssignGarnish(ingredient.ingredientType, ingredient.score);
                onAssignedIngredientCallback?.Invoke(linkedDrink, ingredient);
                Debug.Log($"Dropped garnish {ingredient.ingredientType} onto drink {linkedDrink.name}");
            }
        }
    }

    // optional: click to preview or focus the real drink
    public void OnPointerClick(PointerEventData eventData)
    {
        if (linkedDrink != null)
            Debug.Log($"Clicked thumbnail for drink: {linkedDrink.name}");
    }

    private bool IsDrug(DrinkRecipe.Ingredient ing)
    {
        return ing == DrinkRecipe.Ingredient.Flower
            || ing == DrinkRecipe.Ingredient.Leaf
            || ing == DrinkRecipe.Ingredient.Powder
            || ing == DrinkRecipe.Ingredient.Crystal;
    }

    private DrugType ConvertToDrugType(DrinkRecipe.Ingredient ing)
    {
        switch (ing)
        {
            case DrinkRecipe.Ingredient.Flower: return DrugType.Flower;
            case DrinkRecipe.Ingredient.Leaf: return DrugType.Leaf;
            case DrinkRecipe.Ingredient.Powder: return DrugType.Powder;
            case DrinkRecipe.Ingredient.Crystal: return DrugType.Crystal;
            default: return DrugType.Powder;
        }
    }
}