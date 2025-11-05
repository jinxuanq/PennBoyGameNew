using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class IngredientZone : MonoBehaviour, IDropHandler
{
    private GameObject ing;

    //Oscillate
    [Header("Points")]
    public RectTransform pointA;
    public RectTransform pointB;
    public RectTransform center;
    RectTransform currRect;

    [Header("Speed Range")]
    public float minSpeed = 100f;
    public float maxSpeed = 120f;

    [Header("Optional Pause at Ends")]
    public float pauseTime = 0.2f;
    private bool oscillating = false;


    [SerializeField] private GameObject drinkThumbPrefab;     // prefab with DrinkThumb component
    [SerializeField] private Transform drinkThumbParent;      // where to instantiate drink thumbnails
    private List<GameObject> spawnedDrinkThumbs = new List<GameObject>();

    public void OnDrop(PointerEventData eventData)
    {
        DraggableIngredients ingredient = eventData.pointerDrag.GetComponent<DraggableIngredients>();
        if (ingredient != null)
        {
            Debug.Log(ingredient.ingPrefab != null);
            ing = Instantiate(ingredient.ingPrefab, center.parent);
            currRect = ing.GetComponent<RectTransform>();
            currRect.position = center.position;
            StartCoroutine(Oscillate());
            Debug.Log("Oscillate");


            // Optional: hide or reset the UI image
            ingredient.transform.position = ingredient.GetComponent<DraggableIngredients>().transform.parent.position;
            ingredient.ResetPosition();
        }
    }


    private IEnumerator Oscillate()
    {
        Transform target = pointB;
        oscillating = true;

        while (oscillating)
        {
            float speed = Random.Range(minSpeed, maxSpeed);

            // Move toward target
            while (Vector3.Distance(currRect.position, target.position) > 0.001f && oscillating)
            {
                currRect.position = Vector3.MoveTowards(
                    currRect.position,
                    target.position,
                    speed * Time.deltaTime
                );
                yield return null;
            }

            if (oscillating)
            {
                // Snap exactly to target (avoid float drift)
                currRect.position = target.position;

                // Optional pause at ends
                if (pauseTime > 0f)
                    yield return new WaitForSeconds(pauseTime);

                // Switch target
                target = (target == pointA) ? pointB : pointA;
            }
            
        }
    }

    public void Chop()
    {
        oscillating = false;
        float avg = Mathf.Abs(pointA.position.x - pointB.position.x) / 2;
        float g = 100-(Mathf.Abs((Mathf.Abs(currRect.position.x - pointA.position.x) - avg)) / avg*100);

        Debug.Log(ing.GetComponent<DraggableIngredients>().ingredientType + ", score: " + (int)g);
        ing.GetComponent<DraggableIngredients>().score = (int) g;
    }


    public void PopulateDrinks(List<Drink> drinks)
    {
        for(int i = 0; i < drinkThumbParent.childCount; i++)
        {
            Destroy(drinkThumbParent.GetChild(i).gameObject);
        }
        foreach (var d in drinks)
        {
            if (d.HasIngAssigned()) continue; // skip drinks without a ing
            var go = Instantiate(drinkThumbPrefab, drinkThumbParent, false);
            go.GetComponentInChildren<UnityEngine.UI.Image>().sprite = d.assignedDrink.drinkSprite;
            go.GetComponentInChildren<TextMeshProUGUI>().text = d.assignedDrink.drinkName;
            var thumb = go.GetComponent<DrinkThumb>();
            if (thumb != null) thumb.Init(d, OnIngredientAssignedToDrink);
            spawnedDrinkThumbs.Add(go);
        }
    }
    private void OnIngredientAssignedToDrink(Drink drink, DraggableIngredients ing)
    {
        Debug.Log($"Assigned {ing.ingredientType} with score {ing.score} to drink {drink.name}");
        // Remove the drink thumbnail from UI
        var thumbToRemove = spawnedDrinkThumbs.Find(t => t.GetComponent<DrinkThumb>().linkedDrink == drink);
        if (thumbToRemove != null)
        {
            spawnedDrinkThumbs.Remove(thumbToRemove);
            Destroy(thumbToRemove);
        }
    }

    // Utility method for OnEndDrag
    public static bool IsPointerOverDropZone(PointerEventData eventData)
    {
        return eventData.pointerEnter != null && eventData.pointerEnter.GetComponent<MixZone>() != null;
    }
}
