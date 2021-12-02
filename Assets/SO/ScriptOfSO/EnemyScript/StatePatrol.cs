using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
[CreateAssetMenu(fileName = "StatePatrolSO", menuName = "EnnemyState/StatePatrolSO", order = 0)]
public class StatePatrol : StateEnemySO
{
 public float speed;
 public bool toThePlayer;
 public bool toTheOppositePlayer;
 public LayerMask layerMaskForRay;
 //si tu a coché un des deux bool
 public int[] addAngleList;
 public float minDistance;
 public float maxDistance;
 public Vector2 sizeOfDetection;


 public override void StartState(Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary, out bool endStep, EnemyFeedBack enemyFeedBack)
 {  
 Transform aimPatrol =
   (Transform) objectDictionary[ExtensionMethods.ObjectInStateManager.AimPatrol];
    Vector2 destination;
    Rigidbody2D rbPlayer = (Rigidbody2D) objectDictionary[ExtensionMethods.ObjectInStateManager.RigidBodyPlayer];
    Rigidbody2D rb = (Rigidbody2D) objectDictionary[ExtensionMethods.ObjectInStateManager.RigidBodyEnemy];
    aimPatrol.position = rb.position;
    float length = Random.Range(minDistance, maxDistance);
    if (toThePlayer)
    {
     destination = SetUpDestination( rbPlayer.position-rb.position, length);
    }
  
    else if (toTheOppositePlayer)
    {
     destination = SetUpDestination(-(rbPlayer.position-rb.position), length);
    }
    else
    {
     float xrand = Random.Range(-1f, 1f);
     float yrand = Random.Range(-1f, 1f);
     destination = new Vector2(xrand, yrand).normalized * length;
    }
 endStep = false;
 Debug.DrawRay(rb.position, (destination-rb.position).normalized);
 Debug.DrawRay(rb.position, (rbPlayer.position-rb.position).normalized);
 RaycastHit2D hit = Physics2D.BoxCast(rb.position, sizeOfDetection, 0, (destination - rb.position).normalized, length,
  layerMaskForRay);
 ExtDebug.DrawBoxCastBox(rb.position, sizeOfDetection/2, Quaternion.identity, (destination - rb.position).normalized, length, Color.red);
    if (!hit.collider)
    {endStep = true;
  ;
     aimPatrol.position = rb.position + destination;
   
    }
    else
    { 
     
    
     return;
    }
    

 }

 public override void PlayState(Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary, out bool endStep, EnemyFeedBack enemyFeedBack)
 {  
  
  Transform aimPatrol =
   (Transform) objectDictionary[ExtensionMethods.ObjectInStateManager.AimPatrol];
  EnemyMovement enemyMovement =
   (EnemyMovement) objectDictionary[ExtensionMethods.ObjectInStateManager.EnemyMovement];
  Rigidbody2D rb = (Rigidbody2D) objectDictionary[ExtensionMethods.ObjectInStateManager.RigidBodyEnemy];
  enemyMovement.enabled = true;
  enemyMovement.speed = speed;
  enemyMovement.destination = aimPatrol.position;
  if (Vector2.Distance(rb.position, enemyMovement.destination) < 0.3f)
  {

   StartState (objectDictionary,  out endStep, enemyFeedBack);
  }

  endStep = false;
 }

 Vector2 SetUpDestination(Vector3 direction, float length)
 {
  Vector2 destination = new Vector2(); 
  Debug.Log(direction);
  float anglePlayer =Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
  int rand = Random.Range(0, addAngleList.Length);
  rand = addAngleList[rand];
  Debug.Log(anglePlayer);
  
  anglePlayer += rand;
  Debug.Log(anglePlayer);
  anglePlayer *= Mathf.Deg2Rad;

  destination = new Vector2( Mathf.Cos(anglePlayer),Mathf.Sin(anglePlayer) ).normalized * length;
 
  return destination;
 }
}
