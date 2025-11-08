using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderIconSetter : MonoBehaviour
{
     [SerializeField] private Sprite fentFolder;
    [SerializeField] private Sprite loudTweaker;
    [SerializeField] private Sprite susMan;
    [SerializeField] private UnityEngine.UI.Image iconImage;

    public void SetSprite(String customerType)
    {
        if (customerType == "fentFolder")

        {
            iconImage.sprite = fentFolder;
        }
        if (customerType == "loudTweaker")
        {
            iconImage.sprite = loudTweaker;
        }
        if(customerType == "susMan")
        {
            iconImage.sprite = susMan;
        }
    }
}
