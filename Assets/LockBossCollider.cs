using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockBossCollider : MonoBehaviour {
    [SerializeField] private List<PlayerDetectionEvent>  doors = new List<PlayerDetectionEvent>();
    [SerializeField] private Animator dialog = null;
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (!BossManager.instance.isTriggerDoor)
            if (other.CompareTag("Player")) {
                BossManager.instance.isTriggerDoor = true;
                EndDoor();
                dialog.SetTrigger("StartDialog");
            }
    }

    /// <summary>
    /// Close the doors
    /// </summary>
    private void EndDoor() {
        foreach (PlayerDetectionEvent door in doors) {
            door.DisableDoor();
        }
        enabled = false;
    }
}