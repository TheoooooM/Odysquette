using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CloseRoomManager))]
public class CloseRoomEditor : Editor {
    public override void OnInspectorGUI() {
        CloseRoomManager obj = ((CloseRoomManager) target);
        base.OnInspectorGUI();
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Update Room")) {
            obj.UpdateCloseRoom(obj.openTopTest, obj.openRightTest, obj.openBottomTest, obj.openLeftTest, false);
        }
        if (GUILayout.Button("Make Blue Buildings")) {
            obj.MakeEveryBuildingBlue();
        }
        GUILayout.EndHorizontal();
    }
}