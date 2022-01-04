using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;
using Pathfinding;

[CreateAssetMenu(fileName = "StateMouvementSO", menuName = "EnnemyState/StateMouvementSO", order = 0)]
public class StateMouvementSO : StateEnemySO
{
    public float moveSpeed;
    public bool isFastRun;
    public float endFastMove;
    public float startSpinAnimationTime;

    public Vector2 defaultSizeCollider;
    public Vector2 fastMoveSizeCollider;
    public override void StartState(Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary, out bool endStep, EnemyFeedBack enemyFeedBack)
    {
        if (isFastRun)
        {
            BossManager.instance.isBeginSpin = true;

            if (BossManager.instance.inSpinAnimationTime == 0)
            {
                  BossManager.instance.inSpinAnimationTime = startSpinAnimationTime;
                  Debug.Log("test");
            }
              
            if (BossManager.instance.inSpinAnimationTimer > startSpinAnimationTime)
            {
                Debug.Log("tesfdsfqdsfdqsfdqsfdsqfdsqfdsqfdsqfdsqft");
                CheckFeedBackEvent(enemyFeedBack, ExtensionMethods.EventFeedBackEnum.BeginStartState);
                BossManager.instance.enemyStateManager.collider2D.size = fastMoveSizeCollider;

                endStep = true;
                
             
                
                return;
            }
        }
        CheckFeedBackEvent(enemyFeedBack, ExtensionMethods.EventFeedBackEnum.DuringStartState);
        
    
     
        endStep = true;
    }

    public override void PlayState( Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary, out bool endStep, EnemyFeedBack enemyFeedBack)
    {
        EnemyMovement enemyMovement = (EnemyMovement) objectDictionary[ExtensionMethods.ObjectInStateManager.EnemyMovement];
        Rigidbody2D rbPlayer = (Rigidbody2D) objectDictionary[ExtensionMethods.ObjectInStateManager.RigidBodyPlayer];

        bool _endstep = false;
        enemyMovement.speed = moveSpeed;
        if (isFastRun)
        {BossManager.instance.inSpinAnimationTimer = 0;
            BossManager.instance.isBeginSpin = false;
            EnemyStateManager enemyStateManager =
                (EnemyStateManager) objectDictionary[ExtensionMethods.ObjectInStateManager.EnemyStateManager];
         
          
           
            if (enemyStateManager.timerCurrentState > endFastMove)
            {
                endStep = false;
                CheckFeedBackEvent(enemyFeedBack, ExtensionMethods.EventFeedBackEnum.EndFastMove);
                BossManager.instance.enemyStateManager.collider2D.size = defaultSizeCollider;
                return;
            }  
            Transform aimFastMove =
                (Transform) objectDictionary[ExtensionMethods.ObjectInStateManager.AimFastMove];
            aimFastMove.position = rbPlayer.position;

        }
        enemyMovement.enabled = true;
            enemyMovement.destination = rbPlayer.position;
            _endstep = false; 
            CheckFeedBackEvent(enemyFeedBack, ExtensionMethods.EventFeedBackEnum.DuringPlayState);
            endStep = _endstep;
    }



    }

