using UnityEngine;

/// <summary>
/// 衝突したボールに火属性のエフェクトを付与するバンクロール。
/// </summary>
public class FireEffectBankroll : BankrollBase
{
    public override void OnBankrollEffect(GameObject ballObject)
    {
        if (ballObject.TryGetComponent<BallEffectBehaviour>(out var component))
        {
            component.SetBallEffect(BallEffect.Fire);
        }
    }
}
