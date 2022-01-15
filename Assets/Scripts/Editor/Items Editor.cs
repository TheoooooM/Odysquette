using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Items))]
[CanEditMultipleObjects]
public class ItemsEditor : Editor
{
    /// <summary>
    /// Draw custom editor
    /// </summary>
    public override void OnInspectorGUI() {
        serializedObject.Update();
        DrawProperty("Item Type", serializedObject.FindProperty("itemType"), true);
        DrawItem();
        GUILayout.Space(4);
        using (new GUILayout.HorizontalScope()) {
            DrawProperty("Ressource Value", serializedObject.FindProperty("ressourceValue"), true);
            DrawProperty("Cost", serializedObject.FindProperty("cost"), true);
        }
        DrawProperty("Shop Canvas", serializedObject.FindProperty("shopCanvas"), true);
        DrawProperty("Ground Canvas", serializedObject.FindProperty("groundCanvas"), true);
        DrawProperty("SO", serializedObject.FindProperty("SO"), true);
        
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawItem() {
       switch ((Items.type) serializedObject.FindProperty("itemType").enumValueIndex) {
           case Items.type.straw:
               DrawProperty("Straw", serializedObject.FindProperty("straw"), true);
               break;
           case Items.type.juice:
               DrawProperty("Effect", serializedObject.FindProperty("effect"), true);
               break;
           case Items.type.life:
               DrawProperty("Health Value", serializedObject.FindProperty("healthValue"), true);
               break;
       }
   }


    private void DrawProperty(string name, SerializedProperty prop, bool drawName) {
        using (new GUILayout.HorizontalScope()) {
            if(drawName) GUILayout.Label(name, GUILayout.Width(120));
            EditorGUILayout.PropertyField(prop, GUIContent.none);
        }
    }
}
