using System;
using UnityEngine;

public class SpawnAnimation : MonoBehaviour {
    #region Variables
    [Header("Spawn Data")]
    [SerializeField] private GameObject playerPrefab = null;
    [SerializeField] private Transform playerPosTransf = null;
    [SerializeField] private Transform playerGoTo = null;
    [SerializeField] private bool canSpawn = false;
    
    [Header("PreSpawn Data")]
    [SerializeField] private Animator startCanvasAnim = null;
    [SerializeField] private bool hasSpawn = false;
    #endregion Variables

    private void Awake() {
        playerPrefab.SetActive(false);
    }

    private void Update() {
        if ((Input.anyKeyDown) && !hasSpawn && canSpawn) {
            hasSpawn = true;
            startCanvasAnim.SetTrigger("CloseAnim");
            playerPrefab.SetActive(true);
            playerPrefab.GetComponent<PlayerMovementHub>().StartMoveAnim();
        }
    }


    /// <summary>
    /// Activate the menu which allow the player to press any key to start the game
    /// </summary>
    public void ChangePressMenuState() {
        canSpawn = true;
        startCanvasAnim.SetTrigger("PressAnim");
    }
}
