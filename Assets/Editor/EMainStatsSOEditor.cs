using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(EMainStatsSO))]
public class EMainStatsSOEditor : Editor
{
   
   private int currentTab;
   private bool openConditionPanel;
   private Color backgroundColor = Color.gray;
   private Color backgroundColorBox = new Color(2f, 2f, 2f, 1f);
   private Editor editor;
   private List<string> ButtonStrings = new List<string>
   
   {
      "Statistique Principal"
   };
   GUIStyle TabStyle = new GUIStyle();
   private GUIStyle subTitle = new GUIStyle();
  
   public override void OnInspectorGUI()
   {  
      serializedObject.Update();
      
      EMainStatsSO eMainStatsSo = (EMainStatsSO) target;
      
      GUI.backgroundColor = backgroundColorBox;
  
      using (new GUILayout.VerticalScope(EditorStyles.helpBox))
      {
         
      for (int i = 0; i < eMainStatsSo.stateEnnemList.Count; i++)
      {
         if (ButtonStrings.Count == i+1)
         {
          ButtonStrings.Add("");
         }

         if (eMainStatsSo.stateEnnemList[i] != null)
         {
            
              ButtonStrings[i + 1] = eMainStatsSo.stateEnnemList[i].name;
              
         }
         else
         {
           
 ButtonStrings.RemoveAt(i+1);
            
      
           
            
         }
      
      }
      
     currentTab=  EditorGUILayout.Popup(currentTab,ButtonStrings.ToArray(), TabStyle);
     TabStyle.normal.textColor = Color.white;
      TabStyle.normal.background = Texture2D.linearGrayTexture;
      TabStyle.alignment = TextAnchor.MiddleCenter;
      
          TabStyle.fontSize = 14;
         
          TabStyle.fontStyle = FontStyle.Bold; 

    subTitle.normal.textColor = Color.white;
    
    subTitle.alignment = TextAnchor.MiddleLeft;
    subTitle.fontSize = 13;
         
    subTitle.fontStyle = FontStyle.Bold; 
      
      


   
   GUI.backgroundColor = backgroundColor;
   EditorGUILayout.Space(8f);
   if (currentTab == 0)
   {
         using (new GUILayout.VerticalScope(EditorStyles.helpBox))
                  {
                  GUILayout.Label("Main Stats", subTitle);
                  EditorGUILayout.Space(6f);
                  EditorGUIUtility.labelWidth = 125;
                              backgroundColor = Color.blue;
                              
                                
                                                EditorGUILayout.PropertyField(serializedObject.FindProperty("typeName"));
                                                EditorGUILayout.Space(4f);
                                               EditorGUILayout.PropertyField(serializedObject.FindProperty("maxHealth"));
                                                                                             
                                                                                        
                                                                                                 
                                                                                                
                                                                                                
                                                                                                 
                                                EditorGUILayout.Space(4f);

                                              
                                             
                                             serializedObject.FindProperty("coeifficentUltimateStrawPoints").intValue =
                                                                                                 EditorGUILayout.IntSlider("Ultimate Straw Points",
                                                                                                    serializedObject.FindProperty("coeifficentUltimateStrawPoints").intValue,
                                                                                                    0, 20);
                                             EditorGUILayout.Space(4f);
                                             using (new GUILayout.HorizontalScope())
                                             {
                                                serializedObject.FindProperty("isKnockUp").boolValue=    EditorGUILayout.Toggle("Is Knock Up", serializedObject.FindProperty("isKnockUp").boolValue);
                                                if (serializedObject.FindProperty("isKnockUp").boolValue == true)
                                                {
                                                   EditorGUILayout.PropertyField(serializedObject.FindProperty("dragForKnockUp"));
                                                }
                                                
                                             }
                              }
                              EditorGUILayout.Space(8f);
                              using (new GUILayout.VerticalScope(EditorStyles.helpBox))
                              {
                              GUILayout.Label("State List", subTitle);
                              EditorGUILayout.Space(6f);
                              {
                                 
                                    EditorGUILayout.PropertyField(serializedObject.FindProperty("stateEnnemList"), new GUIContent("State List"));
                                    
                                    
                                 }
                              }
                              EditorGUILayout.Space(8f);
                              GUI.backgroundColor = Color.cyan;
                              using (new GUILayout.VerticalScope(EditorStyles.helpBox))
                              { 
                                 GUILayout.Label("Base State", subTitle);
                                 EditorGUILayout.Space(6f);
                                 EditorGUILayout.PropertyField(serializedObject.FindProperty("baseState"));
                                 EditorGUILayout.Space(4f);
                                 if (eMainStatsSo.baseState != null)
                                 {

                                     editor = CreateEditor(eMainStatsSo.baseState);
                                    DrawFoldoutInspector(eMainStatsSo.baseState, ref editor);
                                  
                                 }
                              }

   }
   else
   {
      GUI.backgroundColor= Color.green;
      using (new GUILayout.VerticalScope(EditorStyles.helpBox))
      {
         editor =  CreateEditor(eMainStatsSo.stateEnnemList[currentTab - 1]);
         DrawFoldoutInspector(eMainStatsSo.stateEnnemList[currentTab - 1], ref editor);
         
         EditorGUILayout.Space(8f);
         GUILayout.Label("Conditions", subTitle);
         EditorGUILayout.Space(6f);
         GUI.backgroundColor = Color.yellow; 
         EditorGUI.BeginChangeCheck();
         openConditionPanel = EditorGUILayout.BeginFoldoutHeaderGroup(openConditionPanel, "Conditions");
         if (openConditionPanel)
         {
             using (new GUILayout.VerticalScope(EditorStyles.helpBox))
                        {
                           
                         
                           using (new GUILayout.HorizontalScope())
                           {
                              eMainStatsSo.stateEnnemList[currentTab - 1].useTimeCondition =
                                 EditorGUILayout.Toggle("Time Condition",
                                    eMainStatsSo.stateEnnemList[currentTab - 1].useTimeCondition);
                              if (eMainStatsSo.stateEnnemList[currentTab - 1].useTimeCondition)
                              {
                               
                                 eMainStatsSo.stateEnnemList[currentTab-1].timeCondition =
                                    EditorGUILayout.FloatField("Time for State", eMainStatsSo.stateEnnemList[currentTab-1].timeCondition);
                           
                              }



                             

                           }

                           if (EditorGUI.EndChangeCheck())
                           {
                                 Undo.RegisterCompleteObjectUndo(eMainStatsSo.stateEnnemList[currentTab - 1], "test");
                                                                     EditorUtility.SetDirty(eMainStatsSo.stateEnnemList[currentTab - 1]);
                           }
                           
                             
                           
       



                        }

           
               

            }

      

      }

        
      
   }
      }



      
      
    serializedObject.ApplyModifiedProperties();DestroyImmediate(editor);
   }


}
