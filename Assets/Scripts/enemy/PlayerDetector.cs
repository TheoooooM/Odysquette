using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class PlayerDetector : MonoBehaviour {
    #region VARIABLES

    [SerializeField] private Collider2D detectEnemyArea;

    [SerializeField]
    private EnemyStateManager ESM;
    public float range = 1;
    [SerializeField] private StatePatrol patrol;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform aimPatrol;
    private EnemyMovement enemyMovement;
    private bool canPatrol;

    [SerializeField] private UnityEvent patrolEvent;

    #endregion VARIABLES

    private void OnEnable() {
        if (patrol != null) {
            enemyMovement = GetComponent<EnemyMovement>();
            rb = GetComponent<Rigidbody2D>();
            StartCoroutine(WaitGenFinish());
        }
    }
    
    #region Basic Methods
    private void Start() => ESM = GetComponent<EnemyStateManager>();
    private void Update() => CheckDetection();
    #endregion Basic Methods
    
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    private void BeginPatrol() {
        Vector2 destination;
        aimPatrol.position = rb.position;
        float length = Random.Range(patrol.minDistance, patrol.maxDistance);
        int rand = Random.Range(0, patrol.directionPatrol.Length);
        destination = patrol.directionPatrol[rand] * length;
        GraphNode node = AstarPath.active.GetNearest((Vector2) ESM.spawnPosition + destination).node;

        if (PathUtilities.IsPathPossible(AstarPath.active.GetNearest(rb.position).node, node) && node.Walkable) {
            aimPatrol.position = (Vector2) ESM.spawnPosition + destination;
            PlayPatrol();
        }
        else {
            return;
        }
    }

    private void PlayPatrol() {
        enemyMovement.enabled = true;
        enemyMovement.speed = patrol.speed / 3;
        enemyMovement.destination = aimPatrol.position;
        patrolEvent.Invoke();
        if (Vector2.Distance(rb.position, enemyMovement.destination) < 0.1f) {
            BeginPatrol();
        }
    }

    private IEnumerator WaitGenFinish() {
        yield return new WaitForSeconds(2f);
        BeginPatrol();
        canPatrol = true;
    }

    /// <summary>
    /// Check if the player is in range
    /// </summary>
    public void CheckDetection() {
        float distance = Vector2.Distance(transform.position, GameManager.Instance.Player.transform.position);
        if (distance <= range && ESM.roomParent.runningRoom) {
            EndDetection();
        }
        else if (patrol != null && canPatrol) {
            PlayPatrol();
        }
    }

    /// <summary>
    /// End the detection
    /// </summary>
    public void EndDetection(bool callEnemy = true) {
        if (GetComponent<EnemyStateManager>() != null && callEnemy) {
            foreach (GameObject enemy in GetComponent<EnemyStateManager>().roomParent.ennemiesList) {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance <= range && ESM.roomParent.runningRoom) {
                    if(enemy.GetComponentInChildren<PlayerDetector>() != null) enemy.GetComponentInChildren<PlayerDetector>().EndDetection(false);
                }
            }
        }
        
        ESM.isActivate = true;
        enabled = false;

        //      detectEnemyArea.enabled = false;
        if (patrol != null)
            enemyMovement.enabled = false;
    }
}