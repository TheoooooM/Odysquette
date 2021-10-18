using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool hasRange;
    public float damage;
    public float range;
    public Color colorBang;
     Vector3 basePosition;
     public float speed;

     public Vector3 oldPositionPoison;
   private BulletStat Scriptable;

    public Rigidbody2D rb;
    private Vector3 lastVelocity;

    [Header("==============Effects Stat===============")]
    public int pierceCount;
    private int _pierceCount;

    public int bounceCount;
    private int _bounceCount;

    public float poisonCooldown = 5;
    float _poisonCooldown = 0;
    public float BulletSpeed;
    public bool isEnable;
    public bool isDesactive = false;

    public float distance;
    private void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        if (GameManager.Instance.firstEffect == GameManager.Effect.pierce || GameManager.Instance.secondEffect == GameManager.Effect.pierce)_pierceCount = pierceCount;
        else _pierceCount = 0;
        
        if (GameManager.Instance.firstEffect == GameManager.Effect.bounce || GameManager.Instance.secondEffect == GameManager.Effect.bounce) _bounceCount = bounceCount;
        else _bounceCount = 0;
        
       // rb.velocity = Vector2.zero;
        //rb.AddForce((GameManager.Instance._lookDir).normalized * BulletSpeed, ForceMode2D.Impulse); // met une force sur la paille

    }

    private void OnEnable()
    {
        isDesactive = false;
        isEnable = false;
         basePosition = transform.position;
        _pierceCount = pierceCount;
        //canBounce = false; 
        Invoke(nameof(DelayforDrag),0.5f);
               if (GameManager.Instance.firstEffect == GameManager.Effect.pierce || GameManager.Instance.secondEffect == GameManager.Effect.pierce) _pierceCount = pierceCount;
                else _pierceCount = 0;
                
                if (GameManager.Instance.firstEffect == GameManager.Effect.bounce || GameManager.Instance.secondEffect == GameManager.Effect.bounce) _bounceCount = bounceCount;
                else _bounceCount = 0;
                
    }

    void Update()
    {
         
           if (hasRange)
           {
                 if (Vector3.Distance(basePosition, transform.position) >=range)
                     {
                         DesactiveBullet();
                         
                     }
           }
    
   
      else if (rb.velocity.magnitude <= 0.1 && rb.drag > 0 && isEnable)
      {
          DesactiveBullet();
         
      } 
       
       // transform.rotation = Quaternion.Euler(0f, 0f, GameManager.Instance.angle);
        
        lastVelocity = rb.velocity;

        

    }


    private void FixedUpdate()
    {
        if (GameManager.Instance.firstEffect == GameManager.Effect.poison || GameManager.Instance.secondEffect == GameManager.Effect.poison)
                {
                    
      //  Debug.Log(distance/rb.velocity.magnitude +"   " +  _poisonCooldown + "   " + Time.deltaTime + "  = " + (_poisonCooldown + Time.deltaTime));
        Debug.Log(rb.velocity.magnitude);
        
                    if (_poisonCooldown < distance / rb.velocity.magnitude) 
                    {
                        _poisonCooldown += Time.fixedDeltaTime;
                    }
                    else
                    {
                        //Debug.Log(_poisonCooldown);
                        PoolManager.Instance.SpawnPoisonPool(transform, speed);
                        _poisonCooldown = poisonCooldown;
                    }
                }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      //  Debug.Log("collid" + other.name);
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
            Debug.Log("les degats olalala : " + damage);
        }

        else if (!other.CompareTag("Walls"))

        { 
            DesactiveBullet();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        
        if (_bounceCount > 0)
        {Debug.Log(_bounceCount); 
            _bounceCount--;
            var speed = lastVelocity.magnitude;
            var direction = Vector3.Reflect(lastVelocity.normalized, other.contacts[0].normal);
            rb.velocity = direction * Mathf.Max(speed, 0f);
            var angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            transform.rotation = Quaternion.Euler(0, 0, angle);

          
        }
        else
        {
           
            DesactiveBullet();
        }
    }

    void Explosion()
    {
        Debug.Log("explosion");
        PoolManager.Instance.SpawnExplosionPool(transform);
    }

    void Ice(GameObject gam)
    {
        Debug.Log("ice");
        gam.GetComponent<enemy>().freezeTime = 5;

    }

    void DelayforDrag()
    {
        isEnable = true;
    }

    void DesactiveBullet()
    {
        if (isDesactive == false)
        {
            gameObject.SetActive(false); 
                  //Debug.Log("enqufds" + name);
                    PoolManager.Instance.poolDictionary[GameManager.Instance.actualStraw].Enqueue(gameObject);
                    isDesactive = true;
        }
        
    }
}
