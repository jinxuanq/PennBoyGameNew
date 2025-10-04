using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject customerPrefab;
    private Table[] tables;
    public float initialSpawnInterval = 5f;   // starting spawn interval (seconds)
    public float minSpawnInterval = 1f;       // fastest spawn rate
    public float gameDurationForMaxSpeed = 120f; // time in seconds until max speed reached
    private float elapsedTime = 0f;  // track how long the game has been running

    //Drink Order
    private List<DrinkRecipe> allDrinks;
    public List<Order> orders;

    // Names
    private List<string> possibleNames = new List<string> { "Alice", "Bob", "Charlie", "Diana", "Eve", "Frank", "Grace", "Hank" };
    private List<string> namesInUse = new List<string>();

    void Start()
    {
        Debug.Log(SummonManager.instance.allDrinks.Count);
        allDrinks = SummonManager.instance.allDrinks;

        tables = FindObjectsOfType<Table>();
        StartCoroutine(SpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
}
    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnCustomer();

            // Calculate dynamic interval
            float t = Mathf.Clamp01(elapsedTime / gameDurationForMaxSpeed); // 0 ¡ú 1
            float currentInterval = Mathf.Lerp(initialSpawnInterval, minSpawnInterval, t);

            yield return new WaitForSeconds(currentInterval);
        }
    }

    void SpawnCustomer()
    {
        // Collect all free tables
        var freeTables = new System.Collections.Generic.List<Table>();
        foreach (Table t in tables)
        {
            if (!t.isOccupied) freeTables.Add(t);
        }

        if (freeTables.Count == 0)
        {
            Debug.Log("All tables are full ¡ª no customer spawned.");
            return;
        }

        // Pick a random free table
        Table chosenTable = freeTables[Random.Range(0, freeTables.Count)];

        if (namesInUse.Count >= possibleNames.Count)
        {
            Debug.LogWarning("No unique names left ¡ª cannot spawn new customer.");
            return;
        }
        string customerName = GetRandomAvailableName();
        namesInUse.Add(customerName);

        // Spawn the customer
        GameObject customerObj = Instantiate(customerPrefab, transform.position, Quaternion.identity);
        Customer customer = customerObj.GetComponent<Customer>();
        customer.customerName = customerName;
        customerObj.name = customerName;       // set GameObject name in hierarchy
        customer.AssignTable(chosenTable);

        //Assign Order to Customer
        Order o = new Order(allDrinks[Random.Range(0, allDrinks.Count)]);
        orders.Add(o);
        customer.AssignOrder(o);
        Debug.Log(customer.GetOrder());
        //Debug.Log(o);

        // Listen for when customer leaves to free name
        customer.OnCustomerLeave += () =>
        {
            namesInUse.Remove(customerName);
            orders.Remove(o); // optional: remove order from spawner's list
        };

    }
    private string GetRandomAvailableName()
    {
        List<string> availableNames = new List<string>();
        foreach (string n in possibleNames)
        {
            if (!namesInUse.Contains(n))
                availableNames.Add(n);
        }
        int index = Random.Range(0, availableNames.Count);
        return availableNames[index];
    }
}
