using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionBankroll : BankrollBase
{
	[Header("�s���O����(Z)�����Ɂ}{_angleRange}������͈́F����30��")]
	[SerializeField] float _angleUprRange = 30f;
	//��͈͂̊p�x�����p�x�ɗ��p
	float _angleLwrRange;

	[Header("�X�s�[�h�{��[1.0�`1.2����]")]
	[SerializeField] float _addSpeed = 1.1f;

	// Start is called before the first frame update
	void Start()
	{
		_angleLwrRange = 180 - _angleUprRange;
	}

	// Update is called once per frame
	void Update()
	{

	}

	public override void OnBankrollEffect(GameObject ballObject)
	{
		//�ύX����������i�[
		Vector3 toward = Vector3.zero;

		//�����̕ύX�Ƒ��x�̒��o
		Rigidbody rbBall = ballObject.GetComponent<Rigidbody>();
		//�{�[���ƃs����2�_�Ԃ̋������o��
		Vector3 contactVec = ballObject.transform.position - this.transform.position;
		//�����𑵂��邽�߁AY���W��0�ɂ���
		contactVec.y = 0f;
		//�s����Z��(0,0,1)�Ƃ̂Ȃ��p���o��
		float betweenAngle = Vector3.Angle(Vector3.forward, contactVec.normalized);

		Debug.Log(betweenAngle);

		if (betweenAngle <= _angleUprRange) 
		{
			Debug.Log("�㔻��");
			toward = Vector3.left;
		}

		else if (betweenAngle >= _angleLwrRange) 
		{
			Debug.Log("������");
			toward = Vector3.right;
		}

		//���E����
		else
		{
			if (contactVec.x < 0)
			{
				Debug.Log("������");
				toward = -(Vector3.forward);
			}
			else if (contactVec.x > 0)
			{
				Debug.Log("�E����");
				toward = Vector3.forward;
			}
			else 
			{
				Debug.Log("�͈͂��Ƃ�Ă��܂���");
			}
		}

		rbBall.velocity = toward * rbBall.velocity.magnitude * _addSpeed;
	}
}