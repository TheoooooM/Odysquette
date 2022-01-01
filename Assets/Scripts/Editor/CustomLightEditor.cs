using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(LightColor))]
public class CustomLightEditor : Editor
{
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        for (int i = 0; i < ((LightColor) target).MatDataList.Count(); i++) {
            if (GUILayout.Button(((LightColor) target).MatDataList[i].name)) {
                ((LightColor) target).ChangeLight(((LightColor) target).MatDataList[i]);
            }
        }
        GUILayout.Space(4);
        if (GUILayout.Button("RANDOM")) {
            ((LightColor) target).ChangeLight(((LightColor) target).MatDataList[Random.Range(0, ((LightColor) target).MatDataList.Count)]);
        }
    }
}
