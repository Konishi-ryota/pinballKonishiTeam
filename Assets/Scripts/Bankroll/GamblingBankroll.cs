using UnityEngine;
using UnityEngine.UI;

public class GamblingBankroll : BankrollBase
{
    //コインピンに当たった回数（10回で進化）
    private int _coinPinHitCount = 0;
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
    [Header("コインピンのメッシュ")]
    [SerializeField] private Mesh CoinMesh;
    [Header("ダイスピンのメッシュ")]
    [SerializeField] private Mesh DiceMesh;
    [Header("コインピンのマテリアル")]
    [SerializeField] private Material CoinMaterial;
    [Header("ダイスピンのマテリアル")]
    [SerializeField] private Material DiceMaterial;
    [Header("コインピンのanimtorController")]
    [SerializeField] private RuntimeAnimatorController CoinController;
    [Header("ダイスピンのanimtorController")]
    [SerializeField] private RuntimeAnimatorController DiceController;
    [SerializeField] Text text;
    [SerializeField] Text hitormiss;
    private int _DicedNumber;
    //[SerializeField] UnderPinUI _underHeadMsgPrefab;
    //UnderPinUI _underPinMsg;

    private MoneyManager _moneyManager;
    private SoundManager _soundManager;
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
        _soundManager = GameObject.FindAnyObjectByType<SoundManager>();
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
                Debug.Log($"{_getCoin}�~�������B");
                //�͂���̃e�L�X�g��\��
                hitormiss.gameObject.SetActive(true);
                hitormiss.text = "はずれ";
                _soundManager.PlaySE(SESoundData.SE.Coinfail);
            }
            else if (_coinFlipResult == 1)
            {
                //金を増やす処理
                _moneyManager.AddMoney(_getCoin);
                Debug.Log($"{_getCoin}�~��ɓ��ꂽ");
                //������̃e�L�X�g�\��

                hitormiss.gameObject.SetActive(true);
                hitormiss.text = "当たり";
                _soundManager.PlaySE(SESoundData.SE.CoinOK);
            }

            if (_coinPinHitCount == _diceChangeCount)
            {
                //状態をダイスに変更
                state = GamblePinState.Dice;
                hitormiss.gameObject.SetActive(false);
                text.gameObject.SetActive(false);
                //デバック用にわかりやすくしているだけなので、prefab入れたらコメントアウトしてくれて大丈夫
                //GetComponent<Renderer>().material.color = Color.red;
                transform.GetChild(0).localRotation = Quaternion.Euler(-40, 0, 0);//DiceRotaionのローテーションを変更。
                transform.GetChild(0).GetChild(0).GetComponent<MeshFilter>().mesh = DiceMesh;
                transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = DiceMaterial;
                transform.GetChild(0).GetChild(0).GetComponent<Animator>().runtimeAnimatorController = DiceController;
                //GetComponent<MeshFilter>().mesh = DiceMesh;;
                //GetComponent<Renderer>().material = DiceMaterial;
                _coinPinHitCount = 0;
            }
        }
        //ダイスに当たったらの処理
        else
        {
            _soundManager.PlaySE(SESoundData.SE.DiceRoll);
            Debug.Log(_moneyMultiplierFromDice.Length);

            //Debug.Log("ダイスの"+_rolledDiceNumber +1 + "が出た");
            _DicedNumber = Random.Range(0, _moneyMultiplierFromDice.Length);
            DiceRoll(_DicedNumber);
            Invoke(nameof(Chenge), 2f);

            /*_moneyManager.MultiplicationMoney(_moneyMultiplierFromDice[_rolledDiceNumber]);
            state = GamblePinState.Coin;
            //デバック用にわかりやすくしているだけなので、prefab入れたらコメントアウトしてくれて大丈夫
            //GetComponent<Renderer>().material.color = Color.blue;
            transform.GetChild(0).localRotation = Quaternion.Euler(30, 0, 0);//DiceRotaionのローテーションを変更。
            transform.GetChild(0).GetChild(0).GetComponent<MeshFilter>().mesh = CoinMesh;
            transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = CoinMaterial;
            transform.GetChild(0).GetChild(0).GetComponent<Animator>().runtimeAnimatorController = CoinController;
            //GetComponent<Renderer>().material = CoinMaterial;*/

        }
    }
    private void DiceRoll(int rolledDiceNumber)
    {
        
            transform.GetChild(0).GetChild(0).GetComponent<Animator>().SetInteger("DiceNumber", rolledDiceNumber+1);
        
       
        _moneyManager.MultiplicationMoney(_moneyMultiplierFromDice[rolledDiceNumber]);
        state = GamblePinState.Coin;
        //デバック用にわかりやすくしているだけなので、prefab入れたらコメントアウトしてくれて大丈夫
        //GetComponent<Renderer>().material.color = Color.blue;
        //GetComponent<Renderer>().material = CoinMaterial;
    }
    private void Chenge()
    {
        if (_DicedNumber == 0)
        {
            _soundManager.PlaySE(SESoundData.SE.Dice1);
        }
        if (_DicedNumber == 5)
        {
            _soundManager.PlaySE(SESoundData.SE.Dice6);
        }
        transform.GetChild(0).GetChild(0).GetComponent<Animator>().runtimeAnimatorController = CoinController;
        transform.GetChild(0).localRotation = Quaternion.Euler(30, 0, 0);//DiceRotaionのローテーションを変更。
        transform.GetChild(0).GetChild(0).GetComponent<MeshFilter>().mesh = CoinMesh;
        transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = CoinMaterial;
    }

}
