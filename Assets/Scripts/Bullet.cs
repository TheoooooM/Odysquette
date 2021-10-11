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
    private Vector3 lastVelocity;

    [Header("==============Effects Stat===============")]
    public int pierceCount;
    private int _pierceCount;

    public int bounceCount;
    private int _bounceCount;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (GameManager.Instance.firstEffect == GameManager.Effect.pierce || GameManager.Instance.secondEffect == GameManager.Effect.pierce)_pierceCount = pierceCount;
        if (GameManager.Instance.firstEffect == GameManager.Effect.bounce || GameManager.Instance.secondEffect == GameManager.Effect.bounce) _bounceCount = bounceCount;
        rb.AddForce(transform.right * (BulletSpeed* 1f), ForceMode2D.Impulse); // met une force sur la paille
    }

    private void OnEnable()
    {
        if (GameManager.Instance.firstEffect == GameManager.Effect.pierce || GameManager.Instance.secondEffect == GameManager.Effect.pierce) _pierceCount = pierceCount;
        if (GameManager.Instance.firstEffect == GameManager.Effect.bounce || GameManager.Instance.secondEffect == GameManager.Effect.bounce) _bounceCount = bounceCount;
    }

    private void Update()
    {
        lastVelocity = rb.velocity;
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
        Debug.Log("boom" + other.name);
        switch (GameManager.Instance.firstEffect)
        {
            
            case GameManager.Effect.explosion :
                Explosion();
                break;
            
            
            case GameManager.Effect.ice :
                Ice(other.gameObject);
                break;
        }
        
        switch (GameManager.Instance.secondEffect)
        {
            case GameManager.Effect.explosion :
                Explosion();
                break;

            case GameManager.Effect.ice :
                Ice(other.gameObject);
                break;
        }

        if (_pierceCount > 0 && other.CompareTag("Enemy"))
        {
            _pierceCount--;
        }
        else if (!other.CompareTag("Walls"))
        {
            gameObject.SetActive(false); 
            PoolManager.Instance.poolDictionary[GameManager.Instance.actualStraw].Enqueue(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (_bounceCount > 0)
        {
            var speed = lastVelocity.magnitude;
            var direction = Vector3.Reflect(lastVelocity.normalized, other.contacts[0].normal);
            rb.velocity = direction * Mathf.Max(speed, 0f);
            var angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            transform.rotation = Quaternion.Euler(0, 0, angle);

            _bounceCount--;
        }
        else
        {
            gameObject.SetActive(false); 
            PoolManager.Instance.poolDictionary[GameManager.Instance.actualStraw].Enqueue(gameObject);
        }
    }

    void Explosion()
    {
        Debug.Log("explosion");
    }

    void Ice(GameObject gam)
    {
        Debug.Log("ice");
        
        
    }
    
}
