using System;
using UnityEngine;

public class SpawnAnimation : MonoBehaviour {
    #region Variables
    [Header("Spawn Data")]
    [SerializeField] private GameObject playerPrefab = null;
    [SerializeField] private Animator machineAnimator = null;
    [SerializeField] private bool canSpawn = false;
    
    [Header("PreSpawn Data")]
    [SerializeField] private bool hasSpawn = false;
    #endregion Variables

    private void Awake() {
        playerPrefab.SetActive(false);
    }

    private void Update() {
        if (!hasSpawn && canSpawn) {
            hasSpawn = true;
            machineAnimator.SetTrigger("SpawnPlayer");
        }
    }


    /// <summary>
    /// Activate the menu which allow the player to press any key to start the game
    /// </summary>
    public void ChangePressMenuState() {
        canSpawn = true;
    }
}
