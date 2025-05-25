using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipperController : MonoBehaviour
{
    [Header("フリッパーの左右")] [SerializeField] private bool _isLeft;
    [Header("角度の最大値")] 
    [SerializeField] private float _maxAngle;
    [Header("角度の最小値")] 
    [SerializeField] private float _minAngle;

    private HingeJoint _hingeJoint;
    private JointSpring _jointSpring;
    
    private void Start()
    {
        _hingeJoint = GetComponent<HingeJoint>();
        _jointSpring = _hingeJoint.spring;
        Init();
    }

    //初期化処理
    private void Init()
    {
        JointLimits limits = _hingeJoint.limits;
        limits.min = _minAngle;
        limits.max = _maxAngle;
        _hingeJoint.limits = limits;
        _hingeJoint.useLimits = true;

        _jointSpring.targetPosition = _isLeft ? -_minAngle : -_minAngle;
        _hingeJoint.spring = _jointSpring;
    }
    private void Update()
    {
        ActiveFlipper();
    }

    //フリッパーの処理
    private void ActiveFlipper()
    {
        if (_isLeft)
        {
            if(Input.GetKey(KeyCode.A))
            {
                _jointSpring.targetPosition = -_maxAngle;
            }
            else
            {
                _jointSpring.targetPosition = -_minAngle;
            }

            _hingeJoint.spring = _jointSpring;
        }
        else
        {
            if(Input.GetKey(KeyCode.D))
            {
                _jointSpring.targetPosition = _maxAngle;
            }
            else
            {
                _jointSpring.targetPosition = _minAngle;
            }
            
            _hingeJoint.spring = _jointSpring;
        }
    }
}
