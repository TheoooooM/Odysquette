using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StateFlash))]
public class StateEditorFlash : StateSOEditor
{
 public override void OnInspectorGUI()
 {
  StateFlash eStateFlashSO = (StateFlash) target;
  serializedObject.Update();
  base.OnInspectorGUI();

  subSubTitle.fontSize = 13;

  subSubTitle.fontStyle = FontStyle.Italic;
  EditorGUIUtility.labelWidth = 125;
 
   GUILayout.Label("Specific Stats State", subTitle);
   EditorGUILayout.Space(6f);
   eStateFlashSO.openSpecPanel =
    EditorGUILayout.BeginFoldoutHeaderGroup(eStateFlashSO.openSpecPanel, "Specific Stats State");
   if (eStateFlashSO.openSpecPanel)
   {
    using (new GUILayout.VerticalScope(EditorStyles.helpBox))
    {
    
    EditorGUILayout.Space(4f);
    GUILayout.Label("Range", subSubTitle); 
    EditorGUILayout.Space(4f);
    EditorGUILayout.PropertyField(serializedObject.FindProperty("rangeForShoot"));
    EditorGUILayout.Space(4f);
   
     EditorGUILayout.PropertyField(serializedObject.FindProperty("extentsRangeDetection"),new GUIContent("Wall Detection Box"));
     EditorGUILayout.Space(4f);
     EditorGUILayout.PropertyField(serializedObject.FindProperty("layerMaskRay"));
   

     EditorGUILayout.Space(4f);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("offSetDistance"), new GUIContent("Viewfinder Offset"));


    EditorGUILayout.Space(4f);
    GUILayout.Label("Flash Effect", subSubTitle);
    EditorGUILayout.Space(4f);
    using (new GUILayout.HorizontalScope())
    {
     EditorGUILayout.PropertyField(serializedObject.FindProperty("timeEffectFlash"));
     GUILayout.FlexibleSpace();
     EditorGUILayout.PropertyField(serializedObject.FindProperty("lowSpeed"));

    }
   
    EditorGUILayout.Space(4f);
   }

  
  }
 EditorGUILayout.EndFoldoutHeaderGroup();
  EditorGUILayout.Space(6f);

  serializedObject.ApplyModifiedProperties();
 }
}
