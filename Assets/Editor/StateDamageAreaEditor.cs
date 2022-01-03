using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(StateDamageArea))]
public class StateDamageAreaEditor : StateSOEditor
{
    private Editor bulletEditor;


    public override void OnInspectorGUI()
    {
        StateDamageArea stateDamageAreaSO = (StateDamageArea) target;
        serializedObject.Update();
        base.OnInspectorGUI();
         
        subSubTitle.fontSize = 13;
         
        subSubTitle.fontStyle = FontStyle.Italic; 
        EditorGUIUtility.labelWidth = 125;
        using (new GUILayout.VerticalScope(EditorStyles.helpBox))
        {
            GUILayout.Label("Specific Stats State", subTitle);
            EditorGUILayout.Space(6f);
            using (new GUILayout.HorizontalScope())
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("numberShoot"));
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("totalProbability"));
                }
                EditorGUILayout.Space(4f);
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("intervalDistancesList"), new GUIContent("Interval Distance"));
                if (EditorGUI.EndChangeCheck())
                {
                    stateDamageAreaSO.totalProbability = 0;
                    for (int i = 0; i < stateDamageAreaSO.intervalDistancesList.Length; i++)
                    {
                        stateDamageAreaSO.totalProbability 
                            += stateDamageAreaSO.intervalDistancesList[i].probability;
                    }
                }
                EditorGUILayout.Space(4f);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("delayList"));
                EditorGUILayout.Space(4f);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("prefab"));
            

                EditorGUILayout.Space(4f);
            }
        
    EditorGUILayout.Space(6f);
    serializedObject.ApplyModifiedProperties();
    }
}
