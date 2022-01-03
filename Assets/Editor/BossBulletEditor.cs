using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(BossBullet))]
public class BossBulletEditor : Editor
{
    public override void OnInspectorGUI()
    
    {
        serializedObject.Update();
        using (new GUILayout.HorizontalScope())
        {
           
            EditorGUILayout.PropertyField(serializedObject.FindProperty("damage"));
            GUILayout.FlexibleSpace();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("baseTime"));
        }
        EditorGUILayout.Space(4f);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("damageTime"));
        EditorGUILayout.Space(4f);
        serializedObject.ApplyModifiedProperties();
    }

}
