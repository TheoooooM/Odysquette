using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementHub : MonoBehaviour {
    [SerializeField] private Playercontroller player = null;
    [SerializeField] private GameObject cursor = null;
    [SerializeField] private Collider2D collider2D = null;
    
    [SerializeField] private Transform startPos = null; 
    [SerializeField] private Transform endPos = null;
    [SerializeField] private float animDuration = 1;
    private bool playAnim = false;
    private bool hasPlayedAnim = false;
    
    private void Start() {
        cursor.SetActive(false);
        transform.position = startPos.position;
        GetComponent<Animator>().Play("Move_Front");
        collider2D.enabled = false;
    }

    private void Update() {
        if (playAnim && !hasPlayedAnim) {
            transform.position = Vector3.Lerp(transform.position, endPos.position, 1 / (animDuration * 10));
            if (Vector3.Distance(transform.position, endPos.position) <= 0.1f) {
                hasPlayedAnim = true;
                player.StartInput();
                collider2D.enabled = true;
                cursor.SetActive(true);
            }
        }
    }

    public void StartMoveAnim() => playAnim = true;
}
