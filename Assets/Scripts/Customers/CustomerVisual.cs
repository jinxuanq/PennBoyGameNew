using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerVisual : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField] Sprite fentFolder;
    [SerializeField] Sprite loudTweaker;
    [SerializeField] Sprite susMan;

    void Awake()
    { 
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    public void SetSprite(string customerType)
    {
        if (customerType == "fentFolder")

        {
            spriteRenderer.sprite = fentFolder;
        }
        if (customerType == "loudTweaker")
        {
            spriteRenderer.sprite = loudTweaker;
        }
        if (customerType == "susMan")
        {
            spriteRenderer.sprite = susMan;
        }
    }


}
