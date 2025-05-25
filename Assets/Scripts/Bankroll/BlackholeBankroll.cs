using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackholeBankroll : BankrollBase
{
    [Header("吸引の強さ")]
    [SerializeField] private float _inhalePower = 10f;
    [Header("ボールが当たった時に増えるお金")]
    [SerializeField] private int _addMoneyOnHit = 200;
    //[Header("効果範囲")]
    //[SerializeField] private float _effectRadius = 5f;

    private MoneyManager _moneyManager;
    private List<Rigidbody> _ballsInRange = new List<Rigidbody>();

    void Start()
    {
        _moneyManager = GameObject.FindAnyObjectByType<MoneyManager>();
        StartCoroutine(InhaleBallOnRange());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Rigidbody ballRb = other.attachedRigidbody;
            if (ballRb != null && !_ballsInRange.Contains(ballRb))
            {
                _ballsInRange.Add(ballRb);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Rigidbody ballRb = other.attachedRigidbody;
            if (ballRb != null)
            {
                _ballsInRange.Remove(ballRb);
            }
        }
    }

    /// <summary>
    /// 効果範囲内のボールをバンクロールの方向に吸い寄せる
    /// </summary>
    private IEnumerator InhaleBallOnRange()
    {
        while (true)
        {
            foreach (var ball in _ballsInRange)
            {
                if (ball != null)
                {
                    Vector3 direction = (transform.position - ball.position).normalized;
                    ball.AddForce(direction * _inhalePower, ForceMode.Acceleration);
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public override void OnBankrollEffect(GameObject ballObject)
    {
        _moneyManager?.AddMoney(_addMoneyOnHit);
        if (_ballRigidbody != null)
        {
            Destroy(_ballRigidbody.gameObject);
        }
    }
}
