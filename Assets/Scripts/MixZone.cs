using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class MixZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        DraggableIngredients ingredient = eventData.pointerDrag.GetComponent<DraggableIngredients>();
        if (ingredient != null)
        {
            MixingManager.instance.AddIngredient(ingredient.ingredientType);

            // Optional: hide or reset the UI image
            ingredient.transform.position = ingredient.GetComponent<DraggableIngredients>().transform.parent.position;
            ingredient.ResetPosition();
        }
    }

    // Optional visual feedback
    public void OnPointerEnter(PointerEventData eventData) { /* highlight zone */ }
    public void OnPointerExit(PointerEventData eventData) { /* remove highlight */ }

    // Utility method for OnEndDrag
    public static bool IsPointerOverDropZone(PointerEventData eventData)
    {
        return eventData.pointerEnter != null && eventData.pointerEnter.GetComponent<MixZone>() != null;
    }
}
