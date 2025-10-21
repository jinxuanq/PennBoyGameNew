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
            // Assign glass to the real drink object
            linkedDrink.AssignGlass(glass.glassType);

            // Callback to UI manager
            onAssignedGlassCallback?.Invoke(linkedDrink, glass.glassType);

            // Visual feedback: you could set an icon on this thumb or disable the glass button
            Debug.Log($"Dropped glass {glass.glassType} onto drink {linkedDrink.name}");
        }
        var ingredient = eventData.pointerDrag?.GetComponent<DraggableIngredients>();
        if (ingredient != null && linkedDrink != null)
        {
            // Assign glass to the real drink object
            linkedDrink.AssignGarnish(ingredient.ingredientType, ingredient.score);

            // Callback to UI manager
            onAssignedIngredientCallback?.Invoke(linkedDrink, ingredient);
        }
    }

    // optional: click to preview or focus the real drink
    public void OnPointerClick(PointerEventData eventData)
    {
        if (linkedDrink != null)
            Debug.Log($"Clicked thumbnail for drink: {linkedDrink.name}");
    }
}