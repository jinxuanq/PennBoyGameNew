using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableIngredients : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector3 startPos;

    public DrinkRecipe.Ingredient ingredientType; // assign in Inspector

    private CanvasGroup canvasGroup;

    private Transform originalParent; // remember the original parent for reset

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        startPos = rectTransform.position;

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Save original parent to reset later
        originalParent = rectTransform.parent;

        // Move to top-level canvas so layout doesn't interfere
        rectTransform.SetParent(canvas.transform, true);

        canvasGroup.blocksRaycasts = false; // allow drop detection
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position += (Vector3)eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Reset position if not dropped in a MixZone
        if (!MixZone.IsPointerOverDropZone(eventData))
        {
            ResetPosition();
        }

        canvasGroup.blocksRaycasts = true;
    }

    /// <summary>
    /// Resets the ingredient to its original position and parent
    /// </summary>
    public void ResetPosition()
    {
        rectTransform.position = startPos;
        rectTransform.SetParent(originalParent, true);
        canvasGroup.blocksRaycasts = true;
    }
}