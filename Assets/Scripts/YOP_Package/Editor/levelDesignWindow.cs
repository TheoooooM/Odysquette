using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
/// <summary>
/// Create an editor window for my Level Design
/// </summary>
public class levelDesignWindow : EditorWindow {
    private static GameObject newPrefab;
    private static List<GameObject> sceneObj = new List<GameObject>();
    private bool openFoldoutSelection;
    
    /// <summary>
    /// Get or Create the Window
    /// </summary>
    [MenuItem("Tools/Level Design/Open Window")]
    private static void Init() {
        levelDesignWindow window = GetWindow(typeof(levelDesignWindow)) as levelDesignWindow;
        if (window != null) window.Show();
    }

    #region ChangeSelection

    private void OnEnable() => Selection.selectionChanged += Repaint;
    private void OnDisable() => Selection.selectionChanged -= Repaint;
    
    #endregion ChangeSelection
    
    /// <summary>
    /// Function which allow to draw GUI in the window
    /// </summary>
    private void OnGUI() {
        DrawChangePredabBox();
        GUILayout.Space(4);
        DrawActivationGamBox();
    }

    #region DrawWindow
    /// <summary>
    /// Draw the active GameObject
    /// </summary>
    private void DrawChangePredabBox() {
        using (new GUILayout.VerticalScope(EditorStyles.helpBox)) {
            //Draw the title of the Box
            GUI.skin.label.fontSize = 10;
            GUILayout.Label("Change Selection by Prefab :");
            GUI.skin.label.fontSize = 12;

            using (new GUILayout.HorizontalScope()) {
                GUILayout.Label("New Prefab");
                newPrefab = (GameObject) EditorGUILayout.ObjectField(newPrefab, typeof(GameObject), false);
                if (newPrefab != null) if (PrefabUtility.GetPrefabAssetType(newPrefab) == PrefabAssetType.NotAPrefab) newPrefab = null;
            }

            using (new GUILayout.VerticalScope(EditorStyles.helpBox)) {
                openFoldoutSelection = EditorGUILayout.Foldout(openFoldoutSelection, $"Selection ({Selection.gameObjects.Length} {(Selection.gameObjects.Length > 0 ? "objects" : "object")} selected)", true);
                if (openFoldoutSelection) {
                    List<GameObject> selectionlist = new List<GameObject>(Selection.gameObjects);

                    GUI.enabled = false;
                    for (int i = 0; i < selectionlist.Count; i++) {
                        EditorGUILayout.ObjectField(selectionlist[i], typeof(GameObject), true);
                    }
                    GUI.enabled = true;
                }
            }

            if (GUILayout.Button("Change Prefab (LShift + E)")) {
                ChangePrefab();
            }
        }
    }

    /// <summary>
    /// Draw a box where the user can put a gameObject and use an Input to enable/disable the object
    /// </summary>
    private void DrawActivationGamBox() {
        using (new GUILayout.VerticalScope(EditorStyles.helpBox)) {
            if(sceneObj.Count == 0) sceneObj.Add(null);
            
            //Draw the title of the Box
            GUI.skin.label.fontSize = 10;
            GUILayout.Label("Change Selection by Prefab :");
            GUI.skin.label.fontSize = 12;

            using (new GUILayout.VerticalScope()) {
                for (int i = 0; i < sceneObj.Count; i++) {
                    using (new GUILayout.HorizontalScope()) {
                        GUILayout.Label($"Scene Gam {i} : ");
                        sceneObj[i] = (GameObject) EditorGUILayout.ObjectField(sceneObj[i], typeof(GameObject), true);
                    }
                }
            }
            
            using (new GUILayout.HorizontalScope()) {
                if (GUILayout.Button("-", GUILayout.Width(25))) sceneObj.RemoveAt(sceneObj.Count - 1);
                if (GUILayout.Button("+", GUILayout.Width(25))) sceneObj.Add(null);
                if (GUILayout.Button("Change Object State (LShift + R)")) ChangeObjectSceneState();
            }
            
            
        }
    }

    #region ChangePrefabs
    [MenuItem("Tools/Level Design/Change Prefab #E")]
    private static void ChangePrefab() {
        if (newPrefab == null) {
            EditorUtility.DisplayDialog("Error when changing prefab", "You have to put a prefab in the editor Window to make the switch.", "continue");
            if(GetWindow<levelDesignWindow>() == null) Init();
            return;
        }
        else if (Selection.gameObjects.Length == 0) {
            EditorUtility.DisplayDialog("Error when changing prefab", "You have to select at least one gameObejct to perform this action.", "continue");
            return;
        }
        
        List<GameObject> selectionlist = new List<GameObject>(Selection.gameObjects);
        for (int i = 0; i < selectionlist.Count; i++) {
            GameObject actualSelectedGam = selectionlist[i];
            GameObject newGam = (GameObject) PrefabUtility.InstantiatePrefab(newPrefab);
            
            //Update the data of the new gameObject
            Undo.RegisterCreatedObjectUndo(newGam, "Undo the creation of the new prefab");
            
            //Conserve the children
            List<GameObject> childLists = new List<GameObject>();
            for (int child = 0; child < actualSelectedGam.transform.childCount; child++) {
                childLists.Add(actualSelectedGam.transform.GetChild(child).gameObject);
            }
            
            updateData(newGam, actualSelectedGam, childLists);
            childLists.Clear();

            
        }
    }

    /// <summary>
    /// Update the valeu of the new prefab
    /// </summary>
    /// <param name="newGam"></param>
    /// <param name="previousGam"></param>
    private static void updateData(GameObject newGam, GameObject previousGam, List<GameObject> childLists) {
        newGam.transform.parent = previousGam.transform.parent;
        newGam.transform.SetSiblingIndex(previousGam.transform.GetSiblingIndex());
        newGam.transform.SetPositionAndRotation(previousGam.transform.position, previousGam.transform.rotation);
        newGam.transform.localScale = previousGam.transform.localScale;
        
        //Change child Parent
        for (int child = 0; child < childLists.Count; child++) {
            Undo.SetTransformParent(childLists[child].transform, newGam.transform, "Undo changement parent transform");
        }
        
        //Undo the operation
        Undo.DestroyObjectImmediate(previousGam);
        Selection.activeGameObject = newGam;
    }
    #endregion ChangePrefabs
    
    #region Activation Gameobject
    [MenuItem("Tools/Level Design/Change SceneObject State #R")]
    private static void ChangeObjectSceneState() {
        if (sceneObj.Count == 0) {
            EditorUtility.DisplayDialog("Error when changing Object State", "You have to put a GameObject in the editor Window to change the state.", "continue");
            if(GetWindow<levelDesignWindow>() == null) Init();
            return;
        }

        foreach (GameObject gam in sceneObj) { if(gam != null) gam.SetActive(!gam.activeSelf); }
    }
    #endregion Activation Gameobject
    
    #region Delete Duplicate Name
    [MenuItem("Tools/Level Design/Delete Duplicate Name Extra #Z")]
    private static void DeleteDuplicateExtra() {
        if (Selection.activeGameObject != null) {
            foreach (GameObject gam in Selection.gameObjects) {
                gam.name = gam.name.Substring(0, gam.name.Length - 4);

            }
        }
    }
    #endregion Delete Duplicate Name
    
    #endregion DrawWindow
}
#endif
