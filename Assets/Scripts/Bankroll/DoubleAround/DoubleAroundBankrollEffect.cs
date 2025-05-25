using System;
using UnityEngine;

public class DoubleAroundBankrollEffect : MonoBehaviour
{
    private MoneyGainBankrollBase _bankroll;
    private DoubleAroundBankroll _parent;

    private void Start()
    {
        if (_bankroll == null)
            Init();
    }

    public void Init(DoubleAroundBankroll parent = null)
    {
        if(parent is not null)
            _parent = parent;

        if (_bankroll == null)
        {
            _bankroll = GetComponent<MoneyGainBankrollBase>();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_parent == null)
        {
            Destroy(this);
            return;
        }
        
        if (!other.gameObject.CompareTag("Ball"))
            return;
        
        if(_bankroll == null)
            Init();
        _bankroll?.OnBankrollEffect(other.gameObject);
    }
}