using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldHitCheck : MonoBehaviour
{
    public GameObject fieldObject;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<MoneyGainBankrollBase>().SetMoneyMultiplier(2);//î{ó¶ÇÇQî{Ç…ïœçX
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball") && fieldObject != null)
        {
            fieldObject.GetComponent<FieldControl>().Delete();
        }
    }  
    public void OnTriggerEnter(Collider other)
    {
        if (fieldObject != null)
        {
            if (other.gameObject.GetComponent<FieldControl>() != null)
            {
                fieldObject = other.gameObject;
            }
        }
    }
}
