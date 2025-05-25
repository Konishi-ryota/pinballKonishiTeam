using UnityEngine;

public class RepulsionBankroll : BankrollBase
{
    [Header("反射する強さ（倍率）")] 
    [SerializeField] private float _reflectorMultiplier = 2.0f;
    

    public override void OnBankrollEffect(GameObject ballObject)
    {
        if (_ballRigidbody != null)
        {
            Vector3 reflectedVelocity = Vector3.Reflect(_ballRigidbody.velocity, _hitPos);

            _ballRigidbody.velocity = reflectedVelocity * _reflectorMultiplier;
        }

        _ballRigidbody = null;
    }
}
