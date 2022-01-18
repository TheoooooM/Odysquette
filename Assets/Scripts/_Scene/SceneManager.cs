using UnityEngine;
using Manager = UnityEngine.SceneManagement.SceneManager;

public class SceneManager : MonoBehaviour {
    public static SceneManager instance = null;

    private void Awake() {
        if(instance == null) instance = this;
        if(shop) StartIdleOpen();
    }

    [SerializeField] private GameObject sceneTransitionGam = null;
    [SerializeField] private bool shop = false;
    private string nextSceneName = "";
    
    private void Start() { if(shop) OpenScene(); }


    /// <summary>
    /// Start the launch of the new Scene
    /// </summary>
    /// <param name="sceneName"></param>
    public void StartLoadScene(string sceneName) {
        GameManager.Instance.SetND();
        nextSceneName = sceneName;
        sceneTransitionGam.GetComponent<Animator>().Play("Scene_CloseScene");
    }
    
    
    /// <summary>
    /// Load a scene
    /// </summary>
    /// <param name="scene"></param>
    public void LoadScene() => Manager.LoadScene(nextSceneName);

    public void StartIdleOpen() => sceneTransitionGam.GetComponent<Animator>().Play("Scene_IdleOpen");
    public void OpenScene() => sceneTransitionGam.GetComponent<Animator>().SetTrigger("OpenIdle");
}
