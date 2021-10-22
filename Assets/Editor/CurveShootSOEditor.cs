using System;
using System.Collections;
using System.Collections.Generic;
using PlasticPipe.PlasticProtocol.Client;
using Unity.Mathematics;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CurveShootSO))]
public class CurveShootSOEditor : StrawSOEditor
{
    private CurveShootSO strawSo;
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
       
       strawSo = (CurveShootSO) target;
        GUIStyle myStyle = new GUIStyle();
        GUIStyle myStylo = new GUIStyle();
        myStylo.fontSize = 13;
        myStylo.fontStyle = FontStyle.Bold;
        myStylo.normal.textColor = new Color(0.75f,0.75f,0.75f,1);
        myStyle.fontSize = 13;
        myStyle.fontStyle = FontStyle.Bold;
        myStyle.normal.textColor = Color.white;
        GUI.backgroundColor = new Color(0f, 2f, 0f, 0.5f);
        using (new GUILayout.VerticalScope(EditorStyles.helpBox))
        {
            GUILayout.Label("Specific Stats", myStyle);
            EditorGUILayout.Space(2f);
            using (new GUILayout.HorizontalScope())
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("stepOfCurve")); 
               
                
            }

            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                GUILayout.Label("Trajectories" , myStylo );
                EditorGUILayout.Space(4f);
               
                    GUILayout.TextArea("Vérifier bien que pour chaque trajectoires, que le nombre de points soient égals à 3 ou qui suit la formule 5+2x. " 
                                       );
                    EditorGUILayout.Space(2f);
                    GUILayout.TextArea( "Par exemple si votre total de points pour votre trajectoire est égal à 7 : 7 n'est pas égal à trois, mais si on regarde la formule avec x = 1 on obtient 5+2*1 = 7. Votre trajectoire est validé."
                    );
                    EditorGUILayout.Space(4f);
                for (int n = 0; n < strawSo.trajectories.Count; n++)
                {
                 

                   GUI.enabled = false;
                   EditorGUILayout.ColorField(GizmoColor[n],GUILayout.Width(60f) );
                   GUI.enabled = true;
                   


                   EditorGUILayout.PropertyField(serializedObject.FindProperty("trajectories").GetArrayElementAtIndex(n));
                }
                GUI.contentColor = Color.white;
                EditorGUILayout.Space(2f);
                using (new GUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Add Trajectory") )
                                                 {
                                                     
                                                     strawSo.trajectories.Add(new PointsForBezierCurve());
                                                 }
                                                 if (GUILayout.Button("Remove Trajectory") )
                                                 {
                                                     strawSo.trajectories.RemoveAt(strawSo.trajectories.Count-1);
                                                 }
                }
                  
                } 
           
            EditorGUILayout.Space(2f);

          

            GUI.backgroundColor = new Color(2f, 2f, 0f, 0.6f);
            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                if (serializedObject.FindProperty("rateMode").enumValueIndex == 1)
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

                    EditorGUILayout.PropertyField(serializedObject.FindProperty("stepOfCurveParameter"),
                        new GUIContent("Steps of Curve"));

                

                    EditorGUILayout.PropertyField(serializedObject.FindProperty("trajectoriesParameters"),
                        new GUIContent("Trajectories"));


                    EditorGUILayout.Space(2f);


                }
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    public override void OnEditGizmos(SceneView sceneView)
    {base.OnEditGizmos( sceneView);
        try 
        {

            for (int n = 0; n < strawSo.trajectories.Count; n++)
            {
                
                float currentRange = 0;
             
                Handles.color = GizmoColor[n];
                if (strawSo.basePosition.Length != 0)
                {
                    if (strawSo.basePosition[n] != null)
                    {
                        strawSo.basePosition[n] = strawSo.trajectories[n].pointsForBezierCurve[0];

                    }
                }




   currentRange=   Vector3.Distance( strawSo.trajectories[n].pointsForBezierCurve[0]+EmptyGizmos, strawSo.trajectories[n].pointsForBezierCurve[strawSo.trajectories[n].pointsForBezierCurve.Count-1]+EmptyGizmos);
                for (int i = 1; i < strawSo.trajectories[n].pointsForBezierCurve.Count; i += 2)
                {

                    
                    Vector3 start =
                       strawSo.trajectories[n].pointsForBezierCurve[i - 1]+EmptyGizmos;
                    Vector3 middle =
                        strawSo.trajectories[n].pointsForBezierCurve[i]+EmptyGizmos;
                    Vector3 end =
                  strawSo.trajectories[n].pointsForBezierCurve[i + 1]+EmptyGizmos;
             
                    EditorGUI.BeginChangeCheck();

                    start = Handles.PositionHandle(start, quaternion.identity);
                    middle = Handles.PositionHandle(middle, quaternion.identity);
                    end = Handles.PositionHandle(end, quaternion.identity);
                   

                    if (EditorGUI.EndChangeCheck())
                    {

                        Undo.RecordObject(strawSo, "Move Point");
                        EditorUtility.SetDirty(strawSo);
                        strawSo.trajectories[n].pointsForBezierCurve[i - 1] =
                            start-EmptyGizmos;
                        strawSo.trajectories[n].pointsForBezierCurve[i] =
                            middle-EmptyGizmos;
                        strawSo.trajectories[n].pointsForBezierCurve[i + 1] =
                            end-EmptyGizmos;


                    }

                    for (int j = 0; j <= strawSo.stepOfCurve[n]; j++)
                    {

                        Vector3 StepPoint = ExtensionMethods.Bezier(start,
                            middle,
                            end, (j / (float) strawSo.stepOfCurve[n]));
                        Handles.DrawLine(start, StepPoint, 2f);
                       
                        start = StepPoint;



                    }



                   

                }

                strawSo.range = currentRange;

                try
                {
                    Handles.DrawWireDisc(EmptyGizmos+strawSo.trajectories[n].pointsForBezierCurve[0], Vector3.forward,currentRange, 3f);
                }
                catch (Exception e)
                {
                   Handles.DrawWireDisc(EmptyGizmos, Vector3.forward,
                                       currentRange, 3f);
                   

                }
                


            }
        }
        catch
        {
            
        }
             
    }
        
      
}