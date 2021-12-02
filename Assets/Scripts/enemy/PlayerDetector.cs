
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

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
        //Debug.Log(distance);
        if (distance <= range)
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
            Debug.Log(destination);

            GraphNode node = AstarPath.active.GetNearest(rb.position +destination).node;
           
      
        ExtDebug.DrawBoxCastBox(rb.position,  patrol.sizeOfDetection/2, Quaternion.identity, (destination - rb.position).normalized, length, Color.red);
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
        Debug.Log("testzz");
        enemyMovement.enabled = true;
        enemyMovement.speed = patrol.speed;
        enemyMovement.destination = aimPatrol.position;
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
