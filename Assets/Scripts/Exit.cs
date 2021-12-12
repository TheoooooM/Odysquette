using System;
using System.Collections;
using UnityEngine;

public class Exit : MonoBehaviour {
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
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.E)) ePress = true;
        else ePress = false;
    }


    private void OnEnable() {
        open = false;
        StartCoroutine("OpenDelay");
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (ePress)
        {
            GameManager.Instance.SetND();
            if (other.CompareTag("Player")) UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
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