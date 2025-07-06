using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBankroll : BankrollBase
{
    public override void OnBankrollEffect(GameObject ballObject)
    {
        if (ballObject.TryGetComponent<BallEffectBehaviour>(out var component))
        {
            component.SetBallEffect(BallEffect.Bomb);
        }
    }
}
