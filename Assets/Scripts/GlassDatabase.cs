using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Drink/GlassDatabase")]

public class GlassDatabase : ScriptableObject
{
    public List<GlassType> availableGlasses = new List<GlassType>();
}