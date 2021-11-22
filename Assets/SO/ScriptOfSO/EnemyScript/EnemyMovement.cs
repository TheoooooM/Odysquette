using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private bool reachedEndOfPath;

    private Seeker seeker;

    private Rigidbody2D rb;
   
    private Path path;
 
    private Transform tranform; 
    public int currentWaypoint = 0;
    private float nextWaypointDistance = 0.1f;
    private EnemyStateManager enemyStateManager;
     public Vector3 destination;
    public float speed;
    public Vector2 lastVelocity;
    public bool createPath;

    public Vector2 currentWaypointDirection;
    // Start is called before the first frame update
    void Start()
    {
        tranform = gameObject.transform;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        enemyStateManager = GetComponent<EnemyStateManager>();

    }

    void UpdatePath()
    {
        createPath = true;
        if(seeker.IsDone())
        seeker.StartPath(rb.position, destination, OnPathComplete);
    }
    private void OnEnable()
    {
        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    private void OnDisable()
    {
        CancelInvoke();
        rb.velocity = Vector2.zero;
    }

    void OnPathComplete(Path p)
    {
        
        if (!p.error)
        {
           
            path = p;
           path.vectorPath[0] = tranform.position;
            currentWaypoint = 0;
        }
    }

    private void Update()
    {
        if ((enemyStateManager.IsCurrentStartPlayed || enemyStateManager.IsCurrentStatePlayed) &&
            enemyStateManager.EMainStatsSo.stateEnnemList[enemyStateManager.indexCurrentState].duringDefaultState)
        {
            enabled = true;
        }
       else if (enemyStateManager.IsCurrentStartPlayed || enemyStateManager.IsCurrentStatePlayed)
            enabled = false;
    }
  Vector2 direction = new Vector2();
    // Update is called once per frame
    void FixedUpdate()
    {
       
        if (path == null)
            return;
        
        if (currentWaypoint >= path.vectorPath.Count)
        {
         return;
        }
   
    
       
        
        if (direction != Vector2.zero)
        {
            lastVelocity = direction;
        }
        if (!enemyStateManager.isDragKnockUp)
        {
            if (enemyStateManager.isInWind || enemyStateManager.isConvey)
            {
                 rb.velocity = lastVelocity * speed+enemyStateManager.windDirection+ enemyStateManager.conveyBeltSpeed;
            }
            else
            {
                rb.velocity = lastVelocity * speed;
            }
        }
   
       
        
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (currentWaypoint != path.vectorPath.Count - 1)
        {
               if (distance < nextWaypointDistance)
                    {
                        
                        currentWaypoint++;
                        direction = ( (Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
              
                    }
        }
     
        
        

   
    }
}
