
using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField] private Collider2D detectEnemyArea;

    private EnemyStateManager ESM;
    public float range = 1;
    [SerializeField]
    private StatePatrol patrol;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform aimPatrol;
    private EnemyMovement enemyMovement;
    private bool canPatrol;

    [SerializeField] private UnityEvent patrolEvent;



    
   
    void Start()
    {
  
        ESM = GetComponent<EnemyStateManager>();
    }

    private void OnEnable()
    {
                if (patrol != null)
                {
               
                       enemyMovement = GetComponent<EnemyMovement>();
                        
                            rb = GetComponent<Rigidbody2D>();
                          
        
                           StartCoroutine(WaitGenFinish());
                }
    }

    // Update is called once per frame
    void Update()
    {
        CheckDetection();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
       Gizmos.DrawWireSphere(transform.position, range);
 
        
    }

    void BeginPatrol()
    {
        Vector2 destination;
        aimPatrol.position = rb.position;
        float length = Random.Range(patrol.minDistance, patrol.maxDistance);
        int rand = Random.Range(0, patrol.directionPatrol.Length);
     
        destination =  patrol.directionPatrol[rand]* length;
        
        GraphNode node = AstarPath.active.GetNearest((Vector2)ESM.spawnPosition +destination).node;
           
        if (node.Walkable)
        {
            aimPatrol.position =  (Vector2)ESM.spawnPosition+ destination; 
            PlayPatrol();
        }
        else
        {
            return;
        }
    }

    void PlayPatrol()
    {
       
        enemyMovement.enabled = true;
        enemyMovement.speed = patrol.speed;
        enemyMovement.destination = aimPatrol.position;
        patrolEvent.Invoke();
        if (Vector2.Distance(rb.position, enemyMovement.destination) < 0.1f)
        {
            BeginPatrol();
        }
    }

    IEnumerator WaitGenFinish()
    {
        yield return new WaitForSeconds(2f);
        BeginPatrol();
        canPatrol = true;
    }

    public void CheckDetection()
    {
        float distance = Vector2.Distance(transform.position, GameManager.Instance.Player.transform.position);
        if (distance <= range && ESM.roomParent.runningRoom)
        { EndDetection();
        }
        else if(patrol != null && canPatrol)
        { PlayPatrol();  
        }
    }

    public void EndDetection()
    {
        ESM.isActivate = true;
        enabled = false;
        
        //      detectEnemyArea.enabled = false;
        if (patrol != null)
            enemyMovement.enabled = false;
    }
}
