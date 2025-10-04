using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{

    private Vector3 lastInteractInput;

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
        float raycastDistance = 1f;
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, lastInteractInput, raycastDistance);
        if (raycastHit)
        {
            Debug.Log(raycastHit.transform);
        }
        else
        {
            Debug.Log("-");
        }
    }
}
