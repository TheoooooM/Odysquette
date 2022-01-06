using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallScript : MonoBehaviour
{
    [SerializeField] private GameObject playerShadow = null;
    public Playercontroller controller;
    private Rigidbody2D rb;
    private bool triggered;
    public float goBackForce = 2;
    [HideInInspector] public Vector3 dashPos = Vector3.zero;
    
    private void Start()
    {
        rb = controller.GetComponent<Rigidbody2D>();
    }

    private void Update() {
        if (controller.InDash && dashPos == Vector3.zero) {
            dashPos = controller.transform.position;
        }
        else if (!controller.InDash && dashPos != null) {
            dashPos = Vector3.zero;
        }

        if (!controller.falling) triggered = false;
    }

    /// <summary>
    /// When the player enter over a hole
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Hole")) {
            if (!controller.InDash) {
                controller.StartFall();
                triggered = true;
            }
            
            if(playerShadow != null) playerShadow.SetActive(false);
        }
    }

    /// <summary>
    /// When the player stay over a hole
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Hole") && !controller.InDash && !triggered) {
            controller.StartFall(true,this);
            triggered = true;
        }
    }

    /// <summary>
    /// When the player exit a hole
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Hole")) {
            if (playerShadow != null) playerShadow.SetActive(true);
        }
    }

    private IEnumerator WaitForfall()
    {
        yield return null;
        controller.StartFall(true, this);
        triggered = true;
    }
}