using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubmachineOpen : MonoBehaviour {
    [SerializeField] private PlayerMovementHub PlayerMovementHub = null;
    [SerializeField] private GameObject playerPrefab = null;
    
    /// <summary>
    /// Enable the player
    /// </summary>
    public void ActivPlayer() => playerPrefab.SetActive(true);
    
    /// <summary>
    /// Start The Player Anim
    /// </summary>
    public void StartPlayerAnim() => PlayerMovementHub.StartMoveAnim();
}
