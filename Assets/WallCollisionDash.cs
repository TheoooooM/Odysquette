using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollisionDash : WallCollisionManager
{
    [SerializeField]
    private EnemyDashCollision enemyDashCollision;


    public override void OnCollisionEnter2D(Collision2D other)
    {
        base.OnCollisionEnter2D(other);
        if (other.collider.CompareTag("Walls"))
        {
            enemyDashCollision.contactWall = true;
        }
    }
}
