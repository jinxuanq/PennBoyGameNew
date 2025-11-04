using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkOrder : MonoBehaviour
{

    public DrinkRecipe drinkRecipe;
    public string garnish;
    public string drug;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override string ToString()
    {
        return drinkRecipe.drinkName + " with " + garnish + " and " + drug;
    }

    public double CompareScore(Drink d)
    {
        int score = 0;
        if (d.assignedDrink.drinkName.Equals(drinkRecipe.drinkName)) score += 100; else score += 50;
        score += (int) d.mixGrade;
        if (d.assignedGarnish.ToString().Equals(garnish.ToString())) score += 100; else score += 50;
        score += (int) d.garnishGrade;
        if (d.assignedDrug.ToString().Equals(drug.ToString())) score += 100; else score += 50;

        return score/500.0;
    }
}
