using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{

    private Vector2 lastInteractInput;
    [SerializeField] private GameInput gameInput;

    private GlasswareCounter currentGlassware;
    private Customer currentCustomer;


    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (currentCustomer != null)
        {
            currentCustomer.Interact();
        }
        else if (currentGlassware != null)
        {
            currentGlassware.Interact();
        }
    }
    // Update is called once per frame
    void Update()
    {
        HandleDetection();
    }

    private void HandleDetection()
    {
        Vector2 moveInput = gameInput.GetMovementVectorNormalized();

        if (moveInput != Vector2.zero)
        {
            lastInteractInput = moveInput;
        }
        float raycastDistance = 0.8f;
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, lastInteractInput, raycastDistance);
        if (raycastHit)
        {
            //Customer Interact
            if (raycastHit.transform.TryGetComponent(out Customer customer))
            {
                currentCustomer = customer;
            }
            //Glassware Interact
            if (raycastHit.transform.TryGetComponent(out GlasswareCounter glasswareCounter))
            {
                currentGlassware = glasswareCounter;
                currentGlassware.GlasswareHighlight(true);
            }
        }
        else
        {
            DeSelect();
        }
    }

    private void DeSelect()
    {
        if (currentGlassware != null)
        {
            currentGlassware.GlasswareHighlight(false);
            currentGlassware = null;
        }
            currentCustomer = null;
    }

}
