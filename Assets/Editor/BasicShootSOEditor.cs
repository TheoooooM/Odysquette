using System.Collections;
using System.Collections.Generic;
using PlasticPipe.PlasticProtocol.Client;
using UnityEngine;
using UnityEditor;
using UnityEngine.XR;

[CustomEditor(typeof(BasicShootSO))]
public class BasicShootSOEditor : StrawSOEditor
{

    private BasicShootSO strawSo;

  


    public override void OnInspectorGUI()
    {
        
     serializedObject.Update();
        base.OnInspectorGUI();
        strawSo = (BasicShootSO) target;

        GUIStyle myStyle = new GUIStyle();
        myStyle.fontSize = 13;
        myStyle.fontStyle = FontStyle.Bold;
        myStyle.normal.textColor = Color.white;
        GUI.backgroundColor = new Color(0f,2f,0f,0.5f);
        using (new GUILayout.VerticalScope(EditorStyles.helpBox))
        {
          
            GUILayout.Label("Specific Stats", myStyle);
            EditorGUILayout.Space(4f);
           
         
            EditorGUILayout.PropertyField(serializedObject.FindProperty("directions"));
           
            
            EditorGUILayout.Space(2f);
            
            GUI.backgroundColor = new Color(2f,2f, 0f,0.6f);
            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                if (serializedObject.FindProperty("loadingRateMode").boolValue == true)
                {
                    GUILayout.Label("Specific Parameters for Loading Time ", myStyle);
                }
                else
                {
                    GUILayout.Label("Specific Parameters all of (X) Shoot ", myStyle);



                }

                EditorGUILayout.Space(2f);
                serializedObject.FindProperty("rateSecondParameter").boolValue =
                    EditorGUILayout.Toggle("Specific Parameters",
                        serializedObject.FindProperty("rateSecondParameter").boolValue);
                if (serializedObject.FindProperty("rateSecondParameter").boolValue == true)
                {
                    
                    EditorGUILayout.Space(2f);
                    if (serializedObject.FindProperty("loadingRateMode").boolValue == false)
                    {
                        EditorGUILayout.PropertyField(
                            serializedObject.FindProperty("effectAllNumberShoot"),
                            new GUIContent("(X) Shoot"));
                        EditorGUILayout.Space(2f);
                    }
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("directionParameter"),
                        new GUIContent("Directions"));
                    EditorGUILayout.Space(2f);
                }
            }
        }
        
        
        
        serializedObject.ApplyModifiedProperties();
    }

    public override void OnEditGizmos( SceneView sceneView)
    {
      
        

      

        if (strawSo.directions.Length !=  0)
        {
            if (strawSo.basePosition.Length != 0)
                    {
                        for (int i = 0; i < strawSo.basePosition.Length; i++)
                        {
                            Handles.color = GizmoColor[i];
                            
                            Vector3 start = EmptyGizmos.transform.position + strawSo.basePosition[i];
                            Vector3 rotation = Quaternion.Euler(0, strawSo.directions[i], 0)*EmptyGizmos.transform.forward;
                            Vector3 end = start + rotation * strawSo.range;
                            Handles.DrawWireDisc(start, EmptyGizmos.transform.up, rotation.magnitude * strawSo.range, 3f);
                            Handles.DrawLine(start,end, 2f);
                            
                        } 
                    }
                    else
                    {
                        for (int i = 0; i < strawSo.directions.Length; i++)
                        {
                       
                            Handles.color = GizmoColor[i];
                            Vector3 start = EmptyGizmos.transform.position;
                            
                            Vector3 rotation = Quaternion.Euler(0, strawSo.directions[i], 0)*EmptyGizmos.transform.forward;
                            Vector3 end = start + (rotation * strawSo.range);
                            Handles.DrawWireDisc(EmptyGizmos.transform.position, EmptyGizmos.transform.up, rotation.magnitude * strawSo.range, 3f);
                            Handles.DrawLine(start,end, 2f);
                         
                        } 
                    }
           
        }
      
        
    }
}