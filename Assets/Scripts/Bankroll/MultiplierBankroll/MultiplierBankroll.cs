using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplierBankroll : BankrollBase
{
    [Header("�����������ɓW�J�����t�B�[���h�v���n�u")]
    [SerializeField] private GameObject _fieldObject;
    [Header("�t�B�[���h�𐶐�����͈̓I�u�W�F�N�g�̖��O")]
    [SerializeField] private string _startPositionObjName;
    [SerializeField] private string _goalPositionObjName;
    private GameObject _startPosition;
    private GameObject _goalPosition;
    public override void OnBankrollEffect(GameObject ballObject)
    {
        _startPosition = GameObject.Find(_startPositionObjName);
        _goalPosition = GameObject.Find(_goalPositionObjName);

        GameObject newField = Instantiate(_fieldObject);
        float randomX = Random.Range(_startPosition.transform.localPosition.x, _goalPosition.transform.localPosition.x);
        float randomZ = Random.Range(_startPosition.transform.localPosition.z, _goalPosition.transform.localPosition.z);
        newField.transform.parent = _startPosition.transform.parent;
        newField.transform.localPosition = new Vector3(randomX, _startPosition.transform.localPosition.y, randomZ);
        newField.transform.localEulerAngles = _startPosition.transform.localEulerAngles;
    }

}
