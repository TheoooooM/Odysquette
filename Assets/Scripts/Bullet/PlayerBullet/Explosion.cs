using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private Animation anim;
    private Rigidbody2D rb;
    [SerializeField] private float time = 2f;
    private float timer;
    [SerializeField]
  float  knockUpValue;
    [SerializeField]
    private float damage;

    private float timerCollider;
    [SerializeField] private float timeCollider;
    [SerializeField]
    private  Collider2D collider2D;
    private void Start()
    {
    
       anim = GetComponent<Animation>();
       rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        AudioManager.Instance.PlayImpactStraw(AudioManager.StrawSoundEnum.Explosion, transform.position);
        collider2D.enabled = true;
        timerCollider = 0;
        timer = 0; 
        /*  if(anim !=null)anim.Play("explosion");
        else
        {
            anim = GetComponent<Animation>();
            anim.Play("explosion");
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        timerCollider += Time.deltaTime;
        if (timerCollider > timeCollider)
        {
            collider2D.enabled = false;
        }
        timer += Time.deltaTime;
        if (timer > time)
        {
            gameObject.SetActive(false);
            PoolManager.Instance.explosionQueue.Enqueue(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Enemy")) {
            other.GetComponent<EnemyStateManager>().TakeDamage(damage, rb.position, knockUpValue, true, true);
        }
        else if (other.CompareTag("Walls") && other.GetComponent<DestructableObejct>()) {
            other.GetComponent<DestructableObejct>().TakeDamage(damage);
        }
    }
}
