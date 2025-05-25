
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoubleAroundBankroll : BankrollBase
{
    [Header("効果を適応する距離半径")]
    [SerializeField] 
    private float _radius;
    
    void Update()
    {
        var results = Physics.OverlapSphere(transform.position, _radius);
        foreach (var content in results)
        {
            if (!content.TryGetComponent<DoubleAroundBankrollEffect>(out _))
            {
                var effect = content.AddComponent<DoubleAroundBankrollEffect>();
                effect.Init(this);
            }
        }
    }

    public override void OnBankrollEffect(GameObject _)
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
