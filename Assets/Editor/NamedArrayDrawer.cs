using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


    
    [CustomPropertyDrawer (typeof(NamedArrayAttribute))]
    public class NamedArrayDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            try {
                int pos = int.Parse(property.propertyPath.Split('[', ']')[1]);
              
                GUI.contentColor = ((NamedArrayAttribute)attribute).colorr[pos];
                GUIStyle myStylo = new GUIStyle();
               
               
                switch (((NamedArrayAttribute) attribute).type)
                {
                    case "float" :
                    {
                       EditorGUI.Slider(rect, property, 0f, 360f, "direction" + pos);
                        
                        break;
                    }
                    case "vector3" :
                    {
                        property.vector3Value = EditorGUI.Vector3Field(rect, "direction" + pos, property.vector3Value );
                        break;
                    }
                    case "int" :
                    {
                        property.intValue = EditorGUI.IntField(rect, "Step for Curve " + pos, property.intValue );
                        break;
                    }
                  
                
                }
             
                GUI.contentColor = Color.white; 
               
            } catch {
                GUI.contentColor = Color.white; 
     
            }
        }
    }

