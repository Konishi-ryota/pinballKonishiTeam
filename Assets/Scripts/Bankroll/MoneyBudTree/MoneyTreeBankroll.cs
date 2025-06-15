using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class MoneyTreeBankroll : MoneyGainBankrollBase
{
    [Header("芽バンクロール")]
    [SerializeField] private GameObject _moneyBudBankroll;
    [Header("破壊するまでの回数")]
    [SerializeField] private int _breakCount = 50;
    [Header("破壊するときのお金")]
    [SerializeField] private int _brakeGetMoney = 25000;
    [Header("コインプレハブ")]
    [SerializeField] private GameObject _coinPrefab;
    [Header("コインをばら撒く数")]
    [SerializeField] private int _coinScatterCounts = 10;
    [Header("散らばる強さ")]
    [SerializeField] private float _scatteredPower = 1;
    [Header("コインを生成する位置")]
    [SerializeField] private Transform _clonePosition;
    private int _hitCount = 0;
    private BuildingPlacer _buildingPlacer;
    public override void OnBankrollHit(GameObject ballObject)
    {
        _hitCount++;
        if (_hitCount >= _breakCount)
        {
            BreakTree();
        }
    }
    private void BreakTree()
    {
        if (_moneyBudBankroll == null)
        {
            Debug.LogError("_moneyBudBankroll Null");
            return;
        }
        GameObject newBankroll = Instantiate(_moneyBudBankroll);
        newBankroll.transform.position = this.transform.position;
        newBankroll.transform.rotation = this.transform.rotation;
        _buildingPlacer = FindObjectOfType<BuildingPlacer>();
        newBankroll.transform.parent = _buildingPlacer.GetBankrollParent().transform;
        ScatterCoins();
        GainMoney(_brakeGetMoney);
        Destroy(this.gameObject);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            ScatterCoins();
        } 
    }
    private void ScatterCoins()
    {
        for (int i = 0; i < _coinScatterCounts; i++)
        {
            GameObject newCoin = Instantiate(_coinPrefab);
            newCoin.transform.position = _clonePosition.position;
            _clonePosition.Rotate(0, Random.Range(10f, 100f) ,0);
            Vector3 force = _clonePosition.transform.forward;
            Rigidbody coinRig = newCoin.GetComponent<Rigidbody>();
            coinRig.AddForce(force * _scatteredPower, ForceMode.VelocityChange);
            //coinRig.velocity = force * _scatteredPower;

        }
    }
}
