using UnityEngine;

public class GrowthBankroll : MoneyGainBankrollBase
{
    [Header("レベルごとの獲得金額の増加値")]
    [SerializeField] private int _addMoneyAmount;
    [Header("成長の最大レベル")]
    [SerializeField] private int _maxGrowthLevel;
    [Header("次レベルに必要なヒット回数")]
    [SerializeField] private int _requireHitCountToNextLevel;

    private int _growthLevel = 1;
    private int _totalHit;
    public override void OnBankrollHit(GameObject ballObject)
    {
        _totalHit++;
        if (_totalHit >= _requireHitCountToNextLevel * _growthLevel)
        {
            OnBankrollGrow();
        }

        // お金の量を増やす
        // 現在の計算式 => 成長レベル * 増加値
        GainMoney(_addMoneyAmount * _growthLevel);
    }
    /// <summary>
    /// バンクロール成長時に呼ばれる処理
    /// </summary>
    private void OnBankrollGrow()
    {
        if (_growthLevel < _maxGrowthLevel)
        {
            _growthLevel++;
        }
    }
}
