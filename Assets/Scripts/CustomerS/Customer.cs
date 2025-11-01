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
    private bool interactable = false;
    [SerializeField] private Drink currOrder;
    private Dialogue dialogueBox;

    public event System.Action<Customer> OnDrinkOrdered;

    // Name
    public string customerName;
    public event System.Action OnCustomerLeave; // Event to notify spawner

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameManager.instance.AddMoney(100);
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
            interactable = true;
            //GetComponent<Collider2D>().isTrigger = true;
            //Debug.Log("Customer reached table: " + table.name);
        }
    }

    public void SetDialogueBox(Dialogue box)
    {
        dialogueBox = box;
    }

    public void Interact()
    {
        Debug.Log("Interacted");
        if (interactable)
        {
            GameInput.instance.LockInput(true);
            dialogueBox.AddText("I want sum of dat good shit");
            dialogueBox.AddText("gimme juice");
            dialogueBox.StartDialogue();
            dialogueBox.OnDialogueEnded += (c) =>
            {
                GameInput.instance.LockInput(false);
                dialogueBox.gameObject.SetActive(false);
                OnDrinkOrdered?.Invoke(this);
            };
        }
    }

    public bool GetInteractable()
    {
        return interactable;
    }


    // --------------------
    // Orders and UI
    // --------------------
    public void AssignOrder(Drink o)
    {
        currOrder = o;

        // Add to UI
        if (GameManager.instance != null)
            GameManager.instance.AddOrderToUI(this);
    }

    public Drink GetOrder()
    {
        return currOrder;
    }

    public bool GetReachedTable()
    {
        return reachedTable;
    }

    // --------------------
    // Call when the customer��s order is served
    // --------------------
    public void CompleteOrder()
    {
        if (GameManager.instance != null)
        {
            // Give reward
            GameManager.instance.AddMoney(10);

            // Remove order from UI
            GameManager.instance.RemoveOrderFromUI(this);
        }

        // Clear local order data
        currOrder = null;

        // Mark table as free
        if (table != null)
            table.isOccupied = false;

        // Notify spawner so the name can be reused
        OnCustomerLeave?.Invoke();

        // Destroy the customer GameObject
        Destroy(gameObject);
    }

}
