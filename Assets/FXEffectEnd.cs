using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXEffectEnd : MonoBehaviour
{
    private float timer;
    [SerializeField] private float time;
   [SerializeField] private bool pierce;
   private Animator animator;
   private void Start()
   { 
       if(!pierce)
        animator = GetComponent<Animator>();
       
   }

   void OnEnable()
    {
        timer = 0;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (time <= timer)
        {
            if (pierce)
            {
                
                PoolManager.Instance.pierceQueue.Enqueue(gameObject);
                gameObject.SetActive(false);
            }
            else
            {
              
                PoolManager.Instance.impactQueue.Enqueue(gameObject);
                gameObject.SetActive(false);
            }

         
        } 
        if (!pierce)
        {
           
            animator.Play("Impact");             
        }
    }

    void PierceBullet()
    {
        
    }
    void ImpactBullet()
    {
        
    }
}
