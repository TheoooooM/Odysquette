using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class DestructableObejct : MonoBehaviour {
    [Header("Object Data")] 
    [SerializeField] private List<GameObject> gamToDesactivate = new List<GameObject>();
    [SerializeField] private BoxCollider2D col = null;
    [SerializeField] private GameObject particleToSpawn = null;
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Bullet")) DisableObjects();
    }

    /// <summary>
    /// Disable all the object
    /// </summary>
    private void DisableObjects() {
        foreach (GameObject gam in gamToDesactivate) {
            gam.SetActive(false);
        }

        if (col != null) col.enabled = false;
        if (particleToSpawn != null) Instantiate(particleToSpawn, transform.position, Quaternion.identity, transform);
    }
}
