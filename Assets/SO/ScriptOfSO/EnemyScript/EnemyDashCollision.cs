using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDashCollision : MonoBehaviour
{
    public bool inDash;
    public bool isTrigger;
    [SerializeField] private Vector3 lastImageposition;
    [SerializeField]
    private float distanceBetweenImage;



    public Vector2 direction;

    public bool firstGhost;
    public bool contactWall;
    public Vector2 contact;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (inDash)
        {
            if (other.CompareTag("Player"))
            {
                
               contact =  other.ClosestPoint(transform.position);
              
                direction =  (Vector3)contact -transform.position;
               
                 isTrigger = true;
            }
        }
    }
    public void InstantiateFX()
    {
    GameObject  fxPrefab =  EnemySpawnerManager.Instance.SpawnFxDash(contact);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        fxPrefab.transform.rotation = Quaternion.Euler(0,0,angle);
    }
    



    public void GhostDash()
    {
        if (!firstGhost)
        {
            EnemySpawnerManager.Instance.GetFromPool(transform);
            firstGhost = true;
        }
        if (Vector3.Distance(transform.position , lastImageposition) > distanceBetweenImage)
        {
            EnemySpawnerManager.Instance.GetFromPool(transform);
            lastImageposition = transform.position;
        }
      
    }

    public void CancelGhostDash()
    {
        firstGhost = false;
    }
}
