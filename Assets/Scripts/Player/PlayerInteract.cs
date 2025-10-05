using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{

    private Vector3 lastInteractInput;
    [SerializeField] private GlasswareCounter selectedGlassware;

    // Update is called once per frame
    void Update()
    {
        HandleInteractions();
    }

    private void HandleInteractions()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if (moveInput != Vector2.zero)
        {
            lastInteractInput = moveInput;
        }
        float raycastDistance = 0.8f;
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, lastInteractInput, raycastDistance);
        if (raycastHit)
        {
            //Glassware Interact
            if (raycastHit.transform.TryGetComponent(out GlasswareCounter glasswareCounter))
            {
                selectedGlassware.GlasswareHighlight(true);
                if (glasswareCounter == selectedGlassware && Input.GetButtonDown("Interact"))
                {
                    glasswareCounter.Interact();
                }
            }
            else
            {
                selectedGlassware.GlasswareHighlight(false);
            }
        }
        else
        {
            selectedGlassware.GlasswareHighlight(false);
        }
    }

}
