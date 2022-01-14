using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXSmokeManager : MonoBehaviour
{
    [SerializeField] private float time;
private float timer;

private void OnEnable()
{
  timer = 0;
}

private void Update()
{
  if (time > timer)
  {
    timer += Time.deltaTime;
            
  }
  else
  {
    EnemySpawnerManager.Instance.fxSmokeQueue.Enqueue(gameObject);
    gameObject.SetActive(false);
  }
}
}
