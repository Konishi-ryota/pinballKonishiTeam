using System.Collections;
using UnityEngine;

/// <summary>
/// �{�[���̃G�t�F�N�g��ێ�����N���X�B
/// </summary>
public class BallEffectBehaviour : MonoBehaviour
{
    private BallEffect _effect;
    public BallEffect Effect { get => _effect; }

    [SerializeField] private GameObject _fireEffectPrefab;
    //���e�̌�����
    [SerializeField] private GameObject _bombEffectPrefab;
    //�������̃G�t�F�N�g
    [SerializeField] private GameObject _explosionEffectPrefab;
    [Header("Bomb Settings")]
    [SerializeField] private float _explosionDelay = 3.0f;
    [SerializeField] private float _explosionRadius = 3.0f;

    private GameObject _activeEffect;
    private Coroutine _bombCoroutine;

    private Renderer _renderer;
    [Header("�����ł����������鐔")]
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
            //�d���h�~
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

                    //�f�o�b�N�p�ɐF�����ɂ��Ă���B
                    _renderer.material.color = Color.black;
                    _bombCoroutine = StartCoroutine(BombCountdown());
                }
            }
        }
    }
    private IEnumerator BombCountdown()
    {
        // �_�ŊJ�n��1�b�O�܂ő҂�
        yield return new WaitForSeconds(_explosionDelay - 1f);

        // �_�ŏ����i1�b�Ԃɕ�����_�Łj
        float blinkDuration = 1f;      // �_�őS�̂̎���
        float blinkInterval = 0.2f;    // 1��̓_�Ŏ����i0.2�b�Őԁ̍��j
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

        // ��������
        Explode();
    }
    /// <summary>
    /// �������Ɏ��͂̃s��������z���擾���A�������o���s��
    /// </summary>
    private void Explode()
    {
        //�f�o�b�N�p�ɐF�𔒂ɂ��Ă���
        _renderer.material.color = Color.white;
        
        //�����G�t�F�N�g��\��
        if (_explosionEffectPrefab != null)
        {
            Instantiate(_explosionEffectPrefab, transform.position, Quaternion.identity);
            
            
        }
        // �����͈͓̔��ɂ���I�u�W�F�N�g���擾
        Collider[] hits = Physics.OverlapSphere(transform.position, _explosionRadius);

        
        // MoneyManager�̎Q�Ƃ��擾
        MoneyManager moneyManager = GameObject.FindObjectOfType<MoneyManager>();
        if (moneyManager == null)
        {
            Debug.LogWarning("MoneyManager��������Ȃ��������߁A���z���Z����܂���ł����B");
        }

        // �����œ������z���v
        int totalMoney = 0;

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<BankrollBase>(out var bankroll))
            {
                totalMoney += _addBombMoney;
                //�����邨���̕ϐ��������ꂼ��Ⴂ��ϖʓ|�{�g�����̖��̂��ߔ����͈͂̌��ʂ𔭓������邱�Ƃɂ���
                bankroll.OnBankrollEffect(gameObject);
            }
        }

        // ���z�����Z
        if (totalMoney > 0 && moneyManager != null)
        {
            moneyManager.AddMoney(totalMoney);
            Debug.Log($"������ {totalMoney} �~�l���I");
        }
        

        

        _effect = BallEffect.None;
    }
}
/// <summary>
/// ��莞�Ԍ�ɔ������邽�߂̃R���[�`��
/// </summary>

public enum BallEffect
{
    None,
    Fire,
    Bomb,
}