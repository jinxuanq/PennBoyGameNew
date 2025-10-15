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

    [SerializeField] private SpriteRenderer sprite;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInput = gameInput.GetMovementVectorNormalized();
        if(moveInput.x >0 )
        {
            sprite.flipX = false;
        }
        else if(moveInput.x <0 )
        {
            sprite.flipX=true;
        }
    }
    void FixedUpdate()
    {
        // Move using physics

        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }

}