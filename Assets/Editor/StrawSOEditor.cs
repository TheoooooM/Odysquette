using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UI;
using UnityEngine.XR;
using Object = System.Object;

[CustomEditor(typeof(StrawSO))]
public class StrawSOEditor : Editor
{    GUIStyle myStyle = new GUIStyle();
    private GUIStyle enumStyle = new GUIStyle();
    protected Vector3 EmptyGizmos ;
    private bool editGizmos;
   static protected Color[] GizmoColor = new Color[]
    {
        Color.red*2, Color.magenta*2, new Color(1,0.64f,0f, 1f)*2, Color.cyan*2, Color.gray*2, new Color(0.59f,0.32f,0.31f,1)*2, Color.green*2,  Color.white*2   };

   private string buttonText;


   public override void OnInspectorGUI()
   {



       serializedObject.Update();
       if (editGizmos)
       {
           GUI.enabled = false;
       }
       else
       {
           GUI.enabled = true;
       }

       if (GUILayout.Button("On Edit Gizmos"))
       {
       
           SceneView.duringSceneGui += OnPosVectorGizmos;
           editGizmos = true;
           ActiveEditorTracker.sharedTracker.isLocked = true;
       }
    
    

   


       GUI.enabled = true;

       if (!editGizmos)
       {
           GUI.enabled = false;
       }
       else
       {
           GUI.enabled = true;
       }





       GUI.enabled = true;
       enumStyle.fontSize = 13;
       myStyle.fontStyle = FontStyle.Bold;
       enumStyle.normal.textColor = Color.white;
  
       enumStyle.normal.background = Texture2D.linearGrayTexture;
       enumStyle.alignment = TextAnchor.MiddleCenter;
       myStyle.fontSize = 13;
           myStyle.fontStyle = FontStyle.Bold;
           myStyle.normal.textColor = Color.white;
           GUI.backgroundColor = new Color(0f, 0f, 2f, 0.5f);
           EditorGUILayout.Space(4f);
           EditorGUILayout.PropertyField(serializedObject.FindProperty("strawName"));
           EditorGUILayout.Space(4f);
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
                       serializedObject.FindProperty("rateMode").enumValueIndex =
                           
                           EditorGUILayout.Popup( "Rate Mode =>", serializedObject.FindProperty("rateMode").enumValueIndex, new string[]{"Ultimate", "FireRate", "FireLoading"},enumStyle);
                       GUILayout.FlexibleSpace();
                       switch (serializedObject.FindProperty("rateMode").enumValueIndex)
                       {
                           case 0:
                           {
                              EditorGUILayout.PropertyField(serializedObject.FindProperty("timeValue"),
                                   new GUIContent("Ultimate Time"));
                          
                             
                               break;
                           }
                           case 1:
                           {
                               EditorGUILayout.PropertyField(serializedObject.FindProperty("timeValue"),
                                   new GUIContent("Fire Rate"));
                               GUILayout.FlexibleSpace();
                               EditorGUILayout.PropertyField(serializedObject.FindProperty("ultimatePoints"));
                               break;
                           }
                           case 2:
                           {
                               EditorGUILayout.PropertyField(serializedObject.FindProperty("timeValue"),
                                   new GUIContent("Fire Loading Time"));
                               GUILayout.FlexibleSpace();
                               EditorGUILayout.PropertyField(serializedObject.FindProperty("ultimatePoints"));
                               break;
                           }
                       }
                  
                   }

