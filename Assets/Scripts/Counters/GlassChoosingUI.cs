using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GlassChoosingUI : MonoBehaviour
{
    [Header("Prefabs / UI")]
    [SerializeField] private GameObject glassButtonPrefab;    // prefab with DraggableGlass component
    [SerializeField] private Transform glassListParent;       // content parent (vertical/horizontal layout)
    [SerializeField] private GameObject drinkThumbPrefab;     // prefab with DrinkThumb component
    [SerializeField] private Transform drinkThumbParent;      // where to instantiate drink thumbnails

    [Header("Runtime")]
    [SerializeField] private bool autoCloseOnSelect = false;

    private List<GameObject> spawnedGlassButtons = new List<GameObject>();
    private List<GameObject> spawnedDrinkThumbs = new List<GameObject>();

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Open(List<Drink> drinks, List<GlassType> availableGlasses)
    {
        PopulateGlasses(availableGlasses);
        PopulateDrinks(drinks);
        gameObject.SetActive(true);
        // Optionally lock input via your GameInput if you use one.
    }

    public void Close()
    {
        ClearSpawned();
        gameObject.SetActive(false);
    }

    private void PopulateGlasses(List<GlassType> glasses)
    {
        ClearGlassButtons();
        Debug.Log("Glasses to spawn: " + string.Join(", ", glasses));
        foreach (var g in glasses)
        {
            var go = Instantiate(glassButtonPrefab, glassListParent, false);
            var dg = go.GetComponent<DraggableGlass>();
            if (dg != null) dg.Init(g);
            spawnedGlassButtons.Add(go);
        }
    }

    private void PopulateDrinks(List<Drink> drinks)
    {
        ClearDrinkThumbs();
        foreach (var d in drinks)
        {
            if (d.HasGlassAssigned()) continue; // skip drinks already assigned
            var go = Instantiate(drinkThumbPrefab, drinkThumbParent, false);
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
            Close();
        }
    }

    private void ClearGlassButtons()
    {
        foreach (var g in spawnedGlassButtons) Destroy(g);
        spawnedGlassButtons.Clear();
    }

    private void ClearDrinkThumbs()
    {
        foreach (var t in spawnedDrinkThumbs) Destroy(t);
        spawnedDrinkThumbs.Clear();
    }

    private void ClearSpawned()
    {
        ClearGlassButtons();
        ClearDrinkThumbs();
    }
}