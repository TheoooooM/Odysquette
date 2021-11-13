using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(StateWind))]
public class WindStateEditor : StateSOEditor
{
    public override void OnInspectorGUI()
    {
     ;
        StateWind eStateMouvementSO = (StateWind) target;
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
               
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("speedWind"));
                    EditorGUILayout.Space(4f);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("direction"));
  
                
     
                EditorGUILayout.Space(4f);
            }
  
    
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
    }
}
