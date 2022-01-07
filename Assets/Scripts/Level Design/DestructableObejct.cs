using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;
using Random = UnityEngine.Random;

public class DestructableObejct : MonoBehaviour {
    [Header("---- OBJECT DATA")] 
    [SerializeField] private List<GameObject> gamToDesactivate = new List<GameObject>();
    [SerializeField] private List<Collider2D> colliderPathRecalculation = new List<Collider2D>();
    [SerializeField] private BoxCollider2D col = null;
    [SerializeField] private bool useTrigger = false;
    [SerializeField] private bool recalculatePath = false;

    [Header("---- OBJECT LIFE")] 
    [SerializeField] private float life = 0;
    private float actualLife = 0;
    
    [Header("---- PARTICLES")] 
    [SerializeField] private GameObject particleToSpawn = null;
    [SerializeField] private Transform particlePos = null;
    private bool hasSpawn = false;

    [Header("---- RESSOURCES DROP")] 
    [SerializeField, Range(0,100)] private int dropRate = 50;
    [SerializeField, Range(1,5)] private int maxRessources = 3;

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
            if (other.gameObject.GetComponent<Bullet>() != null) TakeDamage(other.gameObject.GetComponent<Bullet>().damage);
        }
    }

    /// <summary>
    /// Deal damage to the object
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float damage) {
        actualLife -= damage;
        if(actualLife <= 0) DisableObjects();
    }

    /// <summary>
    /// Disable all the object
    /// </summary>
    private void DisableObjects() {
        foreach (Collider2D col2D in colliderPathRecalculation) {
            Bounds bounds = col2D.bounds;
            AstarPath.active.UpdateGraphs(bounds);
        }
        
        foreach (GameObject gam in gamToDesactivate) {
            gam.SetActive(false);
        }

        int rdm = Random.Range(0, 100);

        if (rdm < dropRate) {
            int ressourcesNumber = Random.Range(0, maxRessources + 1);
            for (int i = 0; i < ressourcesNumber; i++) {
                GameObject GO = Resources.Load<GameObject>("Resource");
                Instantiate(GO, transform.position, Quaternion.identity);
            }
        }
        
        if (col != null) col.enabled = false;
        if (particleToSpawn != null && !hasSpawn) Instantiate(particleToSpawn, particlePos.position, Quaternion.identity, transform);
        hasSpawn = true;
    }
}
