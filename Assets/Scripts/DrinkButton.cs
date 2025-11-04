using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkButton : MonoBehaviour
{
    public Drink thisDrink;

    public void SelectDrink()
    {
        PlayerInteract.instance.selectedDrink = thisDrink;
    }
}
