using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GlassChoosingUI : MonoBehaviour
{
    [Header("Prefabs / UI")]
    [SerializeField] private Transform glassListParent;       // content parent (vertical/horizontal layout)
    [SerializeField] private GameObject drinkThumbPrefab;     // prefab with DrinkThumb component
    [SerializeField] private Transform drinkThumbParent;      // where to instantiate drink thumbnails

    [Header("Runtime")]
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
        // Optionally lock input via your GameInput if you use one.
    }

    public void Close()
    {
        ClearSpawned();
        gameObject.SetActive(false);
    }

    private void PopulateDrinks(List<Drink> drinks)
    {
        ClearDrinkThumbs();
        foreach (var d in drinks)
        {
            if (d.HasGlassAssigned()) continue; // skip drinks already assigned
            var go = Instantiate(drinkThumbPrefab, drinkThumbParent, false);
            go.GetComponentInChildren<UnityEngine.UI.Image>().sprite = d.assignedDrink.drinkSprite;
            go.GetComponentInChildren<TextMeshProUGUI>().text = d.assignedDrink.drinkName;
            var thumb = go.GetComponent<DrinkThumb>();
            if (thumb != null) thumb.Init(d, OnGlassAssignedToDrink);
            spawnedDrinkThumbs.Add(go);
        }
    }

    private void OnGlassAssignedToDrink(Drink drink, GlassType glassType)
    {
        Debug.Log($"Assigned {glassType} to drink {drink.name}");
        // Remove the drink thumbnail from UI
        var thumbToRemove = spawnedDrinkThumbs.Find(t => t.GetComponent<DrinkThumb>().linkedDrink == drink);
        if (thumbToRemove != null)
        {
            spawnedDrinkThumbs.Remove(thumbToRemove);
            Destroy(thumbToRemove);
        }

        // Optionally auto-close if all drinks are assigned
        if (autoCloseOnSelect && spawnedDrinkThumbs.Count == 0)
        {
            MixingManager.instance.CloseGlassUI();
        }
    }
    private void ClearDrinkThumbs()
    {
        foreach (var t in spawnedDrinkThumbs) Destroy(t);
        spawnedDrinkThumbs.Clear();
    }

    private void ClearSpawned()
    {
        ClearDrinkThumbs();
    }
}