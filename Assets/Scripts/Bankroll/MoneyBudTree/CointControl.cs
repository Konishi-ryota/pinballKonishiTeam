using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CointControl : MoneyGainBankrollBase
{
    [Header("ステージから浮かせる距離")]
    [SerializeField] private float _fieldDistance;
    [Header("コインをひろったときのお金")]
    [SerializeField] private int _getMoney = 100;
    [Header("消えるまでの時間")]
    [SerializeField] private float _destroyTimer = 30;
    private float _timer = 0;
    private void Update()
    {
        Ray ray = new Ray(this.transform.position, new Vector3(0, -1, 0));
        RaycastHit hit;
        Debug.DrawRay(this.transform.position, new Vector3(0, -1, 0), Color.blue);
        if (Physics.Raycast(ray, out hit, 3))
        {
            if (hit.collider.gameObject.name == "Table" && this.transform.position.y <= hit.point.y + _fieldDistance + 0.5f)
            {
                Vector3 vector = this.transform.position;
                vector.y = hit.point.y + _fieldDistance + 0.5f;
                this.transform.position = vector;
                this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            }
        }

        if (_timer >= _destroyTimer)
        {
            Destroy(this.gameObject);
        }
        _timer += Time.deltaTime;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            GainMoney(_getMoney);
            Destroy(this.gameObject);
        }
    }
    public override void OnBankrollHit(GameObject ballObject) { }
}

