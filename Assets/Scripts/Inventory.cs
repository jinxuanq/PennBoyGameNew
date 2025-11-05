using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    [SerializeField] private GameObject drinkThumbPrefab;     // prefab with DrinkThumb component
    [SerializeField] private Transform drinkThumbParent;      // where to instantiate drink thumbnails
    private List<GameObject> inventoryDrinkList = new List<GameObject>();

    private int selectedIndex = -1;

    public Drink currDrink;

    private void Awake()
    {
        if (instance) Destroy(gameObject);
        else instance = this;
    }

    private void Update()
    {
        if (MixingManager.instance.UIOpen)
        {
            foreach (var u in inventoryDrinkList)
            {
                u.SetActive(false);
            }
        }
        else
        {
            foreach (var u in inventoryDrinkList)
            {
                u.SetActive(true);
            }
        }
    }

    public void PopulateDrinks()
    {

        List<Drink> drinks = MixingManager.instance.createdDrinks;


        for (int i = 0; i < drinkThumbParent.childCount; i++)
        {
            Destroy(drinkThumbParent.GetChild(i).gameObject);
        }
        foreach (var d in drinks)
        {
            if (!d.Finished()) continue; // skip drinks without a glass
            var go = Instantiate(drinkThumbPrefab, drinkThumbParent, false);
            go.GetComponentInChildren<TextMeshProUGUI>().text = d.assignedDrink.drinkName;
            go.GetComponentInChildren<UnityEngine.UI.Image>().sprite = d.assignedDrink.drinkSprite;
            go.GetComponentInChildren<DrinkThumb>().linkedDrink = d;
            inventoryDrinkList.Add(go);
        }

        Debug.Log("populated inventory");
    }
}
