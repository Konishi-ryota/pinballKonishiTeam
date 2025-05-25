using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    [Header("初期位置")]
    [SerializeField] private Transform _ballInitialTransform;
    [Header("ボールのプレハブ")] 
    [SerializeField] private GameObject _ballPrefab;

    private int _ballCount;
    private GateColliderController _gateColliderController;

    private void Start()
    {
        _gateColliderController = GameObject.FindObjectOfType<GateColliderController>();
        CheckBall();
    }

    private void CheckBall()
    {
        if (_ballCount == 0)
        {
            ResetBall();
        }
    }

    /// <summary>
    /// ボールカウントを増やす
    /// </summary>
    public void AddBallCount()
    {
        _ballCount++;
    }

    /// <summary>
    /// ボールカウントを減らす
    /// </summary>
    public void DecreaseBallCount()
    {
        _ballCount--;
        CheckBall();
    }
    
    //ボールを初期位置に戻す処理
    private void ResetBall()
    {
#if UNITY_EDITOR
        if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode) return;
#endif
        _gateColliderController.ResetGate();
        GameObject ball = Instantiate(_ballPrefab, _ballInitialTransform.position, Quaternion.identity);
        ball.transform.SetParent(this.gameObject.transform, false);
        AddBallCount();
    }
}
