using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GateColliderController : MonoBehaviour
{
    private BoxCollider _boxCollider;
    private PlungerTrigger _plungerTrigger;
    private void Start()
    {
        _boxCollider = this.GetComponent<BoxCollider>();
        _boxCollider.isTrigger = true;
        _plungerTrigger = GameObject.FindObjectOfType<PlungerTrigger>();
    }

    private void OnTriggerExit(Collider other)
    {
        _boxCollider.isTrigger = false;
        _plungerTrigger.PassGate();
    }

    /// <summary>
    /// ゲートのあたり判定をリセットする関数
    /// </summary>
    public void ResetGate()
    {
        _boxCollider.isTrigger = true;
    }
}
