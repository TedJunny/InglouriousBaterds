using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    // 바리게이트의 hp
    public static MoneyManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    //바리게이트의 최대HP
    public int firstMoney = 500;
    private int currentMoney;
    //HP Text
    public Text Money_UI;

    public int MONEY
    {
        get
        {
            return currentMoney;
        }
        set
        {
            currentMoney = value;
            Money_UI.text = currentMoney.ToString();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        MONEY = firstMoney;
    }

    // Update is called once per frame
    void Update()
    {

    }
}


