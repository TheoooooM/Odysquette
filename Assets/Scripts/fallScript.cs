using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallScript : MonoBehaviour
{
    public Playercontroller controller;
    private Rigidbody2D rb;
    
    public float goBackForce = 2;

    private Vector3 dashPos = Vector3.zero;
    
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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hole") && !controller.InDash)
        {
            Debug.Log("fall");
            Vector3 dir = rb.velocity;
            controller.transform.position -= dir.normalized*goBackForce;

        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Hole")  && !controller.InDash)
        {
            controller.transform.position = dashPos;
            Debug.Log("reset Dash");
        }
    }
}