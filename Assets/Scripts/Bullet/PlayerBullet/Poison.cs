using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : MonoBehaviour
{
    public float Timer;
    private float _Timer;
    public float speed;
    public Transform bulletTransform;
    public Vector3 hit;
    private bool setNormal;
    [SerializeField]
    private float damage = 1 ;
   

 

    private void OnEnable()
    {
        setNormal = false;
        _Timer = Timer;
    }

    private void Update()
    {
        if(!setNormal)
        {
            if (hit != Vector3.zero)
            {
                setNormal = true;
                Debug.Log("set normal");
                transform.position += (hit.normalized * 1.3f);
            }
        }
    }

    void FixedUpdate()
    {
        if (_Timer>0)
        {
            _Timer -= 1;
        }
        else
        {
            gameObject.SetActive(false);
            PoolManager.Instance.PoisonQueue.Enqueue(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
             other.GetComponent<EnemyStateManager>().TakeDamage(damage, transform.position, 0, false, false);
                    gameObject.SetActive(false);
                    PoolManager.Instance.PoisonQueue.Enqueue(gameObject);
        }
       
    }
}
