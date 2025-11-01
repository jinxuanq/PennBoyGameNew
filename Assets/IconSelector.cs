using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class IconSelector : MonoBehaviour
{

    [SerializeField] private Sprite fentFolder;
    [SerializeField] private Sprite loudTweaker;

    public void Sprite(String customerType)
    {
        if (customerType == "fentFolder")
        {
            gameObject.GetComponent<UnityEngine.UI.Image>().sprite = fentFolder;
        }
        if (customerType == "loudTweaker")
        {
            gameObject.GetComponent<UnityEngine.UI.Image>().sprite = loudTweaker;
        }
    }
    //    private List<string> customerTypes = new List<string> { "fentFolder", "loudTweaker" };

    //        customer.CustomerType = customerTypes[Random.Range(0, 1)];


}
