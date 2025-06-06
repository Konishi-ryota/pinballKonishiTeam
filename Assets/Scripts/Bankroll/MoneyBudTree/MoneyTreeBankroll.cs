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
    [Header("コインをばら撒く方向")]
    [SerializeField] private List<Transform> _coinScatterPositions;
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
        Debug.Log("破壊");
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
    private void ScatterCoins()
    {
        foreach (Transform scatterPosition in _coinScatterPositions)
        {
            GameObject newCoin = Instantiate(_coinPrefab);
            newCoin.transform.position = _clonePosition.position;
            Vector3 force = scatterPosition.position - _clonePosition.position;
            Debug.Log(force);
            force = RandomPosition(force);
            Rigidbody coinRig = newCoin.GetComponent<Rigidbody>();
            coinRig.velocity = force * _scatteredPower;

        }
    }
    public Vector3 RandomPosition(Vector3 position)
    {
        Vector3 newPosition;
        Vector3 angle = _clonePosition.localEulerAngles;
        angle.y += Random.Range(0f, 90f);
        _clonePosition.localEulerAngles = angle;
        newPosition = _clonePosition.transform.forward;
        newPosition.y = Random.Range(1f, 2f);
        return newPosition;

    }
}
