using System.Collections;
using UnityEngine;

/// <summary>
/// ボールのエフェクトを保持するクラス。
/// </summary>
public class BallEffectBehaviour : MonoBehaviour
{
    private BallEffect _effect;
    public BallEffect Effect { get => _effect; }

    [SerializeField] private GameObject _fireEffectPrefab;
    //爆弾の見た目
    [SerializeField] private GameObject _bombEffectPrefab;
    //爆発時のエフェクト
    [SerializeField] private GameObject _explosionEffectPrefab;
    [Header("Bomb Settings")]
    [SerializeField] private float _explosionDelay = 3.0f;
    [SerializeField] private float _explosionRadius = 3.0f;

    private GameObject _activeEffect;
    private Coroutine _bombCoroutine;

    private Renderer _renderer;
    [Header("爆発でお金が増える数")]
    [SerializeField] private int _addBombMoney = 500;
    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }
    public void SetBallEffect(BallEffect ballEffect)
    {
        if (_effect != ballEffect)
        {
            _effect = ballEffect;
            //重複防止
            if (_activeEffect == null)
            {
                Destroy(_activeEffect);
                _activeEffect = null;
                if (ballEffect == BallEffect.Fire)
                {
                    Instantiate(_fireEffectPrefab, transform);
                }
                else if (ballEffect == BallEffect.Bomb)
                {
                    if (_bombEffectPrefab)
                        Instantiate(_bombEffectPrefab, transform);

                    //デバック用に色を黒にしている。
                    _renderer.material.color = Color.black;
                    _bombCoroutine = StartCoroutine(BombCountdown());
                }
            }
        }
    }
    private IEnumerator BombCountdown()
    {
        // 点滅開始の1秒前まで待つ
        yield return new WaitForSeconds(_explosionDelay - 1f);

        // 点滅処理（1秒間に複数回点滅）
        float blinkDuration = 1f;      // 点滅全体の時間
        float blinkInterval = 0.2f;    // 1回の点滅周期（0.2秒で赤⇔黒）
        float elapsed = 0f;
        bool isRed = false;

        while (elapsed < blinkDuration)
        {
            if (_renderer != null)
            {
                _renderer.material.color = isRed ? Color.black : Color.red;
            }

            isRed = !isRed;
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        // 爆発処理
        Explode();
    }
    /// <summary>
    /// 爆発時に周囲のピンから金額を取得し、爆発演出を行う
    /// </summary>
    private void Explode()
    {
        //デバック用に色を白にしている
        _renderer.material.color = Color.white;
        
        //爆発エフェクトを表示
        if (_explosionEffectPrefab != null)
        {
            Instantiate(_explosionEffectPrefab, transform.position, Quaternion.identity);
            
            
        }
        // 爆風の範囲内にあるオブジェクトを取得
        Collider[] hits = Physics.OverlapSphere(transform.position, _explosionRadius);

        
        // MoneyManagerの参照を取得
        MoneyManager moneyManager = GameObject.FindObjectOfType<MoneyManager>();
        if (moneyManager == null)
        {
            Debug.LogWarning("MoneyManagerが見つからなかったため、金額加算されませんでした。");
        }

        // 爆風で得た金額合計
        int totalMoney = 0;

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<BankrollBase>(out var bankroll))
            {
                totalMoney += _addBombMoney;
                //増えるお金の変数名がそれぞれ違い大変面倒＋拡張性の問題のため爆発範囲の効果を発動させることにする
                bankroll.OnBankrollEffect(gameObject);
            }
        }

        // 金額を加算
        if (totalMoney > 0 && moneyManager != null)
        {
            moneyManager.AddMoney(totalMoney);
            Debug.Log($"爆発で {totalMoney} 円獲得！");
        }
        

        

        _effect = BallEffect.None;
    }
}
/// <summary>
/// 一定時間後に爆発するためのコルーチン
/// </summary>

public enum BallEffect
{
    None,
    Fire,
    Bomb,
}