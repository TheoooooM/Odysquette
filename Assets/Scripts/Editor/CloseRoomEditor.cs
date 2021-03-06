using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CloseRoomManager))]
public class CloseRoomEditor : Editor {
    public override void OnInspectorGUI() {
        CloseRoomManager obj = ((CloseRoomManager) target);
        base.OnInspectorGUI();
        
        if (GUILayout.Button("Update Room")) {
            obj.UpdateCloseRoom(obj.openTop, obj.openRight, obj.openBottom, obj.openLeft);
        }
    }
}