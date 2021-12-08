using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyFeedBackDeath : MonoBehaviour
{
 public UnityEvent deathEvent;
 public GameObject ParentToDestroy;
 public void DestroyOtherObject(float delay)
 {
  Destroy(ParentToDestroy, delay);
 }
}
