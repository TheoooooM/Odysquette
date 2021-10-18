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
    
  
    
   private BulletStat Scriptable;
    int Timer = 0;
    public Rigidbody2D rb;
     
    [Header("==============Effects Stat===============")]
    public int pierceCount;
    public int _pierceCount;
    private bool isEnable;
    public bool canBounce = false;
    

    private void Start()
    {
     
        _pierceCount = pierceCount;
       
       
    }

    private void OnEnable()
    {
        isEnable = false;
         basePosition = transform.position;
        _pierceCount = pierceCount;
        canBounce = false; 
        Invoke(nameof(DelayforDrag),0.5f);
        
    }

    void Update()
    {
           Debug.Log(GetComponent<Rigidbody2D>().velocity);
           if (hasRange)
           {
                 if (Vector3.Distance(basePosition, transform.position) >=range)
                     {
                         
                         PoolManager.Instance.poolDictionary[GameManager.Instance.actualStraw].Enqueue(gameObject);
                      
                         gameObject.SetActive(false);
                         
                     }
           }
    
   
      else if (rb.velocity.magnitude <= 0.1 && rb.drag > 0 && isEnable)
      {
          
          PoolManager.Instance.poolDictionary[GameManager.Instance.actualStraw].Enqueue(gameObject);
      
          gameObject.SetActive(false);
       
      } 
 




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
            Debug.Log("les degats olalala : " + damage);
        }
        else if (canBounce == true)// && other.CompareTag("Walls"))
        {
            
            
   
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

    void DelayforDrag()
    {
        isEnable = true;
    }
    
}
