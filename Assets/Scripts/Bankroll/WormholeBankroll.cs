using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Profiling;

public class WormholeBankroll : BankrollBase, IPlaceCallBack
{
    [Header("飛ばす力の強さ"),SerializeField]float forceMagnitude = 1f;
    [Header("ボールが出てくるまでの時間"), SerializeField] float _delayTime;

    private WormholeManager _wormholeManager;
    private SoundManager _soundManager = null;
    private List<WormholeBankroll> otherWormhole = new();

    // Start is called before the first frame update
    void Start()
    {
        _wormholeManager = FindAnyObjectByType<WormholeManager>();
        _wormholeManager.BankrollList.Add(this);
        otherWormhole = _wormholeManager.BankrollList;
    }

    public override void OnBankrollEffect(GameObject ballObject)
    {
        
    }
    public void OnPlaced()
    {
        _soundManager = FindAnyObjectByType<SoundManager>();
        Debug.Log("置かれた");
        _soundManager.PlaySE(SESoundData.SE.WarpSet);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            _soundManager = FindAnyObjectByType<SoundManager>();
            GameObject ball = other.gameObject;
            SetBallVisible(ball, false);
            otherWormhole.Remove(this); // 一時的に除外

            if (otherWormhole.Count == 0)
            {
                // 他にワームホールが無い場合、ここでワープ
                StartCoroutine(DelayedTeleportToSelf(ball));
                _soundManager.PlaySE(SESoundData.SE.Warping);
            }
            else
            {
                // ランダムな他のワームホールへワープ
                WormholeBankroll exit = otherWormhole[Random.Range(0, otherWormhole.Count)];
                StartCoroutine(DelayedExit(ball, exit));
                _soundManager.PlaySE(SESoundData.SE.Warping);
            }
        }
    }

    private IEnumerator DelayedExit(GameObject ball, WormholeBankroll exit)
    {
        yield return new WaitForSeconds(_delayTime);
        ball.transform.position = exit.transform.position + new Vector3(0, 1);
        ExistBall(ball);

        // 自分をリストに戻す
        if (!_wormholeManager.BankrollList.Contains(this))
        {
            _wormholeManager.BankrollList.Add(this);
        }
    }

    private IEnumerator DelayedTeleportToSelf(GameObject ball)
    {
        yield return new WaitForSeconds(_delayTime);
        ball.transform.position = this.transform.position + new Vector3(0, 1);
        ExistBall(ball);

        if (!_wormholeManager.BankrollList.Contains(this))
        {
            _wormholeManager.BankrollList.Add(this);
        }
    }

    private void ExistBall(GameObject ball)
    {
        SetBallVisible(ball, true);
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 randomDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
            Debug.Log($"{randomDir}");
            rb.AddForce(randomDir * forceMagnitude * 500);
        }
    }
    private void SetBallVisible(GameObject ball, bool visible)
    {
        Renderer[] renderers = ball.GetComponentsInChildren<Renderer>();// MeshRenderer を無効化／有効化
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = visible;
        }

        Rigidbody rb = ball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = !visible; // 非表示中は動かないように
        }
    }

    private void OnDestroy()
    {
        _wormholeManager.BankrollList.Remove(this);
    }
}

