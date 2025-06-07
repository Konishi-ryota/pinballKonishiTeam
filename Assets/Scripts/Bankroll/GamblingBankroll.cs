using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField] Text text;
    [SerializeField] Text hitormiss;
    //[SerializeField] UnderPinUI _underHeadMsgPrefab;
    //UnderPinUI _underPinMsg;

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
        //テキストを最初に表示させない
        text.gameObject.SetActive(false);
        hitormiss.gameObject.SetActive(false);
        //_underPinMsg = Instantiate(_underHeadMsgPrefab, canvasRect);
        //_underPinMsg.targetTran = transform;
    }
    private void Update()
    {
        //_underPinMsg.ShowMsg(_coinPinHitCount);
    }
    //当たった時の効果
    public override void OnBankrollEffect(GameObject target)
    {
        //コインに当たったらの処理
        if (state == GamblePinState.Coin)
        {
            _coinPinHitCount++;
            //当たった回数を表示
            text.gameObject.SetActive(true);
            text.text = _coinPinHitCount.ToString();
            int _coinFlipResult = Random.Range(0, 2);
            Debug.Log(_coinFlipResult + "が出た");
            if (_coinFlipResult == 0)
            {
                //金を減らす処理
                _moneyManager.DecreaseMoney(_getCoin);
                Debug.Log($"{_getCoin}円失った。");
                //はずれのテキストを表示
                hitormiss.gameObject.SetActive(true );
                hitormiss.text = "はずれ";
            }
            else if (_coinFlipResult == 1)
            {
                //金を増やす処理
                _moneyManager.AddMoney(_getCoin);
                Debug.Log($"{_getCoin}円手に入れた");
                //あたりのテキスト表示
                hitormiss.gameObject.SetActive(true);
                hitormiss.text = "当たり";
            }

            if (_coinPinHitCount == _diceChangeCount)
            {
                //状態をダイスに変更
                state = GamblePinState.Dice;
                hitormiss.gameObject.SetActive(false);
                text.gameObject.SetActive(false);
                //デバック用にわかりやすくしているだけなので、prefab入れたらコメントアウトしてくれて大丈夫
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
            Debug.Log($"所持金が{_rolledDiceNumber}倍された");

            state = GamblePinState.Coin;
            //デバック用にわかりやすくしているだけなので、prefab入れたらコメントアウトしてくれて大丈夫
            GetComponent<Renderer>().material.color = Color.blue;

            
        }
    }

}
