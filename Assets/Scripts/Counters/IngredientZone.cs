using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class IngredientZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject ing;
    private DraggableIngredients ingredient;

    //Oscillate
    [Header("Points")]
    public Transform pointA;
    public Transform pointB;
    public Transform center;

    [Header("Speed Range")]
    public float minSpeed = 10f;
    public float maxSpeed = 20f;

    [Header("Optional Pause at Ends")]
    public float pauseTime = 0.2f;
    private bool oscillating = false;
    private float grade;


    [SerializeField] private GameObject drinkThumbPrefab;     // prefab with DrinkThumb component
    [SerializeField] private Transform drinkThumbParent;      // where to instantiate drink thumbnails
    private List<GameObject> spawnedDrinkThumbs = new List<GameObject>();

    public void OnDrop(PointerEventData eventData)
    {
        ingredient = eventData.pointerDrag.GetComponent<DraggableIngredients>();
        if (ingredient != null)
        {
            Debug.Log(ingredient.ingPrefab != null);
            ing = Instantiate(ingredient.ingPrefab, center);
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
            while (Vector3.Distance(ing.transform.position, target.position) > 0.001f &&oscillating)
            {
                ing.transform.position = Vector3.MoveTowards(
                    ing.transform.position,
                    target.position,
                    speed * Time.deltaTime
                );
                yield return null;
            }

            if (oscillating)
            {
                // Snap exactly to target (avoid float drift)
                ing.transform.position = target.position;

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
        float g = Mathf.Abs((Mathf.Abs(ing.transform.position.x - pointA.position.x) - avg)) / avg*100;

        Debug.Log(ingredient.ingredientType);
        grade = g;
        Debug.Log(grade);
    }


    public void PopulateDrinks(List<Drink> drinks)
    {
        for(int i = 0; i < drinkThumbParent.childCount; i++)
        {
            Destroy(drinkThumbParent.GetChild(i).gameObject);
        }
        foreach (var d in drinks)
        {
            if (!d.HasGlassAssigned()) continue; // skip drinks without a glass
            var go = Instantiate(drinkThumbPrefab, drinkThumbParent, false);
            var thumb = go.GetComponent<DrinkThumb>();
            if (thumb != null) thumb.Init(d, OnIngredientAssignedToDrink, grade);
            spawnedDrinkThumbs.Add(go);
        }
    }
    private void OnIngredientAssignedToDrink(Drink drink, DrinkRecipe.Ingredient ingrdientType)
    {
        Debug.Log($"Assigned {ingrdientType} to drink {drink.name}");
        // Remove the drink thumbnail from UI
        var thumbToRemove = spawnedDrinkThumbs.Find(t => t.GetComponent<DrinkThumb>().linkedDrink == drink);
        if (thumbToRemove != null)
        {
            spawnedDrinkThumbs.Remove(thumbToRemove);
            Destroy(thumbToRemove);
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
