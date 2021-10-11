using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class enemy : MonoBehaviour
{
    [SerializeField] private bool distanceEnemy;
    [SerializeField] private float range;

    private Transform target;
    private NavMeshAgent agent;
    
    
    
    void Start()
    {
        if (!distanceEnemy)
        {
            range = 1;
        }
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        agent.stoppingDistance = range;
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(target.position);
        if (agent.velocity == Vector3.zero)
        {
            Debug.Log("shoot");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, range*0.4f);
    }
}
