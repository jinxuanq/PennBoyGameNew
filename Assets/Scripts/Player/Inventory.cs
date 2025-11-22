using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    [SerializeField] private GameObject drinkThumbPrefab;     // prefab with DrinkThumb component
    [SerializeField] private GameObject drinkThumbParent;      // where to instantiate drink thumbnails
    private List<GameObject> inventoryDrinkList = new List<GameObject>();

    private int selectedIndex = -1;

    private void Awake()
    {
        if (instance) Destroy(gameObject);
        else instance = this;
    }

    public void SetUI(bool b)
    {
        drinkThumbParent.GetComponent<UnityEngine.UI.Image>().enabled = b;
        for (int i = 0; i < drinkThumbParent.transform.childCount; i++)
        {
            drinkThumbParent.transform.GetChild(i).gameObject.SetActive(b);
        }
    }

    public void PopulateDrinks()
    {

        List<Drink> drinks = MixingManager.instance.createdDrinks;


        for (int i = 0; i < drinkThumbParent.transform.childCount; i++)
        {
            inventoryDrinkList.Clear();
            Destroy(drinkThumbParent.transform.GetChild(i).gameObject);
        }
        foreach (var d in drinks)
        {
            if (!d.Finished()) continue; // skip drinks without a glass
            var go = Instantiate(drinkThumbPrefab, drinkThumbParent.transform, false);
            go.GetComponentInChildren<TextMeshProUGUI>().text = d.assignedDrink.drinkName;
            go.GetComponentInChildren<UnityEngine.UI.Image>().sprite = d.assignedDrink.drinkSprite;
            go.GetComponentInChildren<DrinkThumb>().linkedDrink = d;
            inventoryDrinkList.Add(go);
        }
        SetUI(false);

        Debug.Log("populated inventory");
    }

    public void SelectIndex(int index)
    {
        if(inventoryDrinkList.Count == 0)
        {
            selectedIndex = -1;
            return;
        }
        if (inventoryDrinkList.Count > index-1)
        {
            for (int i = 0; i < drinkThumbParent.transform.childCount; i++)
            {
                drinkThumbParent.transform.GetChild(i).gameObject.GetComponent<UnityEngine.UI.Image>().color = Color.white;
            }
            selectedIndex = index-1;
            inventoryDrinkList[selectedIndex].gameObject.GetComponent<UnityEngine.UI.Image>().color = Color.yellow;
        }

    }

    public void ServeCustomer()
    {
        Debug.Log("serve attemtped at index " + selectedIndex);
        if (selectedIndex==-1)
        {
            return;
        }
        GameObject go = inventoryDrinkList[selectedIndex];

        PlayerInteract.instance.currentCustomer.CompleteOrder(go.GetComponentInChildren<DrinkThumb>().linkedDrink);
        inventoryDrinkList.RemoveAt(selectedIndex);
        MixingManager.instance.createdDrinks.RemoveAt(selectedIndex);
        Destroy(go);
        selectedIndex = -1;
    }
    public int GetSelectedIndex()
    {
        return selectedIndex;
    }

    public bool TryConsumeSelectedDrink()
    {
        if (selectedIndex < 0)
        {
            Debug.Log("No drink selected ¡ª cannot consume");
            return false;
        }

        if (selectedIndex >= inventoryDrinkList.Count)
        {
            Debug.Log("Selected drink index is out of range");
            selectedIndex = -1;
            return false;
        }

        // Get UI element
        GameObject uiItem = inventoryDrinkList[selectedIndex];

        // Remove from lists
        inventoryDrinkList.RemoveAt(selectedIndex);
        MixingManager.instance.createdDrinks.RemoveAt(selectedIndex);

        Destroy(uiItem);

        // reset selection
        selectedIndex = -1;

        return true;
    }
}
