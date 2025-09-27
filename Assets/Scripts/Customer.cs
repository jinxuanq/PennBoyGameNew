using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed = 2f;
    private Transform targetTable;
    private Table table;
    private bool reachedTable = false;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!reachedTable && targetTable != null)
        {
            // Move toward table
            Vector2 newPos = Vector2.MoveTowards(rb.position, targetTable.position, moveSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
        }
    }

    public void AssignTable(Table newTable)
    {
        table = newTable;
        targetTable = newTable.transform;
        table.isOccupied = true; // mark the table as taken
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        // Only stop if we enter the table's seat zone
        if (table != null && other == table.seatZone)
        {
            reachedTable = true;
            GetComponent<Collider2D>().isTrigger = true;
            Debug.Log("Customer reached table: " + table.name);
        }
    }

}
