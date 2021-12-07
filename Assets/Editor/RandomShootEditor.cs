using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(RandomShootSO))]
public class RandomShootEditor : StrawSOEditor
{
    public RandomShootSO strawSo;
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.OnInspectorGUI();
        strawSo = (RandomShootSO) target;
         GUIStyle myStyle = new GUIStyle();
        myStyle.fontSize = 13;
        myStyle.fontStyle = FontStyle.Bold;
        myStyle.normal.textColor = Color.white;
        GUI.backgroundColor = new Color(0f,2f,0f,0.5f);
        using (new GUILayout.VerticalScope(EditorStyles.helpBox))
        {
          
            GUILayout.Label("Specific Stats", myStyle);
            EditorGUILayout.Space(4f);
           
            EditorGUILayout.PropertyField(serializedObject.FindProperty("drawNumber"));
       
           
            
            EditorGUILayout.Space(2f);
                 EditorGUILayout.PropertyField(serializedObject.FindProperty("probabilityDiscount"));
          EditorGUILayout.Space(2f);
          strawSo.totalProbability = 0;
          for (int n = 0; n < strawSo.possibleDirections.Count; n++)
          {
              
              GUI.enabled = false;
              EditorGUILayout.ColorField(GizmoColor[n],GUILayout.Width(60f) );
              GUI.enabled = true;
              EditorGUILayout.PropertyField(serializedObject.FindProperty("possibleDirections").GetArrayElementAtIndex(n));
              strawSo.totalProbability += strawSo.possibleDirections[n].probability;
          }
          GUI.contentColor = Color.white;
          
          EditorGUILayout.Space(2f);
          using (new GUILayout.HorizontalScope())
          {
              if (GUILayout.Button("Add Direction") )
              {
                                                     
                  strawSo.possibleDirections.Add(new RandomShootSO.angleRandom());
              }
              if (GUILayout.Button("Remove Direction") )
              {
                  strawSo.possibleDirections.RemoveAt(strawSo.possibleDirections.Count-1);
              }
          }
           
          EditorGUILayout.Space(2f);
          EditorGUILayout.PropertyField(serializedObject.FindProperty("totalProbability"));
            EditorGUILayout.Space(2f);
            
           GUI.backgroundColor = new Color(2f,2f, 0f,0.6f);
          
            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                if (serializedObject.FindProperty("rateMode").enumValueIndex == 2)
                {
                    GUILayout.Label("Specific  Parameters for Loading Time ", myStyle);
                }
                else if(serializedObject.FindProperty("rateMode").enumValueIndex == 1)
                {
                    GUILayout.Label("Specific Parameters all of (X) Shoot ", myStyle);
                       
                }
                else
                {GUILayout.Label("Specific  Parameters  ", myStyle);
                    
                }

                EditorGUILayout.Space(2f);
                serializedObject.FindProperty("rateSecondParameter").boolValue =
                    EditorGUILayout.Toggle("Specific Parameters",
                        serializedObject.FindProperty("rateSecondParameter").boolValue);
                if (serializedObject.FindProperty("rateSecondParameter").boolValue == true)
                {
                    
                    EditorGUILayout.Space(2f);
                    if (serializedObject.FindProperty("rateMode").enumValueIndex == 1)
                    {
                       
                        EditorGUILayout.Space(2f);
                    } EditorGUILayout.PropertyField(
                                                 serializedObject.FindProperty("effectAllNumberShoot"),
                                                 new GUIContent("(X) Shoot"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("possibleDirectionsParameter"),
                        new GUIContent("Possibles Directions"));
                    EditorGUILayout.Space(2f);
                }
            }
        }
        
        serializedObject.ApplyModifiedProperties();
    }
     public override void OnEditGizmos( SceneView sceneView)
    {

base.OnEditGizmos( sceneView);




        try
        {
if (strawSo.basePosition.Length != 0)
                    {
                        for (int i = 0; i < strawSo.basePosition.Length; i++)
                        {
                            Handles.color = GizmoColor[i];
                            
                            Vector3 start = EmptyGizmos + strawSo.basePosition[i];
                            Vector3 rotation = Quaternion.Euler(0, 0,strawSo.possibleDirections[i].angle )*Vector3.right;
                            Vector3 end = start + rotation * strawSo.range;
                            Handles.DrawWireDisc(start, Vector3.forward, rotation.magnitude * strawSo.range, 3f);
                            Handles.DrawLine(start,end, 2f);
                            
                        } 
                    }
                    else
                    {
                        for (int i = 0; i < strawSo.possibleDirections.Count; i++)
                        {
                       
                            Handles.color = GizmoColor[i];
                            Vector3 start = EmptyGizmos;
                            
                            Vector3 rotation = Quaternion.Euler(0, 0, strawSo.possibleDirections[i].angle)*Vector3.right;
                            Vector3 end;
                            if (strawSo.hasRange)
                            {
                                   end = start + (rotation * strawSo.range);
                                                                Handles.DrawWireDisc(EmptyGizmos, Vector3.forward, rotation.magnitude * strawSo.range, 3f);
                            }
                             
                            
                            else
                            {
                                end = start + (rotation * 2f);
                                                                Handles.DrawWireDisc(EmptyGizmos, Vector3.forward, rotation.magnitude * 2f, 3f);
                            }
                         
                            Handles.DrawLine(start,end, 2f);
                         
                        } 
                    }
           
        }
        catch
        {
            
        }
    }
}
