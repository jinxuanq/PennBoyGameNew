using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SelectedGlasswareCounter_Visual : MonoBehaviour
{

    [SerializeField] GameObject gameObjectVisual;

    void Start()
    {
        gameObjectVisual.SetActive(false);
    }
    public void Highlight(bool highlighted)
    {
        gameObjectVisual.SetActive(highlighted);
    }

}
