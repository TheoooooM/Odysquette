using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
  public bool isShop;
  private string sceneToLoad;
  
  private bool open;

  private void Start()
  {
    if (isShop)
    {
      if (NeverDestroy.Instance.level == 1) sceneToLoad = "THM_Basic";
      else sceneToLoad = "Boss";
    }
    else sceneToLoad = "Shop";
  }

  private void OnEnable()
  {
    open = false;
    StartCoroutine("OpenDelay");
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    GameManager.Instance.SetND();
    if (other.CompareTag("Player")) UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
  }

  private void OnTriggerExit2D(Collider2D other)
  {
    if(other.CompareTag("Player")) open = true;
  }

  IEnumerator OpenDelay()
  {
    yield return new WaitForSeconds(1f);
    open = true;
  }
}
