using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GlassType { Shot, Highball, Martini, Rocks} // extend as needed
public enum DrugType { Flower, Leaf, Powder, Crystal}

public class Drink : MonoBehaviour
{
    public GlassType? assignedGlass = null;
    public DrinkRecipe assignedDrink = null;
    public DrinkRecipe.Ingredient? assignedGarnish = null;
    public float garnishGrade = 0;
    public float mixGrade = 0;
    public DrugType? assignedDrug = null;

    // public Sprite thumbSprite; // optional thumbnail for UI

    public void AssignGlass(GlassType g)
    {
        assignedGlass = g;
        // TODO: swap model/mesh/material to show glass visually,
        // or spawn a glass prefab parented to this drink.
        Debug.Log($"{name} assigned glass: {g}");
        ApplyGlassVisual(g);
        PrintStatus();

        Inventory.instance.PopulateDrinks();
    }
    public bool HasGlassAssigned()
    {
        return assignedGlass.HasValue;
    }
    private void ApplyGlassVisual(GlassType g)
    {
        // implement your visual logic:
        // - enable the correct glass model on the drink
        // - change parent/transform orientation, etc.
    }


    public void AssignDrink(DrinkRecipe d)
    {
        assignedDrink = d;
        // TODO: swap model/mesh/material to show glass visually,
        // or spawn a glass prefab parented to this drink.
        Debug.Log($"{name} assigned drink: {d}");
        ApplyDrinkVisual(d);
        PrintStatus();

        Inventory.instance.PopulateDrinks();
    }
    private void ApplyDrinkVisual(DrinkRecipe d)
    {
        // implement your visual logic:
        // - enable the correct glass model on the drink
        // - change parent/transform orientation, etc.
    }


    public void AssignGarnish(DrinkRecipe.Ingredient g, float score)
    {
        assignedGarnish = g;
        garnishGrade = score;
        Debug.Log($"{name} assigned garnish: {g}, score: {score}");
        ApplyGarnishVisual(g);
        PrintStatus();

        Inventory.instance.PopulateDrinks();
    }
    private void ApplyGarnishVisual(DrinkRecipe.Ingredient g)
    {
        // implement your visual logic:
        // - enable the correct glass model on the drink
        // - change parent/transform orientation, etc.
    }

    public void AssignMix(float score)
    {
        mixGrade = score;
        // TODO: swap model/mesh/material to show glass visually,
        // or spawn a glass prefab parented to this drink.
        Debug.Log($"{name} assigned blend: {score}");
        PrintStatus();

        Inventory.instance.PopulateDrinks();
    }


    public void AssignDrug(DrugType d)
    {
        assignedDrug = d;
        // TODO: swap model/mesh/material to show glass visually,
        // or spawn a glass prefab parented to this drink.
        Debug.Log($"{name} assigned drug: {d}");
        ApplyDrugVisual(d);
        PrintStatus();

        Inventory.instance.PopulateDrinks();
    }
    private void ApplyDrugVisual(DrugType d)
    {
        // implement your visual logic:
        // - enable the correct glass model on the drink
        // - change parent/transform orientation, etc.
    }

    public bool HasDrugAssigned()
    {
        return assignedDrug.HasValue;
    }
    public bool HasIngAssigned()
    {
        return assignedGarnish.HasValue;
    }
    public bool Finished()
    {
        return (this.HasDrugAssigned()) && (this.HasGlassAssigned() && (this.HasIngAssigned()));
    }


    public void PrintStatus()
    {
        Debug.Log($"DEBUG: assignedDrink={assignedDrink}, assignedGlass={assignedGlass}, assignedGarnish={assignedGarnish}, assignedDrug={assignedDrug}");

    }

}
