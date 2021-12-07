using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ressource : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float throwForce = 2;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        float x = Random.Range(-throwForce, throwForce)*100;
        float y = Random.Range(-throwForce, throwForce)*100;
        Debug.Log("trhow force " + x + " " + y);
        rb.AddForce(new Vector2(x, y));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            NeverDestroy.Instance.AddRessource();
            Destroy(gameObject);
        }
    }
}
