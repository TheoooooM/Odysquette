using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(StateEnemySO))]
public class StateSOEditor : Editor
{
  
    public GUIStyle subTitle = new GUIStyle();
  public  GUIStyle subSubTitle = new GUIStyle();

    public override void OnInspectorGUI()
    {
     serializedObject.Update();
       StateEnemySO eStateSO = (StateEnemySO) target;
        subTitle.normal.textColor = Color.white;
    
        subTitle.alignment = TextAnchor.MiddleLeft;
        subTitle.fontSize = 13;
         
        subTitle.fontStyle = FontStyle.Bold; 
        subSubTitle.normal.textColor = Color.white;
    
        subSubTitle.alignment = TextAnchor.MiddleLeft;
        subSubTitle.fontSize = 13;
         
        subSubTitle.fontStyle = FontStyle.Italic; 
        EditorGUIUtility.labelWidth = 125;
        GUILayout.Label("Main Stats State", subTitle);
        EditorGUILayout.Space(6f);




        eStateSO.openBasePanel = EditorGUILayout.BeginFoldoutHeaderGroup(eStateSO.openBasePanel, "Main Stats State");


        if (eStateSO.openBasePanel)
        {
                  using (new GUILayout.VerticalScope(EditorStyles.helpBox))
                        {
                            GUILayout.Label("Main Stats State", subSubTitle);
                                        EditorGUILayout.Space(4f);
                                        using (new GUILayout.HorizontalScope())
                                        {
                                             EditorGUILayout.PropertyField(serializedObject.FindProperty("isKnockUpInState"));
                                             EditorGUILayout.PropertyField(serializedObject.FindProperty("duringDefaultState"));
                                        }
                                        EditorGUILayout.Space(4f);
                                        GUILayout.Label("Start State", subSubTitle);
                                        EditorGUILayout.Space(4f);
                                        eStateSO.haveStartState = EditorGUILayout.Toggle("Have Start State",eStateSO.haveStartState);
                                        EditorGUILayout.Space(4f);
                                        using (new GUILayout.HorizontalScope())
                                        {
                                            
                                            if (eStateSO.haveStartState)
                                            {
                                                 EditorGUILayout.PropertyField(serializedObject.FindProperty("oneStartState"));
                                                 GUILayout.FlexibleSpace();
                                                 EditorGUILayout.PropertyField(serializedObject.FindProperty("startTime"));
                                            }
                                           
                                        }
                                        EditorGUILayout.Space(4f);
                                        GUILayout.Label("Play State", subSubTitle);
                                        
                                        EditorGUILayout.Space(4f);
                                        EditorGUILayout.PropertyField(serializedObject.FindProperty("playStateTime"));
                                        EditorGUILayout.Space(4f);
                        }
        }
      
           
        

        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUILayout.Space(8f);
       
       
       
      
 GUILayout.Label("Debug State", subTitle);
                       EditorGUILayout.Space(6f);
        GUI.contentColor = Color.red;
        eStateSO.openKnobDebugPanel = EditorGUILayout.BeginToggleGroup("Debug Panel",eStateSO.openKnobDebugPanel ); GUI.contentColor = Color.white;
        EditorGUILayout.Space(4f);
            if (eStateSO.openKnobDebugPanel)
            { 
                using (new GUILayout.VerticalScope(EditorStyles.helpBox))
                     {
                         EditorGUILayout.PropertyField(serializedObject.FindProperty("isFixedUpdate"));
                EditorGUILayout.Space(4f);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("objectInStateManagersCondition"));
                EditorGUILayout.Space(4f);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("objectInStateManagersState"));
                EditorGUILayout.Space(4f);
             
              
            }
             
        }  
           
      
   EditorGUILayout.EndToggleGroup();
        
     
        
        EditorGUILayout.Space(4f);
        
serializedObject.ApplyModifiedProperties();
        
       
    }
}
