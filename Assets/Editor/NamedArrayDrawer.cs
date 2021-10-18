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

                if (((NamedArrayAttribute) attribute).withColor)
                {
                        Rect tempoRect = new Rect()
                        {
                            x = rect.position.x+2.5f, y = rect.position.y+2.5f, width = rect.height-5, height = rect.height-5
                        };
                        EditorGUI.DrawRect(tempoRect, ((NamedArrayAttribute)attribute).colorr[pos]);
                        rect.position += new Vector2(rect.height + 5, 0);
                        rect.size -= new Vector2(rect.height + 5, 0);
                        
                 
                }
                
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

