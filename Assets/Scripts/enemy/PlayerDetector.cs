using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    private EnemyStateManager ESM;
    public float range = 1;
    
    void Start()
    {
        ESM = GetComponent<EnemyStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(transform.position, GameManager.Instance.Player.transform.position);
        //Debug.Log(distance);
        if (distance <= range)
        {
            ESM.isActivate = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
       Gizmos.DrawWireSphere(transform.position, range);
        
    }
}