                   EditorGUILayout.Space(2f);
               }



               EditorGUILayout.Space(4f);

               using (new GUILayout.VerticalScope(EditorStyles.helpBox))
               {
                   GUILayout.Label("Range", myStyle);
                   EditorGUILayout.Space(2f);
                   serializedObject.FindProperty("hasRange").boolValue =
                       EditorGUILayout.Toggle("Has Range",
                           serializedObject.FindProperty("hasRange").boolValue);
                   if (serializedObject.FindProperty("hasRange").boolValue == true)
                   {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("range"));
                   }
                 
                  
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
                       EditorGUILayout.PropertyField(serializedObject.FindProperty("strawRenderer"));

                   }

                   EditorGUILayout.Space(2f);

               }

               EditorGUILayout.Space(4f);
               using (new GUILayout.VerticalScope(EditorStyles.helpBox))
               {
                   GUILayout.Label("Delay Stats", myStyle);
                   EditorGUILayout.Space(2f);

                   using (new GUILayout.HorizontalScope())
                   {
                       serializedObject.FindProperty("isDelayBetweenShoot").boolValue =
                           EditorGUILayout.Toggle("Delay Shoot",
                               serializedObject.FindProperty("isDelayBetweenShoot").boolValue
                           );
                       if (serializedObject.FindProperty("isDelayBetweenShoot").boolValue == true)
                       {
                           GUILayout.FlexibleSpace();
                           EditorGUILayout.Space(2f);
                           EditorGUILayout.PropertyField(serializedObject.FindProperty("delayBetweenShoot"),new GUIContent("Delay Shoot"));
                       }
                   }
                   EditorGUILayout.Space(2f);
                   
                       
                       serializedObject.FindProperty("isDelayBetweenWaveShoot").boolValue =
                           EditorGUILayout.Toggle("Delay Wave Shoot",
                               serializedObject.FindProperty("isDelayBetweenWaveShoot").boolValue
                           );
                       if (serializedObject.FindProperty("isDelayBetweenWaveShoot").boolValue == true)
                       {
                           GUILayout.FlexibleSpace();
                           EditorGUILayout.Space(2f);
                           using (new GUILayout.HorizontalScope())
                           {
                                 EditorGUILayout.PropertyField(serializedObject.FindProperty("delayBetweenWaveShoot"),new GUIContent("Delay Wave Shoot"));
                                                          EditorGUILayout.PropertyField(serializedObject.FindProperty("numberWaveShoot"),new GUIContent("Number Wave Shoot"));
                           }
                         
                       


                   }
}
                  
                   EditorGUILayout.Space(4f);
                   using (new GUILayout.VerticalScope(EditorStyles.helpBox))
                   {
                       GUILayout.Label("Other Stats", myStyle);
                       EditorGUILayout.Space(2f);
                       EditorGUILayout.PropertyField(serializedObject.FindProperty("dragRB"),
                           new GUIContent("Drag"));
                       EditorGUILayout.Space(2f);
                       EditorGUILayout.PropertyField(serializedObject.FindProperty("knockUp"));
                       EditorGUILayout.Space(2f);
                   }

                  

               

               EditorGUILayout.Space(4f);
               GUI.backgroundColor = new Color(0f, 2f, 2f, 0.25f);
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
                   serializedObject.FindProperty("rateMainParameter").boolValue =
                       EditorGUILayout.Toggle("Main Parameters",
                           serializedObject.FindProperty("rateMainParameter").boolValue);
                   if (serializedObject.FindProperty("rateMainParameter").boolValue == true)
                   {
                       if (serializedObject.FindProperty("rateMode").enumValueIndex == 1 )
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
    
      
        Handles.DrawWireDisc(EmptyGizmos, Vector3.forward, 0.1f);
      EmptyGizmos =  Handles.DoPositionHandle(EmptyGizmos, Quaternion.identity);
   GUI.backgroundColor = new Color(0f,0f,2f,0.5f);
   GUI.Window(0, new Rect(5, 20, 250, 75), delegate {
            EmptyGizmos=  EditorGUI.Vector3Field(new Rect(25, 40, 200, 80), GUIContent.none, EmptyGizmos );
        }, "Gizmos Position");  
   if (Event.current.keyCode == KeyCode.Space)
   {

       SceneView.duringSceneGui -= OnEditGizmos;
       try
       {
           SceneView.duringSceneGui -= OnPosVectorGizmos;
       }
       catch (Exception e)
       {
               
       }
           
       Selection.objects = new UnityEngine.Object[1]
       {
           target
       };
      
       editGizmos = false;
       ActiveEditorTracker.sharedTracker.isLocked = false;

   }
    }

   public void OnPosVectorGizmos(SceneView sceneView)
   {
       Ray ray  = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
       RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
       if (hit.collider !=null)
       {EmptyGizmos = hit.point;
           
           Handles.DrawWireDisc(EmptyGizmos, Vector3.forward, 1f);
           
       }
       if (Event.current.type == EventType.MouseMove)
       {
           
           sceneView.Repaint();
       }

       if (Event.current.keyCode == KeyCode.A)
       {  SceneView.duringSceneGui += OnEditGizmos;
           SceneView.duringSceneGui -= OnPosVectorGizmos;
         
       }
       if (Event.current.keyCode == KeyCode.Space)
       {

           SceneView.duringSceneGui -= OnEditGizmos;
           try
           {
               SceneView.duringSceneGui -= OnPosVectorGizmos;
           }
           catch (Exception e)
           {
               
           }
           
           Selection.objects = new UnityEngine.Object[1]
           {
               target
           };
      
           editGizmos = false;
           ActiveEditorTracker.sharedTracker.isLocked = false;

       }
       







   }

  
}
