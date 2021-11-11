using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class RoomClass : MonoBehaviour
{
    [Header("----------------IN GAME------------------")] [SerializeField]
    bool IsClosed; //Est ce que la salle est fermé


    [HideInInspector] public bool OpenTop; //Est ce que la salle est ouverte en haut
    [HideInInspector] public bool OpenBottom; //Est ce que la salle est ouverte à bas
    [HideInInspector] public bool OpenLeft; //Est ce que la salle est ouverte en gauche
    [HideInInspector] public bool OpenRight; //Est ce que la salle est ouverte à droite

    [HideInInspector] public GameObject[] GenTop;
    [HideInInspector] public GameObject[] GenBot;
    [HideInInspector] public GameObject[] GenLeft;
    [HideInInspector] public GameObject[] GenRight;

    [Header("----------Générration Procédurale---------")]
    public NewRoomManager.open[] ExitArray;

    public bool spawned = false;


    [HideInInspector] public int NbrExit; //Ancienne version de la gen a pas enlever
    [HideInInspector] public bool Closing; //Ancienne version de la gen a pas enlever

    private void OnTriggerStay2D(Collider2D other)
    {
            Debug.Log("destroy Room");
        if (!spawned)
        {
            NewRoomManager.instance.reset = true;
            Destroy(gameObject);
        }
    }
}





#if UNITY_EDITOR
[CustomEditor(typeof(RoomClass))]
public class RoomClass_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        DrawDefaultInspector(); // for other non-HideInInspector fields
 
        RoomClass script = (RoomClass)target;
        
        script.OpenLeft = EditorGUILayout.Toggle("Open Left", script.OpenLeft);
        script.OpenRight = EditorGUILayout.Toggle("Open Right", script.OpenRight);
        script.OpenTop = EditorGUILayout.Toggle("Open Top", script.OpenTop);
        script.OpenBottom = EditorGUILayout.Toggle("Open Bot", script.OpenBottom);

        if (script.OpenLeft)
        {
            //script.GenLeft = EditorGUILayout.ObjectField("GenLeft", script.GenLeft, typeof(GameObject), true) as GameObject;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("GenLeft"));
        }
        
        if (script.OpenRight)
        {
            //script.GenRight = EditorGUILayout.ObjectField("GenRight", script.GenRight, typeof(GameObject), true) as GameObject;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("GenRight"));
        }
        
        if (script.OpenTop)
        {
            //script.GenTop = EditorGUILayout.ObjectField("GenTop", script.GenTop, typeof(GameObject), true) as GameObject;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("GenTop"));
        }
        
        if (script.OpenBottom)
        {
            //script.GenBot = EditorGUILayout.ObjectField("GenBot", script.GenBot, typeof(GameObject), true) as GameObject;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("GenBot"));
        }

        serializedObject.ApplyModifiedProperties();

    }
}
#endif
