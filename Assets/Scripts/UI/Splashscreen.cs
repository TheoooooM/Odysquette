using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Splashscreen : MonoBehaviour {
    private bool canPressBtn = false;

    private void Update() {
        if (Input.anyKey && canPressBtn) {
            GetComponent<Animator>().SetTrigger("Scene");
        }
    }

    public void CanPressKey() => canPressBtn = true;
    
    public void LoadScene() => UnityEngine.SceneManagement.SceneManager.LoadScene("HUB");
}
