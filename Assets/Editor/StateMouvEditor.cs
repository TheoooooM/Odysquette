using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(StateMouvementSO))]
public class StateMouvEditor : StateSOEditor
{
   public override void OnInspectorGUI()
   {
    StateMouvementSO eStateMouvementSO = (StateMouvementSO) target;
    serializedObject.Update();
    base.OnInspectorGUI();
  
    subSubTitle.fontSize = 13;
  
    subSubTitle.fontStyle = FontStyle.Italic;
    EditorGUIUtility.labelWidth = 125;
   
     GUILayout.Label("Specific Stats State", subTitle);
     EditorGUILayout.Space(6f);
     eStateMouvementSO.openSpecPanel =
      EditorGUILayout.BeginFoldoutHeaderGroup(eStateMouvementSO.openSpecPanel, "Specific Stats State");
     if (eStateMouvementSO.openSpecPanel)
     {
      using (new GUILayout.VerticalScope(EditorStyles.helpBox))
      {
       
      EditorGUILayout.Space(4f);
      EditorGUILayout.PropertyField(serializedObject.FindProperty("moveSpeed"));



      using (new GUILayout.HorizontalScope())
      {
       EditorGUILayout.Space(4f);
       eStateMouvementSO.isFastRun = EditorGUILayout.Toggle("Is Fast Run",eStateMouvementSO.isFastRun);
       if (eStateMouvementSO.isFastRun)
       {
        GUILayout.FlexibleSpace();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("endFastMove"));
       }
      }

      EditorGUILayout.Space(4f);
     }
  
    
    }
   EditorGUILayout.EndFoldoutHeaderGroup();
    EditorGUILayout.Space(6f);
  
    serializedObject.ApplyModifiedProperties();
   }
}
