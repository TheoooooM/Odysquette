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
    public int _pierceCount;

    public bool canBounce = false;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _pierceCount = pierceCount;
        rb.AddForce(transform.right * (BulletSpeed* 1f), ForceMode2D.Impulse); // met une force sur la paille
    }

    private void OnEnable()
    {
        _pierceCount = pierceCount;
        canBounce = false;
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
            
            
            /*Vector2 dir = Vector2.Reflect(rb.velocity.normalized, Vector2.right);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            rb.velocity = Vector2.zero;
            gameObject.transform.rotation = quaternion.Euler(0f, 0f, angle);*/
            //rb.AddForce(dir);
        }
        else 
        {
            gameObject.SetActive(false); 
            PoolManager.Instance.poolDictionary[GameManager.Instance.actualStraw].Enqueue(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var speed = lastVelocity.magnitude;
        var direction = Vector3.Reflect(lastVelocity.normalized, other.contacts[0].normal);
        rb.velocity = direction * Mathf.Max(speed, 0f);
        var angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        Debug.Log("angle : " + angle);
        transform.rotation = Quaternion.Euler(0, 0, angle);
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
