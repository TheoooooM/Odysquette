using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    
   [SerializeField] float BulletSpeed;
   private BulletStat Scriptable;
    int Timer = 0;
    private Rigidbody2D rb;

    [Header("==============Effects Stat===============")]
    public int pierceCount;
    public int _pierceCount;

    public bool canBounce = false;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _pierceCount = pierceCount;
    }

    private void OnEnable()
    {
        _pierceCount = pierceCount;
        canBounce = false;
    }

    void Update()
    {
        rb.AddForce(transform.right * (BulletSpeed* 0.0001f), ForceMode2D.Impulse); // met une force sur la paille
        
        
        
    }

    private void FixedUpdate()
    {
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("boom" + _pierceCount);
        switch (GameManager.Instance.firstEffect)
        {
            case GameManager.Effect.bounce :
                Bounce();
                break;
            
            case GameManager.Effect.explosion :
                Explosion();
                break;
            
            
            case GameManager.Effect.ice :
                Ice();
                break;
        }
        
        switch (GameManager.Instance.secondEffect)
        {
            case GameManager.Effect.bounce :
                Bounce();
                break;

            case GameManager.Effect.explosion :
                Explosion();
                break;

            case GameManager.Effect.ice :
                Ice();
                break;
        }

        if (_pierceCount > 0 && other.CompareTag("Enemy"))
        {
            _pierceCount--;
        }
        else if (canBounce == true)// && other.CompareTag("Walls"))
        {
            
            
            Vector2 dir = Vector2.Reflect(rb.velocity.normalized, Vector2.right);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            rb.velocity = Vector2.zero;
            gameObject.transform.rotation = quaternion.Euler(0f, 0f, angle);
            //rb.AddForce(dir);
        }
        else 
        {
            gameObject.SetActive(false); 
            PoolManager.Instance.poolDictionary[GameManager.Instance.actualStraw].Enqueue(gameObject);
        }
    }

    void Bounce()
    {
        canBounce = true;
        Debug.Log("bounce");
    }

    void Explosion()
    {
        Debug.Log("explosion");
    }

    void Ice()
    {
        Debug.Log("ice");
    }
    
}
