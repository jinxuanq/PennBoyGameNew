using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{

    public event EventHandler OnInteractAction;
    private PlayerInputActions playerInputActions;
    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += Interact_performed;

    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public void LockInput(bool locked)
    {
        if (locked)
        {
            playerInputActions.Player.Disable();
            playerInputActions.Locked.Enable();
        }
        else
        {
            playerInputActions.Player.Enable();
            playerInputActions.Locked.Disable();
        }
    }
    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 moveInput = playerInputActions.Player.Move.ReadValue<Vector2>();
        return moveInput;
    }
}
