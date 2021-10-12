using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class enemy : MonoBehaviour
{
    [SerializeField] private bool distanceEnemy;
    [SerializeField] private float range = 1;
    [SerializeField] private float speed = 3.5f;
    [SerializeField] private float freezeModifier = 0.5f;

    private Transform target;
    private NavMeshAgent agent;

    public float freezeTime = 0;
    
    
    
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
        agent.speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(target.position);
        if (agent.velocity == Vector3.zero)
        {
            Debug.Log("shoot");
        }

        if (freezeTime > 0)
        {
            agent.speed = speed * freezeModifier;
            freezeTime -= Time.deltaTime * 2;
        }
        else
        {
            agent.speed = speed;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
