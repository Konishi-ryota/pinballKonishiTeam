using UnityEngine;

/// <summary>
/// 衝突時にお金を得る系バンクロールの基底クラス。
/// </summary>
public abstract class MoneyGainBankrollBase : BankrollBase
{
    private int _ballMultiplier = 1;
    private int _bankrollMultiplier = 1;
    private MoneyManager _moneyManager;

    /// <summary>
    /// 衝突したボールの種類によってお金の倍率を変える。
    /// </summary>
    private int BallMultiplier
    {
        get
        {
            var value = _ballMultiplier;
            _ballMultiplier = 1;
            // 一度倍率が使用されたら、値を初期値に戻す。
            return value;
        }
        set => _ballMultiplier = value;
    }
    protected virtual void Start()
    {
        _moneyManager = FindAnyObjectByType<MoneyManager>();
    }

    /// <summary>
    /// お金の得られる倍率を変える。
    /// </summary>
    public void SetMoneyMultiplier(int value) => _bankrollMultiplier = value;

    /// <summary>
    /// お金を増やす。
    /// </summary>
    /// <param name="value"></param>
    protected void GainMoney(int value) => _moneyManager.AddMoney(value * BallMultiplier * _bankrollMultiplier);
    public override void OnBankrollEffect(GameObject ballObject)
    {
        // 衝突したボールが火属性の場合、倍率を変更。
        if (ballObject.TryGetComponent<BallEffectBehaviour>(out var component) &&
            component.Effect == BallEffect.Fire)
        {
            BallMultiplier = 2;
        }
        OnBankrollHit(ballObject);
    }
    /// <summary>
    /// バンクロールにボールが当たった時に呼び出したい処理
    /// </summary>
    public abstract void OnBankrollHit(GameObject ballObject);
}
