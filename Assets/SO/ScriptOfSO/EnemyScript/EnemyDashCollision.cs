using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDashCollision : MonoBehaviour
{
    public bool inDash;
    public bool isTrigger;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (inDash)
        {
            isTrigger = true;


        }
    }
}
