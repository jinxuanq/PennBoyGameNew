using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public GameState currentState;

    //Money
    public static event Action<int> OnMoneyChanged;
    [SerializeField] int money;
    [SerializeField] int moneyWin;

    public int Money
    {
        get => money;
        set
        {
            money = value;
            OnMoneyChanged?.Invoke(money); // notify listeners (like UI)
        }
    }

    // === ORDER UI ===
    [Header("Order UI")]
    [SerializeField] private TextMeshProUGUI orderTextTemplate; // assign the disabled template
    [SerializeField] private Transform orderListContainer;      // assign the OrderListContainer transform
    private Dictionary<Customer, TextMeshProUGUI> activeOrders = new Dictionary<Customer, TextMeshProUGUI>();

    private void Awake()
    {
        if (instance) Destroy(this.gameObject);
        else instance = this;
    }

    // --- Order UI management ---
    /// <summary>Call when a customer receives/places an order.</summary>
    public void AddOrderToUI(Customer customer)
    {
        if (customer == null) return;
        if (customer.GetOrder() == null) return;
        if (activeOrders.ContainsKey(customer)) return; // already shown

        // Instantiate a copy, enable it and set text
        TextMeshProUGUI text = Instantiate(orderTextTemplate, orderListContainer);
        text.gameObject.SetActive(true);

        // Use a clear display name: GameObject name (avoid ambiguous field names)
        string displayName = customer.gameObject.name;
        text.text = $"{displayName}: {customer.GetOrder().ToString()}";

        activeOrders[customer] = text;
    }

    /// <summary>Call if order text needs to be refreshed (e.g., changed name/status).</summary>
    public void UpdateOrderUI(Customer customer)
    {
        if (customer == null) return;
        if (!activeOrders.ContainsKey(customer)) return;

        var text = activeOrders[customer];
        text.text = $"{customer.gameObject.name}: {customer.GetOrder().ToString()}";
    }

    /// <summary>Call when the customer's order is completed / removed.</summary>
    public void RemoveOrderFromUI(Customer customer)
    {
        if (customer == null) return;
        if (activeOrders.TryGetValue(customer, out var text))
        {
            Destroy(text.gameObject);
            activeOrders.Remove(customer);
        }
    }



    public enum GameState
    {
        Start,
        MixDrinks,
        ServeCustomers,
        Win,
        Lose,
        Pause
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (money >= moneyWin){StateChanged(GameState.Win);}
    }

    public void StateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.Start:
                currentState = state;
                GameStart();
                break;
            case GameState.MixDrinks:
                currentState = state;
                MixDrinks();
                break;
            case GameState.ServeCustomers:
                currentState = state;
                ServeCustomers();
                break;
            case GameState.Win:
                currentState = state;
                Win();
                break;
            case GameState.Lose:
                currentState = state;
                Lose();
                break;
            case GameState.Pause:
                currentState = state;
                Pause();
                break;
        }
    }

    private void GameStart()
    {

    }
    private void MixDrinks()
    {

    }
    private void ServeCustomers()
    {

    }
    private void Win()
    {

    }
    private void Lose()
    {

    }
    private void Pause()
    {

    }

    public void AddMoney(int amount)
    {
        Money += amount;
    }

    public void SpendMoney(int amount)
    {
        Money -= amount;
    }
}


