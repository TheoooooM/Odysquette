using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CircleCollider2D))]
public class PlayerDetectionEvent : MonoBehaviour {
    #region Variables
    [SerializeField] private UnityEvent enterTriggerEvent =  null;
    [SerializeField] private UnityEvent interactTriggerEvent =  null;
    [SerializeField] private UnityEvent exitTriggerEvent =  null;
    [Space(4)] [SerializeField] private float radiusDetection = 2f;
    [Space(4)] [SerializeField] private bool needToPressE = false;
    private PlayerMapping playerInput;

    /// <summary>
    /// Update the radius size when changes are made
    /// </summary>
    private void OnValidate() {
        GetComponent<CircleCollider2D>().radius = radiusDetection;
        GetComponent<CircleCollider2D>().isTrigger = true;
    }
    #endregion Variables

    private void Start() {
        playerInput = new PlayerMapping();
        playerInput.Interface.InteractBtn.performed += PressE;
    }


    #region Player Detection
    /// <summary>
    /// When the player enter in collision
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            enterTriggerEvent.Invoke();
            if(needToPressE) playerInput.Interface.Enable();
        }
    }

    /// <summary>
    /// When the player need to press to make an interaction
    /// </summary>
    /// <param name="obj"></param>
    private void PressE(InputAction.CallbackContext obj) {
        interactTriggerEvent.Invoke();
        if (needToPressE) playerInput.Interface.Disable();
    }
    
    /// <summary>
    /// When the player exit the collision
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            exitTriggerEvent.Invoke();
            if (needToPressE) playerInput.Interface.Disable();
        }
    }

    #endregion Player Detection
}
