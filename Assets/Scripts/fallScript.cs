using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallScript : MonoBehaviour
{

    public enum Side
    {
        top,left,right,bot
    }
    
    [SerializeField] private GameObject playerShadow = null;
    public Playercontroller controller;
    private Rigidbody2D rb;
    private bool triggered;
    public bool footTrigger;
    public float goBackForce = 2;
    [HideInInspector] public Vector3 dashPos = Vector3.zero;
    [SerializeField] Side fallSide;
    [SerializeField] private fallScript[] otherFoot;

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
        if (other.CompareTag("Hole"))
        {
            bool fall = true;
            foreach (var foot in otherFoot)
            {
                if (foot.footTrigger == false)
                {
                    fall = false;
                    break;
                }
            }
            
            //Debug.Log(otherFoot.name + " is " + otherFoot.foottrigger);
            if (!controller.InDash && fall == true) {
            Debug.Log("fall by " + gameObject.name);
                controller.StartFall(fallSide);
                triggered = true;
            }
            else
            { 
            footTrigger = true;
            }
            
            if(playerShadow != null) playerShadow.SetActive(false);
        }
    }

    /// <summary>
    /// When the player stay over a hole
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay2D(Collider2D other) {

        bool fall = true;
        foreach (var foot in otherFoot)
        {
            if (foot.footTrigger == false)
            {
                fall = false;
                break;
            }
        }
        
        if (other.CompareTag("Hole") && !controller.InDash && !triggered && fall == true) {
            footTrigger = true;
            Debug.Log("fall by " + gameObject.name);
            controller.StartFall(fallSide,true,this);
            triggered = true;
        }
    }

    /// <summary>
    /// When the player exit a hole
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Hole"))
        {
            footTrigger = false;
            if (playerShadow != null) playerShadow.SetActive(true);
        }
    }

    IEnumerator WaitForfall()
    {
        yield return null;
        controller.StartFall(fallSide,true, this);
        triggered = true;
    }
}