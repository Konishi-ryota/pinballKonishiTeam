using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DuplicateBankroll : BankrollBase
{
    [Header("ボールのプレハブ")]
    [SerializeField] private GameObject _ballPrefab;
    [Header("ボールの分裂する個数")] 
    [SerializeField, Range(0, 3)] private int _duplicateCount;
    [Header("射出する強さ")] 
    [SerializeField] private float _shootPower = 10f;
    [Header("クールタイム")] 
    [SerializeField] private float _intervalTime = 10f;
    [Header("複製する位置")] 
    [SerializeField] private Transform[] _duplicatePos;

    private float _timer;
    private BallManager _ballManager;

    private void Start()
    {
        _ballManager = GameObject.FindObjectOfType<BallManager>();
        _timer = _intervalTime;
    }
    private void Update()
    {
        _timer += Time.deltaTime;
    }
    
    public override void OnBankrollEffect(GameObject ballObject)
    {
        if (_timer > _intervalTime)
        {
            // 最も近い複製位置を求める
            Transform closestPosition = null;
            float closestDistance = float.MaxValue;
            foreach (var pos in _duplicatePos)
            {
                float distance = Vector3.Distance(_hitPos, pos.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPosition = pos;
                }
            }

            // 最も近い位置を除いた位置をランダムで選択
            List<Transform> availablePositions = new List<Transform>(_duplicatePos);
            availablePositions.Remove(closestPosition); // 最も近い位置を除外

            for (int i = 0; i < _duplicateCount; i++)
            {
                // ランダムで位置を選ぶ
                Transform randomPosition = availablePositions[Random.Range(0, availablePositions.Count)];

                // ボールを指定された位置で生成
                GameObject newBall = Instantiate(_ballPrefab, randomPosition.position, randomPosition.rotation);
                
                // ボールに射出の力を加える
                Rigidbody rb = newBall.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce(newBall.transform.forward * _shootPower, ForceMode.Impulse); // 射出力を加える
                    availablePositions.Remove(randomPosition);
                }
                
                //ボール生成時にカウントする
                _ballManager.AddBallCount();
            }

            _timer = 0;
        }
        
    }
}
