using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Order
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
