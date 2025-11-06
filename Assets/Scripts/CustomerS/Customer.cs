using System;
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
    private bool hasOrdered = false;
    private int dialogueIndex;
    [SerializeField] private DrinkOrder currOrder;
    private Dialogue dialogueBox;

    private static Customer currCustomer;


    public event System.Action<Customer> OnDrinkOrdered;

    // Name
    public string customerName;

    public string customerType;

    [SerializeField] private CustomerVisual customerVisual;

    public event System.Action OnCustomerLeave; // Event to notify spawner

    private List<string> customerDialogueOne = new List<string>();
    private List<string> customerDialogueTwo = new List<string>();
    private List<string> customerDialogueHasOrdered = new List<string>();

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

    public void AssignSprite()
    {
        customerVisual.SetSprite(customerType);
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

    private void CreateCustomerDialogue()
    {
        customerDialogueOne.Clear();
        customerDialogueTwo.Clear(); //reset dialogue options
        customerDialogueOne.Add("Yo bro lemme get " + currOrder.drinkRecipe.drinkName + " with " + currOrder.garnish);
        customerDialogueTwo.Add("Throw in some " + currOrder.drug + " too, thanks homie.");
        customerDialogueHasOrdered.Add("Hey bro, I already ordered.");

        customerDialogueOne.Add("hey bro i need " + currOrder.drinkRecipe.drinkName.ToLower() + " plus some " + currOrder.garnish.ToLower());
        customerDialogueTwo.Add("if you got me... hook me up with some " + currOrder.drug.ToLower() + " in that..... thanks.");
        customerDialogueHasOrdered.Add("ay bro, im still waiting on my drink");

        customerDialogueOne.Add("erm..... i want " + currOrder.drinkRecipe.drinkName.ToLower() + " with " + currOrder.garnish.ToLower() + " please.......");
        customerDialogueTwo.Add("erm..... and " + currOrder.drug.ToLower() + " plz...........shhhhhh.........");
        customerDialogueHasOrdered.Add("erm.........please hurry.......");

        customerDialogueOne.Add("HEY!! I DONT KNOW WHAT IM GONNA DO WITHOUT A " + currOrder.drinkRecipe.drinkName.ToUpper() + " WITH SOME " + currOrder.garnish.ToUpper());
        customerDialogueTwo.Add("IF YOU DONT ADD " + currOrder.drug.ToUpper() + " IM GONNA TWEAK!!");
        customerDialogueHasOrdered.Add("GET ME MY FUCKING ORDER NOW!!!!!");

        customerDialogueOne.Add("Yoooo brochacho hit me with a " + currOrder.drinkRecipe.drinkName + " and a little " + currOrder.garnish);
        customerDialogueTwo.Add("And u know I lowkey want some " + currOrder.drug + " with that too... thank you my goat");
        customerDialogueHasOrdered.Add("Yoooo my brother, take ur time with my drink man, you always get me right.");

        customerDialogueOne.Add("Can I get one order of " + currOrder.drinkRecipe.drinkName + " with some " + currOrder.garnish + "?");
        customerDialogueTwo.Add(".............put some " + currOrder.drug.ToLower() + " in............ im not a fucking narc i swear bro");
        customerDialogueHasOrdered.Add("im stressed as hell man, hurry so I can get tf out of here...");


        customerDialogueOne.Add("BLESS ME UP WITH  " + currOrder.drinkRecipe.drinkName.ToUpper() + " NOW!!!! DONT FORGET THE " + currOrder.garnish.ToUpper() + "!!!!!");
        customerDialogueTwo.Add("ANDDDDDD OF COURSE....THE USUAL.............my bad..... ill keep it down......just yknow..... " + currOrder.drug.ToLower() + ".");
        customerDialogueHasOrdered.Add("ARE YOU GONNA HURRY UP AND BLESS ME UP NOW???");
    }

    public void SetDialogueBox(Dialogue box)
    {
        dialogueBox = box;
    }

    public void Interact()
    {
        if (interactable)
        {
            if (hasOrdered == false)
            {
                Customer.currCustomer = this;
                GameInput.instance.LockInput(true);
                OnDrinkOrdered?.Invoke(this);
                CreateCustomerDialogue();
                dialogueIndex = UnityEngine.Random.Range(0, customerDialogueOne.Count);
                dialogueBox.AddText(customerDialogueOne[dialogueIndex]);
                dialogueBox.AddText(customerDialogueTwo[dialogueIndex]);
                dialogueBox.SetSprite(customerType, customerName);
                dialogueBox.StartDialogue();
                dialogueBox.OnDialogueEnded += (c) =>
                {
                    GameInput.instance.LockInput(false);
                    dialogueBox.gameObject.SetActive(false);
                    hasOrdered = true;
                };
            }
            else
            {
                GameInput.instance.LockInput(true);
                dialogueBox.AddText(customerDialogueHasOrdered[dialogueIndex]);
                if (dialogueIndex==4 && UnityEngine.Random.Range(0, 2)==1)
                {
                    dialogueBox.AddText("You know I will always glaze you forever king....");
                }
                dialogueBox.SetSprite(customerType, customerName);
                dialogueBox.StartDialogue();
                dialogueBox.OnDialogueEnded += (c) =>
                {
                    GameInput.instance.LockInput(false);
                    dialogueBox.gameObject.SetActive(false);
                };
            }
        }
    }

    public bool GetInteractable()
    {
        return interactable;
    }


    // --------------------
    // Orders and UI
    // --------------------
    public void AssignOrder(DrinkOrder o)
    {
        currOrder = o;

        // Add to UI
        if (GameManager.instance != null)
            GameManager.instance.AddOrderToUI(this);
    }

    public DrinkOrder GetOrder()
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
    public void CompleteOrder(Drink d)
    {
        if (GameManager.instance != null)
        {
            // Give reward
            GameManager.instance.AddMoney((int) (currOrder.CompareScore(d)*20));

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
