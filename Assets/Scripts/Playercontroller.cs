using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercontroller : MonoBehaviour
{
    [SerializeField] float MouvementSpeed = 0.01f;
    public GameObject gun;
    private Rigidbody2D rb;
    private int a = 3;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

       
    }


    // Update is called once per frame
    void FixedUpdate()
    {
       
        if (Input.GetKey(KeyCode.D))
        {
            rb.position += new Vector2(MouvementSpeed*0.2f, 0);
         
        }
        if (Input.GetKey(KeyCode.Q))
        {
            rb.position += new Vector2(-MouvementSpeed*0.2f, 0);
          
        }
        if (Input.GetKey(KeyCode.Z))
        {
            rb.position += new Vector2(0, MouvementSpeed*0.2f);
          
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.position += new Vector2(0, -MouvementSpeed*0.2f);
           
        }

    }


}
