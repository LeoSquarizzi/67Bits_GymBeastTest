using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public int money;
    TextMeshProUGUI moneyTxt;

    public static MoneyManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        UpdateUI();
    }

    public void AddMoney(int toAdd)
    {
        money += toAdd;
        UpdateUI();
    }

    public bool ChargeMoney(int price)
    {
        if (money >= price)
        {
            money -= price;
            UpdateUI();
            return true;
        }
        return false;
    }

    private void UpdateUI()
    {
        if(moneyTxt ==  null)
            moneyTxt = GameObject.FindGameObjectWithTag("MoneyLabel").GetComponent<TextMeshProUGUI>();

        moneyTxt.text = money.ToString();
    }

    public bool CheckMoney(int value)
    {
        if (money >= value)
            return true;
        return false;
    }
}
