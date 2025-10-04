using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyUI : MonoBehaviour
{
[SerializeField] private TMP_Text moneyText;

    private void Start()
    {
        UpdateMoneyText(GameManager.instance.Money);
    }

    private void OnEnable()
    {
        GameManager.OnMoneyChanged += UpdateMoneyText;
    }

    private void OnDisable()
    {
        GameManager.OnMoneyChanged -= UpdateMoneyText;
    }

    private void UpdateMoneyText(int newAmount)
    {
        moneyText.text = "$" + newAmount.ToString();
    }

}
