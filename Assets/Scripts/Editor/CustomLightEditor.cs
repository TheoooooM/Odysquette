using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(LightCol))]
public class CustomLightEditor : Editor
{
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        for (int i = 0; i < ((LightCol) target).MatDataList.Count(); i++) {
            if (GUILayout.Button(((LightCol) target).MatDataList[i].name)) {
                ((LightCol) target).ChangeLight(((LightCol) target).MatDataList[i]);
            }
        }
        GUILayout.Space(4);
        if (GUILayout.Button("RANDOM")) {
            ((LightCol) target).ChangeLight(((LightCol) target).MatDataList[Random.Range(0, ((LightCol) target).MatDataList.Count)]);
        }
    }
}
