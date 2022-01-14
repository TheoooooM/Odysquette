using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollisionDash : WallCollisionManager
{
    [SerializeField]
    private EnemyDashCollision enemyDashCollision;

    [SerializeField] private Transform aimPoint;

    [SerializeField] private float angle;

    bool CheckAngle(Vector3 point)
    {
        Vector2 directionDash = aimPoint.position - transform.position;
        Vector2 directionCollider = point - transform.position;
        if (Vector2.Angle(directionCollider, directionDash) < angle)
        {
            Debug.Log(angle);
            return true;
        }
        Debug.Log(angle);
        return false;
    }

    public override void OnCollisionEnter2D(Collision2D other)
    {
        base.OnCollisionEnter2D(other);
        if (other.collider.CompareTag("Walls"))
        {
            if (enemyDashCollision.inDash)
            {
                if( CheckAngle(other.contacts[0].point))
            enemyDashCollision.contactWall = true;
                
            }
        }
    }
}
