using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GlassType { Coupe, Highball, Martini, Collins, Tumbler } // extend as needed


public class Drink : MonoBehaviour
{
    public GlassType? assignedGlass = null;

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
}
