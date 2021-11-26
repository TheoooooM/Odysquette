using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CircleCollider2D))]
public class PlayerDetectionEvent : MonoBehaviour {
    #region Variables
    [SerializeField] private UnityEvent enterTriggerEvent =  null;
    [SerializeField] private UnityEvent exitTriggerEvent =  null;
    [Space(4)] [SerializeField] private float radiusDetection = 2f;

    /// <summary>
    /// Update the radius size when changes are made
    /// </summary>
    private void OnValidate() {
        GetComponent<CircleCollider2D>().radius = radiusDetection;
    }
    #endregion Variables

    #region Player Detection
    /// <summary>
    /// When the player enter in collision
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) enterTriggerEvent.Invoke();

    }
    
    /// <summary>
    /// When the player exit the collision
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) exitTriggerEvent.Invoke();
        }
    #endregion Player Detection
}
