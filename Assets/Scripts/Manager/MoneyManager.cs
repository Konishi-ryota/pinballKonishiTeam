using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    [Header("初期のお金")] 
    [SerializeField] private int _initialMoney;
    [Header("お金テキスト")] 
    [SerializeField] private Text _moneyText;
    private int _money;
    public int Money => _money;
    
    private void Start()
    {
        _money = _initialMoney;
        SetMoneyText();
    }
    
    //MoneyTextの更新をするメソッド
    private void SetMoneyText()
    {
        string format = _money == 0 ?  "0" : _money.ToString("0,#");
        _moneyText.text = format;
    }

    /// <summary>
    /// お金を増やす処理
    /// </summary>
    /// <param name="money">増やす量</param>
    public void AddMoney(int money)
    {
        _money += money;
        SetMoneyText();
    }

    /// <summary>
    /// お金を減らす処理
    /// </summary>
    /// <param name="money">減らす量</param>
    public void DecreaseMoney(int money)
    {
        _money -= money;
        if (_money < 0)
        {
            _money = 0;
        }
        SetMoneyText();
    }
}
