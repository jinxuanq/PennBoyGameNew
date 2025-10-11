using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public float moveSpeed = 5f; // movement speed
    [SerializeField] private GameInput gameInput;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInput = gameInput.GetMovementVectorNormalized();
    }
    void FixedUpdate()
    {
        // Move using physics

        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }

}