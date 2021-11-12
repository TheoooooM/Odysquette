using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(EMainStatsSO))]
public class EMainStatsSOEditor : Editor
{
   private int currentTab;
   private Color backgroundColor = Color.gray;
   private List<string> ButtonStrings = new List<string>
   {
      "Statistique Principal", "Etat 1"
   };
   GUIStyle TabStyle = new GUIStyle();
   private GUIStyle subTitle = new GUIStyle();
  
   public override void OnInspectorGUI()
   {  
      serializedObject.Update();
      EMainStatsSO eMainStatsSo = (EMainStatsSO) target;
      for (int i = 0; i < eMainStatsSo.stateEnnemList.Count; i++)
      {
         if (ButtonStrings.Count == i+1)
         {
          ButtonStrings.Add("");
         }

        ButtonStrings[i + 1] = eMainStatsSo.stateEnnemList[i].name;
      }
     currentTab=  EditorGUILayout.Popup(currentTab,ButtonStrings.ToArray(),TabStyle);
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
               GUILayout.Label("Statistiques Principales", subTitle);
               EditorGUILayout.Space(6f);
               EditorGUIUtility.labelWidth = 120;
                           backgroundColor = Color.blue;
                           
                                using (new GUILayout.HorizontalScope())
                                          {
                                             EditorGUILayout.PropertyField(serializedObject.FindProperty("typeName"));
                                             GUILayout.FlexibleSpace();
                                             EditorGUILayout.PropertyField(serializedObject.FindProperty("maxHealth"));
                                             GUILayout.FlexibleSpace();
                                             EditorGUILayout.PropertyField(serializedObject.FindProperty("sprite"));
                                            
                                             
                                          } 
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
                           GUILayout.Label("Listes des States", subTitle);
                           EditorGUILayout.Space(6f);
                           {
                              
                                 EditorGUILayout.PropertyField(serializedObject.FindProperty("stateEnnemList"));
                              }
                           }
}
else
{
   backgroundColor = Color.green;
   Editor editor = CreateEditor(eMainStatsSo.stateEnnemList[currentTab-1]);
   DrawFoldoutInspector(eMainStatsSo.stateEnnemList[currentTab-1], ref editor );
   EditorGUILayout.Space(8f);
   GUILayout.Label("Conditions", subTitle);
   EditorGUILayout.Space(6f);
   using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
   {
      backgroundColor = Color.yellow;
     // eMainStatsSo.healthCondition[currentTab - 1] =
      //   EditorGUILayout.FloatField("Healh Condition", eMainStatsSo.healthCondition[currentTab - 1]);
    //  eMainStatsSo.timeCondition[currentTab - 1] =
        // EditorGUILayout.FloatField("Healh Condition", eMainStatsSo.timeCondition[currentTab - 1]);
   }
}
         
                
         


               
            
      
      serializedObject.ApplyModifiedProperties();
   }
}
