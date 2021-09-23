using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    
   [SerializeField] float BulletSpeed;
   private BulletStat Scriptable;
    int Timer = 0;

    void Update()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.right * (BulletSpeed* 0.0001f), ForceMode2D.Impulse); // met une force sur la paille
        
        //------------------------ Debug pour faire disparaitre la balle --------------------
        Timer= Timer + 1 ;
        if (Timer == 500)
        {
            Timer = 0;
            gameObject.SetActive(false);
            
            PoolManager.Instance.poolDictionary[GameManager.Instance.actualStraw].Enqueue(gameObject); // Remet le GameObject dans la file d'attente, a placer au moment ou elle est "détruite"
        }
        //-----------------------------------------------------------------------------------------
        
    }
}
