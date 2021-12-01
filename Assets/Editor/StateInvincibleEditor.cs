using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(StateInvincible))]
public class StateInvincibleEditor : StateSOEditor
{
    public override void OnInspectorGUI()
    {StateInvincible eStateDashSO = (StateInvincible) target;
        serializedObject.Update();
        base.OnInspectorGUI();
        subSubTitle.fontSize = 13;
         
        subSubTitle.fontStyle = FontStyle.Italic; 
        EditorGUIUtility.labelWidth = 125;
        GUILayout.Label("Specific Stats State", subTitle);
        EditorGUILayout.Space(6f);
        eStateDashSO.openSpecPanel = EditorGUILayout.BeginFoldoutHeaderGroup(eStateDashSO.openSpecPanel, "Specific Stats State");
        if (eStateDashSO.openSpecPanel)
        {
            using (new GUILayout.HorizontalScope())
            { EditorGUILayout.PropertyField(serializedObject.FindProperty("phaseBoss"));
            }
        }
        EditorGUILayout.Space(6f);
     
        serializedObject.ApplyModifiedProperties();
    }
}
