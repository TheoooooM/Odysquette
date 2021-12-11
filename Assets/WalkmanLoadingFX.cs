using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkmanLoadingFX : MonoBehaviour
{
   private float timer;
   [SerializeField] private float time;

   private void OnEnable()
   {
      timer = 0;
   }

   private void Update()
   {
      timer += Time.deltaTime;
      if (time <= timer)
         
      { 
         gameObject.SetActive(false);
         PoolManager.Instance.enemypoolDictionary[ExtensionMethods.EnemyTypeShoot.WalkmanLoading].Enqueue(gameObject);
      }
   }
}
