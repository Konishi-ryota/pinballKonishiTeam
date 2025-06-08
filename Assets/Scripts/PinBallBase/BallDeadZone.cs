using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDeadZone : MonoBehaviour
{
    private MoneyManager _moneyManager;
    //TODO:値は決まってないから仮置き中
    [SerializeField] int _addMoneyOnBallDrop = 100;//ボールが落下した時に手に入るお金
    private void Awake()
    {
        _moneyManager = GameObject.FindAnyObjectByType<MoneyManager>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        //ボールがあたり判定に入ったら消す
        if (other.gameObject.CompareTag("Ball"))
        {
            Destroy(other.gameObject);
            _moneyManager.AddMoney(_addMoneyOnBallDrop);

            FieldControl[] fields = FindObjectsOfType<FieldControl>();
            foreach (FieldControl field in fields)
            {
                field.AddTurnCount();
            }
        }
    }
}
