using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order : MonoBehaviour
{
    public DrinkRecipe drink;
    public string name;

    public Order(DrinkRecipe drink)
    {
        this.drink = drink;
        name = drink.drinkName;
    }

    public override string ToString()
    {
        return name;
    }
}
