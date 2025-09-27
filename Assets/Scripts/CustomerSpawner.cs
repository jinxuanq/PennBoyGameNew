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


    void Start()
    {
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

        // Spawn the customer
        GameObject customerObj = Instantiate(customerPrefab, transform.position, Quaternion.identity);
        Customer customer = customerObj.GetComponent<Customer>();
        customer.AssignTable(chosenTable);
    }

}
