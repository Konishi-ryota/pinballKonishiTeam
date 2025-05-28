using UnityEngine;

public class GamblingBankroll : BankrollBase
{
    //コインピンに当たった回数（10回で進化）
    private int _coinPinHitCount = 0;
    //サイコロの出目（1～6）
    private int _rolledDiceNumber = 0;
    //出目に応じた倍率（例：1→0.7）
    [Header("コインを振った時にもらえる増減コイン")]
    [SerializeField] private float[] _moneyMultiplierFromDice = { 0.7f, 0.8f, 1.0f, 1.2f, 1.5f, 2.0f };
    //倍率をかける前の所持金（履歴として）＞デバッグ用にコインが増加しているか確認できる
    private int _moneyBeforeMultiplier = 0;
    //0か1（コイントス結果）
    private int _coinFlipResult = 0;
    //直前のギャンブル結果
    private int _lastCoinResult = 0;
    //サイコロが既に振られたかどうかのフラグ
    private bool _hasRolledDice = false;
    //コインを振った時にもらえるコイン（+-5000）
    [Header("コインを振った時にもらえる増減コイン")]
    [SerializeField] private int _getCoin = 5000;
    [Header("コインに何回当たったらダイスになるかの回数")]
    [SerializeField] private int _diceChangeCount = 10;


    private MoneyManager _moneyManager;
    public GamblePinState state;
    public enum GamblePinState
    {
        Coin,
        Dice
    }
    private void Start()
    {
        state = GamblePinState.Coin;
        _moneyManager = GameObject.FindAnyObjectByType<MoneyManager>();
    }
    //当たった時の効果
    public override void OnBankrollEffect(GameObject target)
    {
        //コインに当たったらの処理
        if (state == GamblePinState.Coin)
        {
            _coinPinHitCount++;
            int _coinFlipResult = Random.Range(0, 2);
            Debug.Log(_coinFlipResult + "が出た");
            if (_coinFlipResult == 0)
            {
                //金を減らす処理
                _moneyManager.DecreaseMoney(_getCoin);
            }
            else if (_coinFlipResult == 1)
            {
                //金を増やす処理
                _moneyManager.AddMoney(_getCoin);
            }

            if (_coinPinHitCount == _diceChangeCount)
            {
                //状態をダイスに変更
                state = GamblePinState.Dice;
                GetComponent<Renderer>().material.color = Color.red;
                _coinPinHitCount = 0;
            }
        }
        //ダイスに当たったらの処理
        else
        {
            int _rolledDiceNumber = Random.Range(0, _moneyMultiplierFromDice.Length);
            Debug.Log(_rolledDiceNumber+1 + "が出た");

            _moneyManager.MultiplicationMoney(_moneyMultiplierFromDice[_rolledDiceNumber]);
            state= GamblePinState.Coin;
            GetComponent<Renderer>().material.color = Color.blue;
        }
    }

}
