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
        Debug.Log("AAAA");
        for (int i = 0; i < _coinScatterCounts; i++)
        {
            GameObject newCoin = Instantiate(_coinPrefab);
            newCoin.transform.position = _clonePosition.position;
            Vector3 randomPosition = Vector3.zero;
            do
            {
                randomPosition = new Vector3(Random.Range(0f, 4f), Random.Range(0.5f, 1f), Random.Range(0f, 3f));

            } while (Vector3.Distance(new Vector3(0, 0, 0), randomPosition) < 1.7f);
            Debug.Log(randomPosition);
            Vector3 force = this.transform.TransformPoint(randomPosition);
            //force.Normalize();
            Rigidbody coinRig = newCoin.GetComponent<Rigidbody>();
            coinRig.velocity = force * _scatteredPower;

        }
    }
}
