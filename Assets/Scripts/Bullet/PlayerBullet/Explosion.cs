using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private Animation anim;

    private void Start()
    {
       anim = GetComponent<Animation>();
    }

    void OnEnable()
    {
        if(anim !=null)anim.Play("explosion");
        else
        {
            anim = GetComponent<Animation>();
            anim.Play("explosion");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!anim.isPlaying)
        {
            gameObject.SetActive(false);
            PoolManager.Instance.explosionQueue.Enqueue(gameObject);
        }
    }
}
