using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(StatePatrol))]
public class StatePatrolEditor : StateSOEditor
{
  public override void OnInspectorGUI()
  {
    
    StatePatrol eStateDashSO = (StatePatrol) target;
    serializedObject.Update();
    base.OnInspectorGUI();
         
    subSubTitle.fontSize = 13;
         
    subSubTitle.fontStyle = FontStyle.Italic; 
    EditorGUIUtility.labelWidth = 125;
    using (new GUILayout.VerticalScope(EditorStyles.helpBox))
    {
      GUILayout.Label("Specific Stats State", subTitle);
      EditorGUILayout.Space(6f);
      eStateDashSO.openSpecPanel = EditorGUILayout.BeginFoldoutHeaderGroup(eStateDashSO.openSpecPanel, "Specific Stats State");
      if (eStateDashSO.openSpecPanel)
      {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("speed"));
        EditorGUILayout.Space(4f);
        using (new GUILayout.HorizontalScope())
        {
          serializedObject.FindProperty("toThePlayer").boolValue = EditorGUILayout.Toggle("To The Player",
            serializedObject.FindProperty("toThePlayer").boolValue);
          GUILayout.FlexibleSpace();
          serializedObject.FindProperty("toTheOppositePlayer").boolValue =
            EditorGUILayout.Toggle("To The Opposite Player",
              serializedObject.FindProperty("toTheOppositePlayer").boolValue);
          
        }
EditorGUILayout.Space(4f);



using (new GUILayout.HorizontalScope())
        {
          EditorGUILayout.PropertyField(serializedObject.FindProperty("minDistance"));
          GUILayout.FlexibleSpace();
          EditorGUILayout.PropertyField(serializedObject.FindProperty("maxDistance"));
                            
        }  
     
              
        EditorGUILayout.Space(4f);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("sizeOfDetection"));
        EditorGUILayout.Space(4f);
     
        EditorGUILayout.PropertyField(serializedObject.FindProperty("layerMaskForRay"),new GUIContent("Wall Detection Box") );
        EditorGUILayout.Space(4f);
      }
      EditorGUILayout.EndFoldoutHeaderGroup();
    }
    if (serializedObject.FindProperty("toThePlayer").boolValue == true ||serializedObject.FindProperty("toTheOppositePlayer").boolValue == true)
    {
      EditorGUILayout.PropertyField(serializedObject.FindProperty("addAngleList"));
      EditorGUILayout.Space(4f);
    }
        
    EditorGUILayout.Space(6f);
         
    serializedObject.ApplyModifiedProperties();
  }
}
