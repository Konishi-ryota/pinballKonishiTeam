using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FieldControl : MonoBehaviour
{
    public enum type
    {
        timer,
        turn
    }
    public type daleteType;

    public float deleteTime = 5f;
    public int deleteTurn = 1;
    private float timer = 0f;
    private float turn = 0f;
    private void Update()
    {
        if(daleteType == type.timer)
        {
            TimeCount();
        }
        else if (daleteType == type.turn)
        {
            TurnCount();
        }
    }
    public void TimeCount()
    {
        timer += Time.deltaTime;
        if(timer >= deleteTime)
        {
            Delete();
        }
    }
    public void TurnCount()
    {
        if(turn >= deleteTurn)
        {
            Delete();
        }
    }
    public void Delete() => Destroy(this.gameObject);
    public void AddTurnCount()
    {
        if(daleteType != type.turn)
        {
            return;
        }

        turn++;

    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bankroll") && other.gameObject.GetComponent<MoneyGainBankrollBase>() != null)
        {
            other.gameObject.GetComponent<MoneyGainBankrollBase>().SetMoneyMultiplier(2);//倍率を２倍に変更
        }
    }
}
[CustomEditor(typeof(FieldControl))]
public class MyComponentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        FieldControl myTarget = (FieldControl)target;

        // enum 表示
        EditorGUILayout.LabelField("フィールドの消え方", EditorStyles.boldLabel);
        myTarget.daleteType = (FieldControl.type)EditorGUILayout.EnumPopup("Type", myTarget.daleteType);

        // enum に応じて表示項目を切り替え
        switch (myTarget.daleteType)
        {
            case FieldControl.type.timer:
                EditorGUILayout.LabelField("時間", EditorStyles.boldLabel);
                myTarget.deleteTime = EditorGUILayout.FloatField("daleteTime", myTarget.deleteTime);
                break;

            case FieldControl.type.turn:
                EditorGUILayout.LabelField("ボールが落ちる数", EditorStyles.boldLabel);
                myTarget.deleteTurn = EditorGUILayout.IntField("deleteTurn", myTarget.deleteTurn);
                break;
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(myTarget);
        }
    }
}
