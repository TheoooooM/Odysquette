using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
  public float baseTime;
  public int damage;
  private float baseTimer;
  private bool isImpact;
  public float damageTime;
   float damageTimer;
   private Transform child;
   private SpriteRenderer spriteRenderer;
   private float speed;
   private float basePos; 
   private void Start()
   {
     child = transform.GetChild(0);
     spriteRenderer = GetComponent<SpriteRenderer>();
     spriteRenderer.color = Color.white;
     basePos =Vector2.Distance(transform.position, child.position);
     speed = (basePos / baseTime)*Time.deltaTime;
    

   }

   private void OnEnable()
   {
     if(child != null)
     {
       var childPosition = child.localPosition;
       childPosition.y = basePos/2;
       child.localPosition = childPosition;
     }

     baseTimer = 0;
    damageTimer = 0;
    isImpact = false;
  }

  private void Update()
  {

    if (baseTime > baseTimer)
    {
      child.position = Vector2.MoveTowards(child.position, transform.position, speed);
      
      baseTimer +=Time.deltaTime;
      
    }
    else
    {
      isImpact = true;
      if (damageTime > damageTimer)
        {
          spriteRenderer.color = Color.red;
          damageTimer +=Time.deltaTime;
        }
        else
        {
          spriteRenderer.color = Color.white;
          isImpact = false;
          EnemySpawnerManager.Instance.bossShootQueue.Enqueue(gameObject);
          gameObject.SetActive(false);
        }
    }
  }

  private void OnTriggerStay2D(Collider2D other)
  {
    if (isImpact)
    {
      if (other.CompareTag("Player"))
      {
        HealthPlayer.Instance.TakeDamagePlayer(damage);
      }
    }
  }
}
