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
    public bool UseTrigger => useTrigger;
    [SerializeField] private bool recalculatePath = false;

    [Header("---- OBJECT LIFE")] 
    [SerializeField] private float life = 0;
    [SerializeField] private string triggerName = "";
    [SerializeField] private string damageName = "";
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
    /// Deal damage to the object
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float damage) {
        actualLife -= damage;
        if(GetComponent<Animator>() != null) GetComponent<Animator>().SetTrigger(damageName);
        if (actualLife <= 0) {
            if(GetComponent<Animator>() != null) GetComponent<Animator>().SetTrigger(triggerName);
            else DisableObjects();
        }
    }

    /// <summary>
    /// Disable all the object
    /// </summary>
    public void DisableObjects() {
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
