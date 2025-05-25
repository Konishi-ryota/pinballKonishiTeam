using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlungerTrigger : MonoBehaviour
{

    [Header("打つのに必要な最低限のお金")] 
    [SerializeField] private int _minimumRequiredMoney = 100;
    [Header("パワーが最大になる時間")] 
    [SerializeField] private float _maxPowerTime = 5.0f;
    [Header("時間ごとのパワー")] 
    [SerializeField] private List<float> _powerList = new List<float>();

    [Header("自動パワー設定")]
    [SerializeField] private int _powerStageCount = 20;
    [SerializeField] private float _minPower = 100;
    [SerializeField] private float _maxPower = 1000;

    private MoneyManager _moneyManager;
    private float _currentPower;
    private Rigidbody _rigidbody;
    private float _currentTime;
    private Vector3 _resetPos;
    private float _powerDivisionUnit;
    public bool IsBall;
    private bool _isPush;
    
    
    private void Start()
    {
        if (_powerList.Count < _powerStageCount)
        {
            SetPower();
            Debug.LogWarning("パワーが設定されていません 自動で設定いたします。");
        }
        _currentPower = _powerList[0];
        _currentTime = 0;
        _resetPos = this.transform.position;
        _powerDivisionUnit = _maxPowerTime / _powerStageCount;
        _moneyManager = GameObject.FindObjectOfType<MoneyManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        FlickBall();
    }

    private void FlickBall()
    {
        if (_rigidbody != null)
        {
            //押している間の処理
            if (Input.GetKey(KeyCode.Space) && CheckMoney())
            {
                _isPush = true;
                _currentTime += Time.deltaTime;
                if (_currentTime <= _maxPowerTime)
                {
                    int powerIndex = (int)(_currentTime / _powerDivisionUnit);
                    _currentPower = _powerList[powerIndex];
                    Debug.Log(powerIndex);
                }
                else
                {
                    _currentPower = _powerList[_powerStageCount - 1];
                }
            }
            
            //離した時の処理
            if (Input.GetKeyUp(KeyCode.Space) && CheckMoney())
            {
                _rigidbody.AddForce(_currentPower * Vector3.forward, ForceMode.Impulse);
                _isPush = false;
                Debug.Log(_currentPower);
            }
        }
        else
        {
            _isPush = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            _rigidbody = other.GetComponent<Rigidbody>();
            IsBall = true;
        }
    }

    //お金が最低値よりあるか
    private bool CheckMoney()
    {
        int money = _moneyManager.Money;
        if (money >= _minimumRequiredMoney)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// ゲートを通過したときに呼ばれる処理
    /// </summary>
    public void PassGate()
    {
        _moneyManager.DecreaseMoney(_minimumRequiredMoney);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            _rigidbody = null;
            _currentPower = _powerList[0];
            _currentTime = 0;
            IsBall = false;
        }
    }

    /// <summary>
    /// ボールを初期位置に戻す
    /// </summary>
    public void ResetPositionBall()
    {
        Rigidbody rb = GameObject.FindWithTag("Ball").GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        rb.position = _resetPos;
        rb.isKinematic = false;
    }
    
    /// <summary>
    /// 発射時パワーの自動設定
    /// </summary>
    public void SetPower()
    {
        _powerList.Clear();

        for (int i = 0; i < _powerStageCount; i++)
        {
            _powerList.Add(Mathf.Lerp(_minPower, _maxPower, (float)i / (_powerStageCount - 1)));
        }
    }
}
