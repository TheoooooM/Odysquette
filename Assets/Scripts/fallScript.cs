using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallScript : MonoBehaviour
{
    public Playercontroller controller;
    private Rigidbody2D rb;

    private bool triggered;
    
    public float goBackForce = 2;

    [HideInInspector] public Vector3 dashPos = Vector3.zero;
    
    private void Start()
    {
        rb = controller.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (controller.InDash && dashPos == Vector3.zero)
        {
            dashPos = controller.transform.position;
        }
        else if (!controller.InDash && dashPos != null)
        {
            dashPos = Vector3.zero;
        }

        if (!controller.falling)
        {
            triggered = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hole") && !controller.InDash)
        {
            
            controller.StartFall();
            triggered = true;
            /*Debug.Log("fall");
            Vector3 dir = rb.velocity;
            controller.transform.position -= dir.normalized*goBackForce;*/

        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Hole")  && !controller.InDash && !triggered)
        {
           controller.StartFall(true, this);
           triggered = true;
        }
    }
}