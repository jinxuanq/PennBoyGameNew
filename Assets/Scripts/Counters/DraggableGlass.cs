using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;

public class DraggableGlass : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rt;
    private Canvas canvas;
    private CanvasGroup cg;
    private Vector3 startPos;
    private Transform startParent;

    public GlassType glassType;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        cg = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = rt.position;
        startParent = rt.parent;
        rt.SetParent(canvas.transform, true); // bring to top
        cg.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rt.position += (Vector3)eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // If not dropped on a valid drop target, reset
        cg.blocksRaycasts = true;
        if (!eventData.pointerEnter || eventData.pointerEnter.GetComponent<DrinkThumb>() == null)
        {
            ResetPosition();
        }
        else
        {
            // let the DrinkThumb handle the drop via IDropHandler or pointerEnter check
            // optionally destroy or snap to target
            ResetPosition();
        }
    }

    public void ResetPosition()
    {
        rt.position = startPos;
        rt.SetParent(startParent, true);
    }
}