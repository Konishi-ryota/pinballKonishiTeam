using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlungerTrigger))]
public class PlungerTriggerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlungerTrigger plungerTrigger = (PlungerTrigger)target;
        if (GUILayout.Button("発射時パワーの自動設定"))
        {
            plungerTrigger.SetPower();
        }
    }
}
