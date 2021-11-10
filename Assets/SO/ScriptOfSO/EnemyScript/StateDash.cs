using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;

[CreateAssetMenu(fileName = "StateDashSO", menuName = "EnnemyState/StateDashSO", order = 0)]
public class StateDash : StateEnemySO
{
    public AnimationCurve animationCurve;
    public float maxspeed;
    public float rangeDetection;
   
 
    public Rect rectExtents;
   public Vector3 extentsRangeDetection;

    private void OnValidate()
    {
        extentsRangeDetection = new Vector3(rectExtents.width, rectExtents.height);
    }

    public override bool CheckCondition(Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary)
    {
               Rigidbody2D rb =
                     (Rigidbody2D) objectDictionary[ExtensionMethods.ObjectInStateManager.RigidBodyEnemy];
               Rigidbody2D rbPlayer = (Rigidbody2D) objectDictionary[ExtensionMethods.ObjectInStateManager.RigidBodyPlayer];
               if (Vector2.Distance(rb.position, rbPlayer.position) < rangeDetection)
               {
                   
                   Vector2 direction = (rbPlayer.position - rb.position);
                   RaycastHit2D hit = Physics2D.BoxCast(rb.position, extentsRangeDetection, 0,
                       direction.normalized, direction.magnitude);
               
                   if(hit.collider.gameObject.layer == 9 )
                         {
                                    Debug.Log("suis je la");   
                             Transform target = (Transform) objectDictionary[ExtensionMethods.ObjectInStateManager.AimDash];
                             target.position  = rbPlayer.position; 
                             target.gameObject.SetActive(true);
                                          
                                                
                             Debug.DrawRay(rb.position, direction*rangeDetection);
                             return true;
                         }
                  }
          
                
                   
               

               return false;
    }
    

  
    public override void StartState(Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary, out bool endStep)
    {
       

       
        Rigidbody2D rb = (Rigidbody2D) objectDictionary[ExtensionMethods.ObjectInStateManager.RigidBodyEnemy];
       
        rb.velocity = Vector2.zero;
        endStep = false;
    }
    public override void PlayState( Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary, out bool endStep)
    {
        Debug.Log("test");
        
       Transform transformDash = (Transform) objectDictionary[ExtensionMethods.ObjectInStateManager.AimDash];
        Rigidbody2D rb =
            (Rigidbody2D) objectDictionary[ExtensionMethods.ObjectInStateManager.RigidBodyEnemy]; 
        Vector2 direction = ((Vector2)transformDash.position - rb.position);
        float factorSpeed =direction.magnitude/ rangeDetection ;
        float speed=  animationCurve.Evaluate(factorSpeed)*maxspeed;

        rb.velocity = direction.normalized * speed;
       
        if (Vector2.Distance(rb.position, transformDash.position) < 0.2f)
        {
           
           transformDash.gameObject.SetActive(false);
            endStep = true;
            return;
        }
        

        endStep = false;
    }

 

}
