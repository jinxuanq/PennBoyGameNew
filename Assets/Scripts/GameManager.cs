using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public GameState currentState;

    //Money
    public int money;
    [SerializeField] int moneyWin;

    private void Awake()
    {
        if (instance) Destroy(this.gameObject);
        else instance = this;
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
}


