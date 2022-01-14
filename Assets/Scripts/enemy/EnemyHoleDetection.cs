using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHoleDetection : MonoBehaviour {
    [SerializeField] private EnemyStateManager enemyState = null;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Hole")) {
            enemyState.OnDeath(true);
        }
    }
}
