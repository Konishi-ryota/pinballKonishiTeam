using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplierBankroll : BankrollBase
{
    [Header("当たった時に展開されるフィールドプレハブ")]
    [SerializeField] private GameObject _fieldObject;
    //private GameObject _bankrollParent;
    public override void OnBankrollEffect(GameObject ballObject)
    {
        GameObject newField = Instantiate(_fieldObject);
        newField.transform.parent = _bankrollParent.transform;
        Transform[] children = _bankrollParent.GetComponentsInChildren<Transform>();
        Vector3 randomPosition = children[Random.Range(0, children.Length - 1)].position;
        newField.transform.localPosition = new Vector3(randomPosition.x, _bankrollParent.transform.localPosition.y, randomPosition.z);
        newField.transform.localEulerAngles = _bankrollParent.transform.localEulerAngles;
    }

}
