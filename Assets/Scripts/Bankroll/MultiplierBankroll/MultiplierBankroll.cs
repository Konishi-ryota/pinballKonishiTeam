using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplierBankroll : BankrollBase
{
    [Header("当たった時に展開されるフィールドプレハブ")]
    [SerializeField] private GameObject _fieldObject;
    private SoundManager _soundManager;
    //private GameObject _bankrollParent;
    public void Awake()
    {
        _soundManager = FindAnyObjectByType<SoundManager>();
    }
    public override void OnBankrollEffect(GameObject ballObject)
    {
        GameObject newField = Instantiate(_fieldObject);
        newField.transform.parent = this.transform.parent.gameObject.transform;
        Transform[] children = this.transform.parent.gameObject.GetComponentsInChildren<Transform>();
        Vector3 randomPosition = children[Random.Range(0, children.Length - 1)].position;
        newField.transform.localPosition = new Vector3(randomPosition.x, this.transform.parent.gameObject.transform.localPosition.y, randomPosition.z);
        newField.transform.localEulerAngles = this.transform.parent.gameObject.transform.localEulerAngles;
        _soundManager.PlaySE(SESoundData.SE.BaikaHatudou);
    }

}
