using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockBossCollider : MonoBehaviour
{
 
 private void OnTriggerEnter2D(Collider2D other)
 {
  if(!BossManager.instance.isTriggerDoor)
   if (other.CompareTag("Player"))
   {
       
       BossManager.instance.isTriggerDoor = true;
   }
 }
}
