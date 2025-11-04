using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DrugChoosingUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject drinkThumbPrefab;
    [SerializeField] private Transform drinkThumbParent;

    [Header("Settings")]
    [SerializeField] private bool autoCloseOnSelect = false;

    private List<GameObject> spawnedDrinkThumbs = new List<GameObject>();

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Open(List<Drink> drinks)
    {
        PopulateDrinks(drinks);
        gameObject.SetActive(true);
    }

    public void Close()
    {
        ClearThumbs();
        gameObject.SetActive(false);
    }

    private void PopulateDrinks(List<Drink> drinks)
    {
        ClearThumbs();

        foreach (var d in drinks)
        {
            if (d.HasDrugAssigned()) continue;

            var go = Instantiate(drinkThumbPrefab, drinkThumbParent, false);
            var thumb = go.GetComponent<DrinkThumb>();

            go.GetComponentInChildren<Image>().sprite = d.assignedDrink.drinkSprite;
            go.GetComponentInChildren<TextMeshProUGUI>().text = d.assignedDrink.drinkName;

            // Initialize for ingredient assignment callback
            if (thumb != null)
                thumb.Init(d, OnDrugAssignedCallback);

            spawnedDrinkThumbs.Add(go);
        }
    }

    private void OnDrugAssignedCallback(Drink drink, DraggableIngredients ingredient)
    {
        Debug.Log($"Assigned DRUG {ingredient.ingredientType} to drink {drink.name}");

        // Hide the drink slot after applying drug (like glass)
        var thumbToRemove = spawnedDrinkThumbs.Find(t => t.GetComponent<DrinkThumb>().linkedDrink == drink);
        if (thumbToRemove != null)
        {
            spawnedDrinkThumbs.Remove(thumbToRemove);
            Destroy(thumbToRemove);
        }

//        ingredient.gameObject.SetActive(false);

        if (autoCloseOnSelect && spawnedDrinkThumbs.Count == 0)
            Close();
    }

    private void ClearThumbs()
    {
        foreach (var t in spawnedDrinkThumbs)
            Destroy(t);

        spawnedDrinkThumbs.Clear();
    }
}