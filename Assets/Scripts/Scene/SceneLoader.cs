using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour {
    [SerializeField] private SceneManager sceneManager = null;

    /// <summary>
    /// Launch the load of the scene
    /// </summary>
    public void LoadScene() => sceneManager.LoadScene();
}
