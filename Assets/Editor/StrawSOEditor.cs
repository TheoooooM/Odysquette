using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UI;
using Object = System.Object;

[CustomEditor(typeof(StrawSO))]
public class StrawSOEditor : Editor
{
    protected GameObject EmptyGizmos ;
    private bool editGizmos;
   static protected Color[] GizmoColor = new Color[]
    {
        Color.red*2, Color.magenta*2, new Color(1,0.64f,0f, 1f)*2, Color.cyan*2, Color.gray*2, new Color(0.59f,0.32f,0.31f,1)*2, Color.green*2,  Color.white*2   };

  

    public override void OnInspectorGUI()
    {
       
        serializedObject.Update();
        
        if (GUILayout.Button("Edit Stat with Gizmos"))
        {
           
            ActiveEditorTracker.sharedTracker.isLocked = true;
            EditorApplication.ExecuteMenuItem("GameObject/Create Empty");
             EmptyGizmos = Selection.gameObjects[0];
             SceneView.duringSceneGui += OnEditGizmos;
             editGizmos = true;
            
        }
        if (GUILayout.Button("Stop Edit"))
        {
            DestroyImmediate(EmptyGizmos);
            SceneView.duringSceneGui -= OnEditGizmos;
            Selection.objects = new UnityEngine.Object[1]
            {
                target
            };
            EmptyGizmos = null;
            editGizmos = false;
            ActiveEditorTracker.sharedTracker.isLocked = false;
            
        }
        GUIStyle myStyle = new GUIStyle();
        myStyle.fontSize = 13;
        myStyle.fontStyle = FontStyle.Bold;
        myStyle.normal.textColor = Color.white;
        GUI.backgroundColor = new Color(0f,0f,2f,0.5f);
        
        using (new GUILayout.VerticalScope(EditorStyles.helpBox))
        {
            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUIUtility.labelWidth = 120;
                GUILayout.Label("Main Stats ", myStyle);
                EditorGUILayout.Space(2f);

                using (new GUILayout.HorizontalScope())
                {
                   
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("damage"));
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("speedBullet"));



                }

                EditorGUILayout.Space(2f);
            }

            EditorGUILayout.Space(4f);
            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {

                GUILayout.Label("Rate Mode ", myStyle);
                EditorGUILayout.Space(2f);



                using (new GUILayout.HorizontalScope())
                {
                    serializedObject.FindProperty("loadingRateMode").boolValue =
                        EditorGUILayout.Toggle("Loading Mode",
                            serializedObject.FindProperty("loadingRateMode").boolValue
                        );

                    if (serializedObject.FindProperty("loadingRateMode").boolValue == true)
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("timeValue"),
                            new GUIContent("Loading Time"));
                    }
                    else
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("timeValue"),
                            new GUIContent("Fire Rate"));

                    }


 


                }EditorGUILayout.Space(2f);
            }

           

                EditorGUILayout.Space(4f);

                using (new GUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    GUILayout.Label("Range", myStyle);
                    EditorGUILayout.Space(2f);
          
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("range"));
                    EditorGUILayout.Space(2f);
                
          
                EditorGUILayout.PropertyField(serializedObject.FindProperty("basePosition"));
          
             EditorGUILayout.Space(2f);
                }

             
            

            EditorGUILayout.Space(4f);

            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                GUILayout.Label("Bullet", myStyle);
                EditorGUILayout.Space(2f);

                using (new GUILayout.HorizontalScope())
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("prefabBullet"));

                }

                EditorGUILayout.Space(2f);
              
            }
            EditorGUILayout.Space(4f);
            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                GUILayout.Label("Other Stats", myStyle);
                EditorGUILayout.Space(2f);

                using (new GUILayout.HorizontalScope())
                {
                    serializedObject.FindProperty("isDelay").boolValue =
                        EditorGUILayout.Toggle("Is Delay",
                            serializedObject.FindProperty("isDelay").boolValue
                        );
                    if (serializedObject.FindProperty("isDelay").boolValue == true)
                    { GUILayout.FlexibleSpace();
                        EditorGUILayout.Space(2f);
                          EditorGUILayout.PropertyField(serializedObject.FindProperty("delay"));
                    }
                  
                  
                  

                } 
                EditorGUILayout.Space(2f);
  EditorGUILayout.PropertyField(serializedObject.FindProperty("dragRB"),
                        new GUIContent("Drag"));
                EditorGUILayout.Space(2f);
              
            }
            EditorGUILayout.Space(4f);
GUI.backgroundColor = new Color(0f,2f, 2f,0.25f);
            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                if (serializedObject.FindProperty("loadingRateMode").boolValue == true)
                {
                    GUILayout.Label("Main Parameters for Loading Time ", myStyle);
                }
                else
                {
                    GUILayout.Label("Main Parameters all of (X) Shoot ", myStyle);
                  
                    
                    
                }
                EditorGUILayout.Space(2f);
                                serializedObject.FindProperty("rateMainParameter").boolValue =
                                    EditorGUILayout.Toggle("Main Parameters",
                                        serializedObject.FindProperty("rateMainParameter").boolValue);
                                if (serializedObject.FindProperty("rateMainParameter").boolValue == true)
                                {
                                    if (serializedObject.FindProperty("loadingRateMode").boolValue == false)
                                    {
                                        EditorGUILayout.PropertyField(
                                            serializedObject.FindProperty("effectAllNumberShoot"),
                                            new GUIContent("(X) Shoot"));
                                    EditorGUILayout.Space(2f);
                                }

                                using (new GUILayout.HorizontalScope())
                                          {
                                              EditorGUILayout.PropertyField(
                                                  serializedObject.FindProperty("damageParameter"), new GUIContent("Damage"));
                                              GUILayout.FlexibleSpace();
                                              EditorGUILayout.PropertyField(serializedObject.FindProperty("speedParameter"),
                                                  new GUIContent("Speed"));
                                           
                                          }
                                          EditorGUILayout.Space(2f);
                                          
                                              EditorGUILayout.PropertyField(serializedObject.FindProperty("rangeParameter"),
                                                  new GUIContent("Range"));
                                              EditorGUILayout.PropertyField(serializedObject.FindProperty("basePositionParameter"), 
                                                  new GUIContent("Base Position"));
                                           
                                          
                                          EditorGUILayout.Space(2f);
                                          using (new GUILayout.HorizontalScope())
                                          {
                                              EditorGUILayout.PropertyField(serializedObject.FindProperty("delayParameter"),
                                                  new GUIContent("Delay"));
                                              GUILayout.FlexibleSpace();
                                              EditorGUILayout.PropertyField(serializedObject.FindProperty("dragRBParameter"),
                                                  new GUIContent("Drag"));
                                            
                                          }
                                          EditorGUILayout.Space(2f);
                                    }
                                  
                                    
                                }
            }
  
            EditorGUILayout.Space(6f);
            
            GUI.backgroundColor = Color.white;
            serializedObject.ApplyModifiedProperties();



        }

    public virtual void OnEditGizmos(SceneView sceneView)
    {
        

        
    }
    }
