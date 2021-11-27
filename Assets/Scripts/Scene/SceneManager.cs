using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Manager = UnityEngine.SceneManagement.SceneManager;

public class SceneManager : MonoBehaviour {
    [SerializeField] private GameObject sceneTransitionGam = null;
    private string nextSceneName = "";


    /// <summary>
    /// Start the launch of the new Scene
    /// </summary>
    /// <param name="sceneName"></param>
    public void StartLoadScene(string sceneName) {
        nextSceneName = sceneName;
        sceneTransitionGam.GetComponent<Animator>().Play("Scene_CloseScene");
    }
    
    
    /// <summary>
    /// Load a scene
    /// </summary>
    /// <param name="scene"></param>
    public void LoadScene() => Manager.LoadScene(nextSceneName);
}
