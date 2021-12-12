using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour {
    private SceneManager sceneManager = null;

    private void Start() => sceneManager = SceneManager.instance;

    /// <summary>
    /// Launch the load of the scene
    /// </summary>
    public void LoadScene() => sceneManager.LoadScene();
}
