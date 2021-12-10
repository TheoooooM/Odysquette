using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class DestructableObejct : MonoBehaviour {
    [Header("---- OBJECT DATA")] 
    [SerializeField] private List<GameObject> gamToDesactivate = new List<GameObject>();
    [SerializeField] private BoxCollider2D col = null;
    [SerializeField] private bool useTrigger = false;

    [Header("---- OBJECT LIFE")] 
    [SerializeField] private float life = 0;
    private float actualLife = 0;
    
    [Header("---- PARTICLES")] 
    [SerializeField] private GameObject particleToSpawn = null;
    [SerializeField] private Transform particlePos = null;
    private bool hasSpawn = false;


    private void Start() {
        actualLife = life;
    }

    /// <summary>
    /// When a bullet enter in trigger
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other) {
        if (!useTrigger) return;
        
        if (other.CompareTag("Bullet")) {
            if (other.GetComponent<Bullet>() != null) actualLife -= other.GetComponent<Bullet>().damage;
            if(actualLife <= 0) DisableObjects();
        }
    }

    /// <summary>
    /// When a bullet enter in collision
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter2D(Collision2D other) {
        if (useTrigger) return;
        if (other.gameObject.CompareTag("Bullet")) {
            if (other.gameObject.GetComponent<Bullet>() != null) actualLife -= other.gameObject.GetComponent<Bullet>().damage;
            if(actualLife <= 0) DisableObjects();
        }
    }

    /// <summary>
    /// Disable all the object
    /// </summary>
    private void DisableObjects() {
        foreach (GameObject gam in gamToDesactivate) {
            gam.SetActive(false);
        }

        if (col != null) col.enabled = false;
        if (particleToSpawn != null && !hasSpawn) Instantiate(particleToSpawn, particlePos.position, Quaternion.identity, transform);
        hasSpawn = true;
    }
}
