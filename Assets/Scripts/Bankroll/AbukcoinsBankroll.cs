using UnityEngine;

public class AbukcoinsBankroll : MoneyGainBankrollBase
{
    [Header("当たった時のお金の増加量")]
    [SerializeField] private int _addMoney;

    public override void OnBankrollHit(GameObject ballObject)
    {
        //お金の量を増やす
        GainMoney(_addMoney);
    }
}
