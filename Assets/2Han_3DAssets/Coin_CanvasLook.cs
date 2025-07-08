using UnityEngine;

public class Coin_CanvasLook : MonoBehaviour
{
    void LateUpdate()
    {
        transform.forward = Camera.main.transform.forward;//このオブジェクトの向きを固定する
    }
}
