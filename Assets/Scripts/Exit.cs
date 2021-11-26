using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
  private bool open;

  private void OnEnable()
  {
    open = false;
    StartCoroutine("OpenDelay");
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.CompareTag("Player")) SceneManager.LoadScene("Shop");
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
