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

    public bool firstGhost;
    public bool contactWall;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (inDash)
        {
            isTrigger = true;


        }
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
