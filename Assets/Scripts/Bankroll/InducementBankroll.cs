using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionBankroll : BankrollBase
{
	[Header("ピン前方向(Z)を軸に±{_angleRange}°を上範囲：初期30°")]
	[SerializeField] float _angleUprRange = 30f;
	//上範囲の角度を下角度に流用(Startで代入)
	float _angleLwrRange;

	[Header("スピード倍率[1.0～1.2推奨]")]
	[SerializeField] float _addSpeed = 1.1f;

	// Start is called before the first frame update
	void Start()
	{
		_angleLwrRange = 180 - _angleUprRange;
	}

	public override void OnBankrollEffect(GameObject ballObject)
	{
		//変更する方向を格納
		Vector3 toward = Vector3.zero;

		//方向の変更と速度の抽出
		Rigidbody rbBall = ballObject.GetComponent<Rigidbody>();
		//ボールとピンの2点間の距離を出す
		Vector3 contactVec = ballObject.transform.position - this.transform.position;
		//高さを揃えるため、Y座標を0にする(Y座標の差をなくす)
		contactVec.y = 0f;
		//ピンのZ軸(0,0,1)とのなす角を出す
		float betweenAngle = Vector3.Angle(Vector3.forward, contactVec.normalized);

		Debug.Log(betweenAngle);

		//上下判定(デフォルト±30°)
		if (betweenAngle <= _angleUprRange) 
		{
			Debug.Log("上判定");
			toward = Vector3.left;
		}
		else if (betweenAngle >= _angleLwrRange) 
		{
			Debug.Log("下判定");
			toward = Vector3.right;
		}

		//左右判定
		else
		{
			if (contactVec.x < 0)
			{
				Debug.Log("左判定");
				toward = -(Vector3.forward);
			}
			else if (contactVec.x > 0)
			{
				Debug.Log("右判定");
				toward = Vector3.forward;
			}
			else 
			{
				Debug.Log("範囲がとれていません");
			}
		}

		//ボールの指定方向に、ボールの速度に倍率をかけたものを出力
		rbBall.velocity = toward * rbBall.velocity.magnitude * _addSpeed;
	}
}