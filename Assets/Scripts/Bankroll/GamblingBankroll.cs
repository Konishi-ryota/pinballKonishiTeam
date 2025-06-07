using UnityEngine;
using UnityEngine.UI;

public class GamblingBankroll : BankrollBase
{
    //�R�C���s���ɓ��������񐔁i10��Ői���j
    private int _coinPinHitCount = 0;
    //�T�C�R���̏o�ځi1�`6�j
    private int _rolledDiceNumber = 0;
    //�o�ڂɉ������{���i��F1��0.7�j
    [Header("�R�C����U�������ɂ��炦�鑝���R�C��")]
    [SerializeField] private float[] _moneyMultiplierFromDice = { 0.7f, 0.8f, 1.0f, 1.2f, 1.5f, 2.0f };
    //�{����������O�̏������i�����Ƃ��āj���f�o�b�O�p�ɃR�C�����������Ă��邩�m�F�ł���
    private int _moneyBeforeMultiplier = 0;
    //0��1�i�R�C���g�X���ʁj
    private int _coinFlipResult = 0;
    //���O�̃M�����u������
    private int _lastCoinResult = 0;
    //�T�C�R�������ɐU��ꂽ���ǂ����̃t���O
    private bool _hasRolledDice = false;
    //�R�C����U�������ɂ��炦��R�C���i+-5000�j
    [Header("�R�C����U�������ɂ��炦�鑝���R�C��")]
    [SerializeField] private int _getCoin = 5000;
    [Header("�R�C���ɉ��񓖂�������_�C�X�ɂȂ邩�̉�")]
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
        //�e�L�X�g���ŏ��ɕ\�������Ȃ�
        text.gameObject.SetActive(false);
        hitormiss.gameObject.SetActive(false);
        //_underPinMsg = Instantiate(_underHeadMsgPrefab, canvasRect);
        //_underPinMsg.targetTran = transform;
    }
    private void Update()
    {
        //_underPinMsg.ShowMsg(_coinPinHitCount);
    }
    //�����������̌���
    public override void OnBankrollEffect(GameObject target)
    {
        //�R�C���ɓ���������̏���
        if (state == GamblePinState.Coin)
        {
            _coinPinHitCount++;
            //���������񐔂�\��
            text.gameObject.SetActive(true);
            text.text = _coinPinHitCount.ToString();
            int _coinFlipResult = Random.Range(0, 2);
            Debug.Log(_coinFlipResult + "���o��");
            if (_coinFlipResult == 0)
            {
                //�������炷����
                _moneyManager.DecreaseMoney(_getCoin);
                Debug.Log($"{_getCoin}�~�������B");
                //�͂���̃e�L�X�g��\��
                hitormiss.gameObject.SetActive(true );
                hitormiss.text = "�͂���";
            }
            else if (_coinFlipResult == 1)
            {
                //���𑝂₷����
                _moneyManager.AddMoney(_getCoin);
                Debug.Log($"{_getCoin}�~��ɓ��ꂽ");
                //������̃e�L�X�g�\��
                hitormiss.gameObject.SetActive(true);
                hitormiss.text = "������";
            }

            if (_coinPinHitCount == _diceChangeCount)
            {
                //��Ԃ��_�C�X�ɕύX
                state = GamblePinState.Dice;
                hitormiss.gameObject.SetActive(false);
                text.gameObject.SetActive(false);
                //�f�o�b�N�p�ɂ킩��₷�����Ă��邾���Ȃ̂ŁAprefab���ꂽ��R�����g�A�E�g���Ă���đ��v
                GetComponent<Renderer>().material.color = Color.red;
                _coinPinHitCount = 0;
            }
        }
        //�_�C�X�ɓ���������̏���
        else
        {
            int _rolledDiceNumber = Random.Range(0, _moneyMultiplierFromDice.Length);
            Debug.Log(_rolledDiceNumber+1 + "���o��");

            _moneyManager.MultiplicationMoney(_moneyMultiplierFromDice[_rolledDiceNumber]);
            Debug.Log($"��������{_rolledDiceNumber}�{���ꂽ");

            state = GamblePinState.Coin;
            //�f�o�b�N�p�ɂ킩��₷�����Ă��邾���Ȃ̂ŁAprefab���ꂽ��R�����g�A�E�g���Ă���đ��v
            GetComponent<Renderer>().material.color = Color.blue;

            
        }
    }

}
