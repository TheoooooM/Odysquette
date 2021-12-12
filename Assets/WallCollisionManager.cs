using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollisionManager : MonoBehaviour
{
    [SerializeField]
    private EnemyStateManager enemyStateManager;

   
    // Start is called before the first frame update
    public virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Walls"))
        {
            enemyStateManager.isContactWall = true;
        }
    }
}
