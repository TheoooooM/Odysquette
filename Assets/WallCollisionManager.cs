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
            enemyStateManager.directionWall = other.contacts[0].normal; 
            enemyStateManager.isContactWall = true;
        }
    }
    public virtual void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.CompareTag("Walls"))
        {
            enemyStateManager.directionWall = Vector3.zero; 
            enemyStateManager.isContactWall = false;
        }
    }
}
