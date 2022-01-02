using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(StateDash))]
public class StateDashEditor : StateSOEditor
{
    public override void OnInspectorGUI()
    {
        StateDash eStateDashSO = (StateDash) target;
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
                          using (new GUILayout.HorizontalScope())
                          {
                              EditorGUILayout.PropertyField(serializedObject.FindProperty("maxspeed"));
                              GUILayout.FlexibleSpace();
                              EditorGUILayout.PropertyField(serializedObject.FindProperty("distanceDash"));
                              GUILayout.FlexibleSpace();
                              EditorGUILayout.PropertyField(serializedObject.FindProperty("rangeDetection"));
                            
                          }
                          
                          EditorGUILayout.Space(4f);
                          EditorGUILayout.PropertyField(serializedObject.FindProperty("animationCurve"));
                          EditorGUILayout.Space(4f);
                          EditorGUILayout.PropertyField(serializedObject.FindProperty("extentsRangeDetection"), new GUIContent("Wall Detection Box"));
                          EditorGUILayout.Space(4f);
                          EditorGUILayout.PropertyField(serializedObject.FindProperty("layerMask"));
                          EditorGUILayout.Space(4f);
                          using (new GUILayout.HorizontalScope())
                          {
                              EditorGUILayout.PropertyField(serializedObject.FindProperty("timeKnockback"));
                              GUILayout.FlexibleSpace();
                              EditorGUILayout.PropertyField(serializedObject.FindProperty("speedKnockback"));
                          }

                          EditorGUILayout.Space(4f);
                          EditorGUILayout.PropertyField(serializedObject.FindProperty("curveKnockback"));
                       
                          EditorGUILayout.Space(4f);
                      }
                      EditorGUILayout.EndFoldoutHeaderGroup();
         }
        
         EditorGUILayout.Space(6f);
         
        serializedObject.ApplyModifiedProperties();
    }
}
