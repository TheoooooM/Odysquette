using System;
using System.Collections;
using UnityEngine;

public class Exit : MonoBehaviour {
    [SerializeField] private SceneManager sceneManager = null;
    public bool isShop;
    public bool ePress;
    private string sceneToLoad;
    
    private bool open;
    

    private void Start() {
        if (isShop) {
            if (NeverDestroy.Instance.level == 1) sceneToLoad = "YOP_Basic";
            else sceneToLoad = "Boss";
        }
        else sceneToLoad = "Shop";
        
        if(sceneManager == null) sceneManager = SceneManager.instance;
    }

    private void Update() => ePress = Input.GetKey(KeyCode.E);


    private void OnEnable() {
        open = false;
        StartCoroutine("OpenDelay");
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (ePress)
        {
            GameManager.Instance.SetND();
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
}