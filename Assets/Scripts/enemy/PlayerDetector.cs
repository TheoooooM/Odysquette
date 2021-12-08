
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDetector : MonoBehaviour
{
    private EnemyStateManager ESM;
    public float range = 1;
    [SerializeField]
    private StatePatrol patrol;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform aimPatrol;
    private EnemyMovement enemyMovement;
    private bool canPatrol;
    [SerializeField]
    private UnityEvent patrolEvent;




    void Start()
    {
        if (patrol != null)
        {
            Debug.Log("start fonctionne");
               enemyMovement = GetComponent<EnemyMovement>();
                
                    rb = GetComponent<Rigidbody2D>();
                   Debug.Log("start fonctionne");

                   StartCoroutine(WaitGenFinish());
        }
        Debug.Log("start fonctionne");
          ESM = GetComponent<EnemyStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(transform.position, GameManager.Instance.Player.transform.position);
        if (distance <= range && ESM.roomParent.runningRoom)
        {
            ESM.isActivate = true;
            enabled = false;
            if (patrol != null)
            
            enemyMovement.enabled = false;
        }
        else if(patrol != null && canPatrol)
        {
         PlayPatrol();  
        }
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
         

            GraphNode node = AstarPath.active.GetNearest(rb.position +destination).node;
           
      
        
        if (node.Walkable)
        {
            aimPatrol.position = rb.position + destination; 
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
}
