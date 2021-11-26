using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class SpawnAnimation : MonoBehaviour {
    #region Variables
    [SerializeField] private GameObject startCanvas = null;
    [SerializeField] private bool hasSpawn = false;
    #endregion Variables

    private void Update() {
        if ((Input.anyKeyDown) && !hasSpawn) {
            hasSpawn = true;
            startCanvas.SetActive(false);
            //Spawn and launch all the animation and particles needed for the spawn of the player
            //Enable the inputs for the player
        }
    }
}
