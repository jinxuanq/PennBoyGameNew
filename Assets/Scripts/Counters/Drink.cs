using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GlassType { Coupe, Highball, Martini, Collins, Tumbler } // extend as needed
public enum GarnishType { Mint, Ice, Lemon}
public enum DrugType { Flower, Leaf, WhitePowder}

public class Drink : MonoBehaviour
{
    public GlassType? assignedGlass = null;
    public DrinkRecipe assignedDrink = null;
    public GarnishType? assignedGarnish = null;
    public int garnishGrade = 0;
    public int blendGrade = 0;
    public DrugType? assignedDrug = null;

    // public Sprite thumbSprite; // optional thumbnail for UI

    public void AssignGlass(GlassType g)
    {
        assignedGlass = g;
        // TODO: swap model/mesh/material to show glass visually,
        // or spawn a glass prefab parented to this drink.
        Debug.Log($"{name} assigned glass: {g}");
        ApplyGlassVisual(g);
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
    }
    private void ApplyDrinkVisual(DrinkRecipe d)
    {
        // implement your visual logic:
        // - enable the correct glass model on the drink
        // - change parent/transform orientation, etc.
    }


    public void AssignGarnish(GarnishType g, int score)
    {
        assignedGarnish = g;
        garnishGrade = score;
        // TODO: swap model/mesh/material to show glass visually,
        // or spawn a glass prefab parented to this drink.
        Debug.Log($"{name} assigned garnish: {g}, score: {score}");
        ApplyGarnishVisual(g);
    }
    private void ApplyGarnishVisual(GarnishType g)
    {
        // implement your visual logic:
        // - enable the correct glass model on the drink
        // - change parent/transform orientation, etc.
    }

    public void AssignBlender(int score)
    {
        blendGrade = score;
        // TODO: swap model/mesh/material to show glass visually,
        // or spawn a glass prefab parented to this drink.
        Debug.Log($"{name} assigned blend: {score}");
    }


    public void AssignDrug(DrugType d)
    {
        assignedDrug = d;
        // TODO: swap model/mesh/material to show glass visually,
        // or spawn a glass prefab parented to this drink.
        Debug.Log($"{name} assigned drug: {d}");
        ApplyDrugVisual(d);
    }
    private void ApplyDrugVisual(DrugType d)
    {
        // implement your visual logic:
        // - enable the correct glass model on the drink
        // - change parent/transform orientation, etc.
    }

}
