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
    public bool isMovementToSpawn;

    public override void StartState(Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary, out bool endStep, EnemyFeedBack enemyFeedBack)
    {
     
        EnemyMovement enemyMovement =
            (EnemyMovement) objectDictionary[ExtensionMethods.ObjectInStateManager.EnemyMovement];
        CheckFeedBackEvent(enemyFeedBack, ExtensionMethods.EventFeedBackEnum.DuringStartState);
        enemyMovement.enabled = true;
     
        endStep = true;
    }

    public override void PlayState( Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary, out bool endStep, EnemyFeedBack enemyFeedBack)
    {
     
        EnemyMovement enemyMovement =
            (EnemyMovement) objectDictionary[ExtensionMethods.ObjectInStateManager.EnemyMovement];
        Rigidbody2D rbPlayer = (Rigidbody2D) objectDictionary[ExtensionMethods.ObjectInStateManager.RigidBodyPlayer];

       
        bool _endstep = false;
        enemyMovement.speed = moveSpeed;
        if (isMovementToSpawn)
        {  Transform spawnerTransform =
                      (Transform) objectDictionary[ExtensionMethods.ObjectInStateManager.Spawner]; Rigidbody2D rbEnemy = (Rigidbody2D) objectDictionary[ExtensionMethods.ObjectInStateManager.RigidBodyEnemy];
            enemyMovement.enabled = true;
            enemyMovement.destination = spawnerTransform.position;
            if (Vector2.Distance(rbEnemy.position, spawnerTransform.position)<0.1f)
            {
                enemyMovement.enabled = false;
                Debug.Log("baa");
                _endstep = true;
            }
           
        }
        else
        { 
            enemyMovement.enabled = true;
            enemyMovement.destination = rbPlayer.position;
            _endstep = false; 
            CheckFeedBackEvent(enemyFeedBack, ExtensionMethods.EventFeedBackEnum.DuringPlayState);
        }


        endStep = _endstep;
    }



    }

