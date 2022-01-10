using System;
using System.Collections;
using UnityEngine;

public class Exit : MonoBehaviour {
    [SerializeField] private SceneManager sceneManager = null;
    public bool isShop;
    public bool isHubTransition = false;
    public bool ePress;
    private string sceneToLoad;
    
    private bool open;
    

    private void Start() {
        if(sceneManager == null) sceneManager = SceneManager.instance;
        
        if (isShop) {
            sceneToLoad = NeverDestroy.Instance.level == 1 ? "Level 02" : "Boss";
        }
        else if(isHubTransition) sceneToLoad = "Level 01";
        else sceneToLoad = "Shop";
        
        if(!isShop && ! isHubTransition) NeverDestroy.Instance.StartTimer();
        else if(isShop || isHubTransition) NeverDestroy.Instance.PauseTimer();
    }

    private void Update() => ePress = Input.GetKey(KeyCode.E);


    private void OnEnable() {
        open = false;
        StartCoroutine("OpenDelay");
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (ePress) {
            GameManager.Instance.SetND();
            if(!isShop && ! isHubTransition) NeverDestroy.Instance.PauseTimer();
            if (other.CompareTag("Player")) sceneManager.StartLoadScene(sceneToLoad);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) open = true;
    }

    IEnumerator OpenDelay() {
        yield return new WaitForSeconds(1f);
        open = true;
    }
    
    public void OpenNewScene() => sceneManager.StartLoadScene(sceneToLoad);
}