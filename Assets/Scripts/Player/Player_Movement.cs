using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public float moveSpeed = 5f; // movement speed
    [SerializeField] private GameInput gameInput;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private KeyCode selectInput;

    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private GameObject drinkProjectilePrefab;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameInput.OnThrowDrink += GameInput_OnThrowAction;
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

    private void GameInput_OnThrowAction(object sender, System.EventArgs e)
    {
        if (Inventory.instance == null) return;

        bool consumed = Inventory.instance.TryConsumeSelectedDrink();
        if (!consumed)
        {
            Debug.Log("No drink consumed ¡ª cannot throw");
            return;
        }

        ThrowDrink();
    }

    private void ThrowDrink()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePos.z = 0;

        Vector2 dir = (mousePos - transform.position).normalized;

        GameObject proj = Instantiate(drinkProjectilePrefab, transform.position, Quaternion.identity);
        proj.GetComponent<ThrowableDrink>().Init(dir);
    }

}