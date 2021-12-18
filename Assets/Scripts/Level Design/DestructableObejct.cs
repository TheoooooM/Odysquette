using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;
using Random = UnityEngine.Random;

public class DestructableObejct : MonoBehaviour {
    [Header("Object Data")] 
    [SerializeField] private List<GameObject> gamToDesactivate = new List<GameObject>();
    [SerializeField] private BoxCollider2D col = null;
    [SerializeField] private GameObject particleToSpawn = null;
    [SerializeField] private Transform particlePos = null;
    private bool hasSpawn = false;
    
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

        int rdm = Random.Range(0, 100);

        if (rdm>80)
        {
            GameObject GO = Resources.Load<GameObject>("ressource");
            Instantiate(GO, transform.position, Quaternion.identity);
        }
        
        if (col != null) col.enabled = false;
        if (particleToSpawn != null && !hasSpawn) Instantiate(particleToSpawn, particlePos.position, Quaternion.identity, transform);
        hasSpawn = true;
    }
}
