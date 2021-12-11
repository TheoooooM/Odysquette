using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubCamera : MonoBehaviour{
    #region Variables
    [SerializeField] private SpawnAnimation spawnData = null;
    #endregion Variables

    /// <summary>
    /// When the camera animation stop. The player can press any button to start
    /// </summary>
    public void CanPressButton() {
        spawnData.ChangePressMenuState();
        GetComponent<Animator>().enabled = false;
    }
}
