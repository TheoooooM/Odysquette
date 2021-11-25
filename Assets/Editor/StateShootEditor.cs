using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(StateShootBasic))]
public class StateShootEditor : StateSOEditor
{
   public override void OnInspectorGUI()
   {
      StateShootBasic eStateShootSO = (StateShootBasic) target;
      serializedObject.Update();
      base.OnInspectorGUI();

      subSubTitle.fontSize = 13;

      subSubTitle.fontStyle = FontStyle.Italic;
      EditorGUIUtility.labelWidth = 125;
 
      GUILayout.Label("Specific Stats State", subTitle);
      EditorGUILayout.Space(6f);
      eStateShootSO.openSpecPanel =
         EditorGUILayout.BeginFoldoutHeaderGroup(eStateShootSO.openSpecPanel, "Specific Stats State");
      if (eStateShootSO.openSpecPanel)
      {
         using (new GUILayout.VerticalScope(EditorStyles.helpBox))
         {
            EditorGUILayout.Space(4f);
            GUILayout.Label("Main Specific Stats Shoot State", subSubTitle); 
            EditorGUILayout.Space(4f);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("enemyTypeShoot"));
            EditorGUILayout.Space(4f);
            using (new GUILayout.HorizontalScope())
            {
               EditorGUILayout.PropertyField(serializedObject.FindProperty("damage"));
     
               EditorGUILayout.PropertyField(serializedObject.FindProperty("speedBullet"));
              
               EditorGUILayout.PropertyField(serializedObject.FindProperty("dragRB"));
            }
            

            EditorGUILayout.Space(4f);
            GUILayout.Label("Range", subSubTitle); 
            EditorGUILayout.Space(4f);
            using (new GUILayout.HorizontalScope())
            {
               EditorGUILayout.PropertyField(serializedObject.FindProperty("isAimPlayer"));
              
               EditorGUILayout.PropertyField(serializedObject.FindProperty("isFirstAimPlayer"));
            }
 EditorGUILayout.Space(4f);
            using (new GUILayout.HorizontalScope())
            {
               EditorGUILayout.PropertyField(serializedObject.FindProperty("rangeForShoot"));
               GUILayout.FlexibleSpace(); 
               eStateShootSO.hasRange = EditorGUILayout.Toggle("Has Range",eStateShootSO.hasRange );
               if (eStateShootSO.hasRange)
               {
                  GUILayout.FlexibleSpace();
                                 EditorGUILayout.PropertyField(serializedObject.FindProperty("rangeForBullet"));
               }
            }
            
            EditorGUILayout.Space(4f);
            
            EditorGUILayout.Space(4f);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("extentsRangeDetection"),new GUIContent("Wall Detection Box"));
            EditorGUILayout.Space(4f);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("layerMaskRay"));
   

            
            EditorGUILayout.Space(4f);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("offSetDistance"), new GUIContent("Viewfinder Offset"));
             EditorGUILayout.Space(4f);
           
        
            
            


            EditorGUILayout.Space(4f);
            GUILayout.Label("Delay", subSubTitle);
            EditorGUILayout.Space(4f);
            eStateShootSO.isDelayBetweenShoot = EditorGUILayout.Toggle("Is Delay Between Shoot",eStateShootSO.isDelayBetweenShoot );
            if (eStateShootSO.isDelayBetweenShoot)
            {
                EditorGUILayout.Space(4f);
               EditorGUILayout.PropertyField(serializedObject.FindProperty("delayBetweenShoot"));
            }
            EditorGUILayout.Space(4f);
            eStateShootSO.isDelayBetweenWaveShoot= EditorGUILayout.Toggle("Is Delay Between Wave Shoot",eStateShootSO.isDelayBetweenWaveShoot );
         
            if (eStateShootSO.isDelayBetweenWaveShoot)
            {
                  EditorGUILayout.Space(4f);
                              EditorGUILayout.PropertyField(serializedObject.FindProperty("delayBetweenWaveShoot"));
            }
            EditorGUILayout.Space(4f);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("numberWaveShoot"));
          
   
            EditorGUILayout.Space(4f);
         }

  
      }
      EditorGUILayout.EndFoldoutHeaderGroup();
      EditorGUILayout.Space(6f);
      GUILayout.Label("Other Parameters", subTitle);
      EditorGUILayout.Space(6f);
      EditorGUILayout.PropertyField(serializedObject.FindProperty("directions"));
      EditorGUILayout.Space(4f);
      EditorGUILayout.PropertyField(serializedObject.FindProperty("basePosition"), new GUIContent("OffSet Position"));
      EditorGUILayout.Space(6f);

      serializedObject.ApplyModifiedProperties();
      
      
   }

 
}
