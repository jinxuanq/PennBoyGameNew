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
    public string disguiseType;

    [SerializeField] private CustomerVisual customerVisual;

    public event System.Action OnCustomerLeave; // Event to notify spawner

    private List<string> customerDialogueOne = new List<string>();
    private List<string> customerDialogueTwo = new List<string>();
    private List<string> customerDialogueHasOrdered = new List<string>();
    private List<string> customerDialogueServedGood = new List<string>();
    private List<string> customerDialogueServedMid = new List<string>();
    private List<string> customerDialogueServedBad = new List<string>();
    private string[] disguiseTypes = new string[] {"fentFolder", "loudTweaker", "susMan" };
    private List<string> copDialogueOne = new List<string>();
    private List<string> copDialogueTwo = new List<string>();
    private List<string> copDialogueHasOrdered = new List<string>();
    private List<string> copDialogueServed = new List<string>();

    private bool isChasing = false;
    private Transform chaseTarget;   // player transform
    public float chaseSpeed = 2f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Awake()
    {
        disguiseType = disguiseTypes[UnityEngine.Random.Range(0, disguiseTypes.Length)];

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

        if (isChasing && chaseTarget != null)
    {
        Vector2 chasePos = Vector2.MoveTowards(rb.position, chaseTarget.position, chaseSpeed * Time.deltaTime);
        rb.MovePosition(chasePos);
    }
    }

    public void AssignSprite()
    {
        if (customerType != "cop")
        {
            customerVisual.SetSprite(customerType);
        }
        else
        {
            customerVisual.SetSprite(disguiseType);
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
        }
        if (isChasing && other.gameObject.CompareTag("Player"))
        {
            // Lose money
            if (GameManager.instance != null)
                GameManager.instance.AddMoney(-15); // losing money

            Debug.Log(customerName + " caught the player!");

            OnCustomerLeave?.Invoke();
            Destroy(gameObject);
        }
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
                if (customerType != "cop")
                {
                    Customer.currCustomer = this;
                    GameInput.instance.LockInput(true);
                    OnDrinkOrdered?.Invoke(this);
                    CreateCustomerDialogue();
                    dialogueIndex = UnityEngine.Random.Range(0, customerDialogueOne.Count);
                    dialogueBox.AddText(customerDialogueOne[dialogueIndex]);
                    dialogueBox.AddText(customerDialogueTwo[dialogueIndex]);
                    dialogueBox.SetSprite(customerType, customerName);
                    dialogueBox.SetState("first order");
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
                    
                    Customer.currCustomer = this;
                    GameInput.instance.LockInput(true);
                    OnDrinkOrdered?.Invoke(this);
                    CreateCopDialogue();
                    dialogueBox.AddText(copDialogueOne[UnityEngine.Random.Range(0, copDialogueOne.Count)]);
                    dialogueBox.AddText(copDialogueTwo[UnityEngine.Random.Range(0, copDialogueTwo.Count)]);
                    dialogueBox.SetSprite(disguiseType, customerName);
                    dialogueBox.SetState("first order");
                    dialogueBox.StartDialogue();
                    dialogueBox.OnDialogueEnded += (c) =>
                    {
                        GameInput.instance.LockInput(false);
                        dialogueBox.gameObject.SetActive(false);
                        hasOrdered = true;
                    };
                }
            }
            else
            {
                if (customerType != "cop")
                {
                    GameInput.instance.LockInput(true);
                    dialogueBox.AddText(customerDialogueHasOrdered[dialogueIndex]);
                    if (dialogueIndex == 4 && UnityEngine.Random.Range(0, 2) == 1)
                    {
                        dialogueBox.AddText("You know I will always glaze you forever king....");
                    }
                    dialogueBox.SetSprite(customerType, customerName);
                    dialogueBox.SetState("waiting");
                    dialogueBox.StartDialogue();
                    dialogueBox.OnDialogueEnded += (c) =>
                    {
                        GameInput.instance.LockInput(false);
                        dialogueBox.gameObject.SetActive(false);
                    };
                }
                else
                {
                    GameInput.instance.LockInput(true);
                    dialogueBox.AddText(copDialogueHasOrdered[UnityEngine.Random.Range(0, copDialogueHasOrdered.Count)]);
                    dialogueBox.SetSprite(disguiseType, customerName);
                    dialogueBox.SetState("waiting");
                    dialogueBox.StartDialogue();
                    dialogueBox.OnDialogueEnded += (c) =>
                    {
                        GameInput.instance.LockInput(false);
                        dialogueBox.gameObject.SetActive(false);
                    };
                }
                
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

        GameInput.instance.LockInput(true);
        dialogueBox.clearLines();
        if (customerType != "cop")
        {
            if ((int)(currOrder.CompareScore(d) * 20) < 15)
            {
                dialogueBox.AddText(customerDialogueServedBad[dialogueIndex]);
            }
            else if ((int)(currOrder.CompareScore(d) * 20) < 19)
            {
                dialogueBox.AddText(customerDialogueServedMid[dialogueIndex]);
            }
            else
            {
                dialogueBox.AddText(customerDialogueServedGood[dialogueIndex]);
            }

            dialogueBox.SetSprite(customerType, customerName);
            dialogueBox.SetState("served");
        }
        else
        {
            if (d.assignedDrug == DrugType.Empty)
            {
                dialogueBox.AddText(copDialogueServed[UnityEngine.Random.Range(0, copDialogueServed.Count)]);
            }
            else
            {
                dialogueBox.AddText("You are under arrest for the sale of " + d.assignedDrug + ".");
            }
            dialogueBox.SetSprite(disguiseType, "Cop");
            customerVisual.showCop();
            dialogueBox.showCop();
            dialogueBox.SetState("served");
        }
        dialogueBox.StartDialogue();
        dialogueBox.OnDialogueEnded += (c) =>
        {
            GameInput.instance.LockInput(false);
            dialogueBox.gameObject.SetActive(false);
            float score = (float)(currOrder.CompareScore(d) * 20f);


            bool shouldChase = false;

            if (customerType != "cop")
            {
                if (score < 15f)
                    shouldChase = true; // bad rating → chase
                else
                {
                    // Good customer, normal leave
                    LeaveNormally(score);
                    return;
                }
            }
            else
            {
                // COP logic
                if (d.assignedDrug != DrugType.Empty)
                    shouldChase = true; // bust → chase
                else
                {
                    LeaveNormally(score);
                    return;
                }
            }

            if (shouldChase)
            {
                StartChasing();
            }
            else
                LeaveNormally(score);
            /*if (GameManager.instance != null)
            {
                // Give reward

                GameManager.instance.AddMoney((int)(currOrder.CompareScore(d) * 20));

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
            Destroy(gameObject);*/
        };
    }

    private void CreateCustomerDialogue()
    {
        customerDialogueOne.Clear();
        customerDialogueTwo.Clear();
        customerDialogueHasOrdered.Clear();
        customerDialogueServedGood.Clear();
        customerDialogueServedMid.Clear();
        customerDialogueServedBad.Clear(); //reset dialogue options
        customerDialogueOne.Add("Yo bro lemme get " + currOrder.drinkRecipe.drinkName + " with " + currOrder.garnish);
        customerDialogueTwo.Add("Throw in some " + currOrder.drug + " too, thanks homie.");
        customerDialogueHasOrdered.Add("Hey bro, I already ordered.");
        customerDialogueServedGood.Add("Ayy thats the stuff, thank you bro");
        customerDialogueServedMid.Add("Thanks");
        customerDialogueServedBad.Add("...I don't know man, you really messed my drink up.");

        customerDialogueOne.Add("hey bro i need " + currOrder.drinkRecipe.drinkName.ToLower() + " plus some " + currOrder.garnish.ToLower());
        customerDialogueTwo.Add("if you got me... hook me up with some " + currOrder.drug.ToLower() + " in that..... thanks.");
        customerDialogueHasOrdered.Add("ay bro, im still waiting on my drink");
        customerDialogueServedGood.Add("'preciate it, you make some good shit bro");
        customerDialogueServedMid.Add("thanks for the drink");
        customerDialogueServedBad.Add("this is terrible bro... what are you doing?");

        customerDialogueOne.Add("erm..... i want " + currOrder.drinkRecipe.drinkName.ToLower() + " with " + currOrder.garnish.ToLower() + " please.......");
        customerDialogueTwo.Add("erm..... and " + currOrder.drug.ToLower() + " plz...........shhhhhh.........");
        customerDialogueHasOrdered.Add("erm.........please hurry.......");
        customerDialogueServedGood.Add("erm.......mmmmmmmmmmmm......i like this......");
        customerDialogueServedMid.Add("erm......thanks....i guess.....");
        customerDialogueServedBad.Add("ew......");

        customerDialogueOne.Add("HEY!! I DONT KNOW WHAT IM GONNA DO WITHOUT A " + currOrder.drinkRecipe.drinkName.ToUpper() + " WITH SOME " + currOrder.garnish.ToUpper());
        customerDialogueTwo.Add("IF YOU DONT ADD " + currOrder.drug.ToUpper() + " IM GONNA TWEAK!!");
        customerDialogueHasOrdered.Add("GET ME MY FUCKING ORDER NOW!!!!!");
        customerDialogueServedGood.Add("LETS FUCKING GOOOOO THIS SHIT IS GAS IM GONNA TWEAK TF OUT");
        customerDialogueServedMid.Add("THANKS FOR THE FIX");
        customerDialogueServedBad.Add("FUCK YOU ASSHOLE ARE YOU TRYING TO RUIN ME????");

        customerDialogueOne.Add("Yoooo brochacho hit me with a " + currOrder.drinkRecipe.drinkName + " and a little " + currOrder.garnish);
        customerDialogueTwo.Add("And u know I lowkey want some " + currOrder.drug + " with that too... thank you my goat");
        customerDialogueHasOrdered.Add("Yoooo my brother, take ur time with my drink man, you always get me right.");
        customerDialogueServedGood.Add("This is the greatest drink I have ever tasted in my entire life brochacho. You will forever be my glorious king.");
        customerDialogueServedMid.Add("My goat never misses");
        customerDialogueServedBad.Add("Umm.............n-no!! I like it bro!!!....I swear.....");

        customerDialogueOne.Add("Can I get one order of " + currOrder.drinkRecipe.drinkName + " with some " + currOrder.garnish + "?");
        customerDialogueTwo.Add(".............put some " + currOrder.drug.ToLower() + " in............ im not a fucking narc i swear bro");
        customerDialogueHasOrdered.Add("im stressed as hell man, hurry so I can get tf out of here...");
        customerDialogueServedGood.Add("Fuck yea...this is amazing bro....see you later.");
        customerDialogueServedMid.Add("thanks for the totally non-drugged beverage.... see you later");
        customerDialogueServedBad.Add("fuck man, you really make me freak out just to serve me this???");


        customerDialogueOne.Add("BLESS ME UP WITH  " + currOrder.drinkRecipe.drinkName.ToUpper() + " NOW!!!! DONT FORGET THE " + currOrder.garnish.ToUpper() + "!!!!!");
        customerDialogueTwo.Add("ANDDDDDD OF COURSE....THE USUAL.............my bad..... ill keep it down......just yknow..... " + currOrder.drug.ToLower() + ".");
        customerDialogueHasOrdered.Add("ARE YOU GONNA HURRY UP AND BLESS ME UP NOW???");
        customerDialogueServedGood.Add("YOU HAVE BLESSED ME UP.");
        customerDialogueServedMid.Add("I AM KINDA BLESSED UP.");
        customerDialogueServedBad.Add("I AM NOT BLESSED UP AND I HATE YOU.");

    }
    
    private void CreateCopDialogue()
    {
        copDialogueOne.Clear();
        copDialogueTwo.Clear();
        copDialogueHasOrdered.Clear();
        copDialogueServed.Clear();

        copDialogueOne.Add("Hello. Can I get one order of a " + currOrder.drinkRecipe.drinkName + " with some " + currOrder.garnish + "?");
        copDialogueTwo.Add("If you will, please add " + currOrder.drug + ".");
        copDialogueHasOrdered.Add("Take your time with my order.");
        copDialogueServed.Add("No " + currOrder.drug + "? I see. I will be watching you.");

        copDialogueOne.Add("May I please order a " + currOrder.drinkRecipe.drinkName + " with added " + currOrder.garnish + "?");
        copDialogueTwo.Add("If accessable, an addition of " + currOrder.drug + " would be lovely.");
        copDialogueHasOrdered.Add("Hello. Do you have my order?");
        copDialogueServed.Add("I see you have left out something. Very interesting.");

        copDialogueOne.Add("Hello, I would like a " + currOrder.drinkRecipe.drinkName + " with an addition of " + currOrder.garnish + ".");
        copDialogueTwo.Add("Could you please add " + currOrder.drug + " to that? Thank you.");
        copDialogueHasOrdered.Add("I am looking forward to my drink.");
        copDialogueServed.Add("I'm not sure if this is my exact order...I wonder why? You would know, wouldn't you.");

        copDialogueOne.Add("Good day to you. May I have a " + currOrder.drinkRecipe.drinkName + " with " + currOrder.garnish + "?");
        copDialogueTwo.Add("I would also like " + currOrder.drug + " added to that.");
        copDialogueHasOrdered.Add("When about will my drink be ready?");
        copDialogueServed.Add("Don't think you're so safe.");

        copDialogueOne.Add("Good evening. I would like a " + currOrder.drinkRecipe.drinkName + " with " + currOrder.garnish + ".");
        copDialogueTwo.Add("Please add " + currOrder.drug + " to that order, too.");
        copDialogueHasOrdered.Add("I hope all is well with you.");
        copDialogueServed.Add("Who do you think you are?");

    }

    private void StartChasing()
    {
        Debug.Log(customerName + " IS NOW CHASING YOU!");

        isChasing = true;
        interactable = false;
        hasOrdered = true;
        chaseTarget = GameObject.FindGameObjectWithTag("Player").transform;

        // Free the table immediately
        if (table != null)
            table.isOccupied = false;

        // No UI order anymore
        if (GameManager.instance != null)
            GameManager.instance.RemoveOrderFromUI(this);

        currOrder = null;
    }

    private void LeaveNormally(float score)
    {
        if (GameManager.instance != null)
            GameManager.instance.AddMoney((int)score);

        if (GameManager.instance != null)
            GameManager.instance.RemoveOrderFromUI(this);

        currOrder = null;

        if (table != null)
            table.isOccupied = false;

        OnCustomerLeave?.Invoke();
        Destroy(gameObject);
    }
    /*
    void OnTriggerEnter2D(Collider2D other)
    {
        if (isChasing && other.gameObject.CompareTag("Player"))
        {
            // Lose money
            if (GameManager.instance != null)
                GameManager.instance.AddMoney(-15); // losing money

            Debug.Log(customerName + " caught the player!");

            OnCustomerLeave?.Invoke();
            Destroy(gameObject);
        }
    }
    */
}
