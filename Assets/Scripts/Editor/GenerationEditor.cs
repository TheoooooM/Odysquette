using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Generation))]
public class GenerationEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        Generation gen = ((Generation) target);
        
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Generate Level")) {
            gen.GenerateLevel();
        }

        if (GUILayout.Button("Generate Random Level")) {
            gen.seed = (int) Random.Range(0, 1000000);
            gen.GenerateLevel();
        }
        GUILayout.EndHorizontal();
    }
}
