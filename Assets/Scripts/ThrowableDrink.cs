using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableDrink : MonoBehaviour
{
    public float speed = 12f;
    private Vector2 direction;

    public void Init(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Customer"))
        {
            Customer c = other.GetComponent<Customer>();

            if (c != null && c.IsChasing())
            {
                c.KillChasingCustomer();  // <-- legal call
            }

            Destroy(gameObject);
        }
    }
}