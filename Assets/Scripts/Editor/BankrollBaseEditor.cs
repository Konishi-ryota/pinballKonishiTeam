using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BankrollBase), true)]
public class BankrollBaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        BankrollBase bankrollBase = (BankrollBase)target;
        if (GUILayout.Button("プレハブと親の設定"))
        {
            bankrollBase.FindDefultBankrollPrefab();
        }
    }
}
