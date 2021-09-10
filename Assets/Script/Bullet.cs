using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
   [SerializeField] float BulletSpeed;
    int Timer = 0;

    void Update()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.right * BulletSpeed, ForceMode2D.Impulse);
        
        Timer= Timer + 1 ;
        if (Timer == 5000) Destroy(gameObject);
    }
}
