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


    private LayerMask currentLayerMask;
    public Vector2 direction;
    [SerializeField]
    private Transform smokeSpawn;
    public bool firstGhost;
    public bool contactWall;
    public Vector2 contact;
    private bool isSmoke; 
    private void Start()
    {
        currentLayerMask = LayerMask.NameToLayer("Player");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (inDash)
        {
            if (other.CompareTag("Player") && !Playercontroller.Instance.InDash)
            {
                contact = (Vector3) other.ClosestPoint(transform.position) ;
                direction =  ((Vector3)contact -transform.position).normalized;
               
                 isTrigger = true;
            }
        }
    }
    public void InstantiateFX()
    {
     
    GameObject  fxPrefab =  EnemySpawnerManager.Instance.SpawnFxDash(contact);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        fxPrefab.transform.rotation = Quaternion.Euler(0,0,angle);
        print("je fais le dash");
    }

    public void SpawnFxSmoke()
    {
        if (!isSmoke)
        {
            StartCoroutine(WaitFrameSmokeAnimation());
        }
    }

    IEnumerator WaitFrameSmokeAnimation()
    {
        yield return new WaitForEndOfFrame();
        
            isSmoke = true;
            GameObject obj = EnemySpawnerManager.Instance.SpawnEnemyPool(smokeSpawn.position, EnemySpawnerManager.Instance.fxSmokeQueue, EnemySpawnerManager.Instance.fxSmokeDashPrefab);
            obj.transform.rotation = smokeSpawn.rotation;
        
    }

    public void CancelSmokeBool() {
        smokeSpawn.localPosition = Vector3.zero; 
        isSmoke = false;
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
