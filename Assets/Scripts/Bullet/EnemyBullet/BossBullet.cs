using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
  public float baseTime;
  public int damage;
  private float baseTimer;

  public float damageTime;
   float damageTimer;
   [SerializeField]
   private Transform impact;

   [SerializeField] private GameObject explosionRocket;
  [SerializeField]
   private LayerMask currentLayerMask;
   private float speed;
   [SerializeField]
   private Transform rocket;
   [SerializeField]
   private Transform basePos; 
  
   private void Start()
   {
     speed = (Vector2.Distance(rocket.position, impact.position)/baseTime);
   }

   private void OnEnable()
   {
     if(speed != 0 )
     {

       rocket.position = basePos.position;
     }
     rocket.gameObject.SetActive(true);
explosionRocket.SetActive(false);
     baseTimer = 0;
    damageTimer = 0;
   
  }

   private void OnDrawGizmos()
   {
 Gizmos.DrawSphere(impact.position, 0.80f);
   }

   private void Update()
  {

    if (baseTime > baseTimer)
    {
      rocket.position = Vector2.MoveTowards(rocket.position, 
        impact.position, speed*Time.deltaTime);
      
      baseTimer +=Time.deltaTime;
      
    }
    else
    {
     
      if (damageTime > damageTimer)
      {
        

        if(Physics2D.OverlapCircle(impact.position, 0.88f, currentLayerMask ))
        {
          if(!Playercontroller.Instance.InDash)
          HealthPlayer.Instance.TakeDamagePlayer(1);
        }
        rocket.gameObject.SetActive(false);
        explosionRocket.SetActive(true);
          damageTimer +=Time.deltaTime;
        }
        else
        {
        
         
          EnemySpawnerManager.Instance.bossShootQueue.Enqueue(gameObject);
          gameObject.SetActive(false);
        }
    }
  }




}
