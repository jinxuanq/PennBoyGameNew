using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlasswareCounter : MonoBehaviour
{

    [SerializeField] private SelectedGlasswareCounter_Visual selectedGlasswareCounter_Visual;
    public void Interact()
    {
        Debug.Log("Glassware Interact");
        MixingManager.instance.OpenMixingUI();
    }

    public void GlasswareHighlight(bool highlighted)
    {
        selectedGlasswareCounter_Visual.Highlight(highlighted);
    }

}
