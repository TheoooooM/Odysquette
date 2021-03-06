using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(AngleAreaShootSO))]
public class AngleAreaShootSOEditor : StrawSOEditor
{
    private AngleAreaShootSO strawSo;
    public override void OnInspectorGUI()
    {
       
        base.OnInspectorGUI();
        serializedObject.Update();
        strawSo = (AngleAreaShootSO) target;
        GUIStyle myStyle = new GUIStyle();
        myStyle.fontSize = 13;
        myStyle.fontStyle = FontStyle.Bold;
        myStyle.normal.textColor = Color.white;GUI.backgroundColor = new Color(0f,2f,0f,0.5f);
        using (new GUILayout.VerticalScope(EditorStyles.helpBox))
        {
            
            GUILayout.Label("Specific Stats", myStyle);
            EditorGUILayout.Space(2f);
           
                
                
              
                
            

        
          
         
      
          EditorGUILayout.Space(2f);
           GUI.contentColor = Color.red;
          EditorGUILayout.Slider( serializedObject.FindProperty("angle"), 0f, 360f);
             
                GUILayout.FlexibleSpace();
                GUI.contentColor = Color.magenta;
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("angleDivision"));
                            GUI.contentColor = Color.white;
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
                                    if (serializedObject.FindProperty("rateMode").enumValueIndex == 2)
                                    {
                                        EditorGUILayout.PropertyField(
                                            serializedObject.FindProperty("effectAllNumberShoot"),
                                            new GUIContent("(X) Shoot"));
                                        EditorGUILayout.Space(2f);
                                    }
                                    using (new GUILayout.HorizontalScope())
                                    {
                                        EditorGUILayout.Slider(serializedObject.FindProperty("angleParameter"), 0, 360,
                                            "Angle Parameter");
                                                                              
                                        GUILayout.FlexibleSpace();
                                                                            EditorGUILayout.PropertyField(serializedObject.FindProperty("angleDivisionParameter"),
                                                                                new GUIContent("AngleDivision"));
                                    }
                                    
                                     EditorGUILayout.Space(2f);
                                    
                                   
                                }
                            }
            
            
            EditorGUILayout.Space(2f);
            
           
        }
        
        serializedObject.ApplyModifiedProperties();
    }
    public override void OnEditGizmos( SceneView sceneView)
    {base.OnEditGizmos( sceneView);
        try
        {
            if (strawSo.angle != 0)
            {
                if (strawSo.basePosition.Length != 0)
                {
                    for (int i = 0; i < strawSo.angleDivision + 2; i++)
                    {
                        Handles.color = Color.magenta;
                        if (i == 0 || i == strawSo.angleDivision + 1)
                        {
                            Handles.color = Color.red;

                        }

                        Vector3 currentBasePosition = new Vector3();
                        float currentAngle = -strawSo.angle / 2 + (strawSo.angle / (1 + strawSo.angleDivision)) * i;



                        Vector3 start = EmptyGizmos + strawSo.basePosition[0];
                        Vector3 rotation = Quaternion.Euler(0, currentAngle, 0) * Vector3.right;
                        Vector3 end = start + rotation * strawSo.range;
                        Handles.DrawWireDisc(start, Vector3.forward, rotation.magnitude * strawSo.range, 3f);
                        Handles.DrawLine(start, end, 2f);

                    }
                }
                else
                {
                    for (int i = 0; i < strawSo.angleDivision + 2; i++)
                    {
                        Handles.color = Color.magenta;
                        if (i == 0 || i == strawSo.angleDivision + 1)
                        {
                            Handles.color = Color.red;

                        }

                        Vector3 currentBasePosition = new Vector3();
                        float currentAngle = -strawSo.angle / 2 + (strawSo.angle / (1 + strawSo.angleDivision)) * i;



                        Vector3 start = EmptyGizmos;

                        Vector3 rotation = Quaternion.Euler(0, 0, currentAngle) * Vector3.right;
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
                        Handles.DrawLine(start, end, 2f);

                    }
                }

            }
        }
        catch
        {
            
        }
     
           
        
      
        
    }
}
