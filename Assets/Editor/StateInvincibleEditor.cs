using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(StateInvincible))]
public class StateInvincibleEditor : StateSOEditor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.OnInspectorGUI();
        serializedObject.ApplyModifiedProperties();
    }
}
