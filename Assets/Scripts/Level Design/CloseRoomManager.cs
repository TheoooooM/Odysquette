using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CloseRoomManager : MonoBehaviour {
    [SerializeField] private CloseRoomClass closeRoom = null;

    [Header("Buildings")] 
    [SerializeField] private List<LightCol> buildingColors = new List<LightCol>();

    [Header("Test")]
    public bool openTopTest = false;
    public bool openRightTest = false;
    public bool openBottomTest = false;
    public bool openLeftTest = false;
    
    
    /// <summary>
    /// Update the room based on the entry and the exit
    /// </summary>
    /// <param name="openTop"></param>
    /// <param name="openRight"></param>
    /// <param name="openBottom"></param>
    /// <param name="openLeft"></param>
    public void UpdateCloseRoom(bool openTop, bool openRight, bool openBottom, bool openLeft, bool changeLight = true, bool isLevelTwo = false) {
        if(closeRoom.CloseRoomTop != null) closeRoom.CloseRoomTop.SetActive(!openTop);
        foreach (GameObject gam in closeRoom.ObjectToDisableTop) { gam.SetActive(openTop); }
        
        if(closeRoom.CloseRoomRight != null) closeRoom.CloseRoomRight.SetActive(!openRight);
        foreach (GameObject gam in closeRoom.ObjectToDisableRight) { gam.SetActive(openRight); }

        if(closeRoom.CloseRoomBottom != null) closeRoom.CloseRoomBottom.SetActive(!openBottom);
        foreach (GameObject gam in closeRoom.ObjectToDisableBottom) { gam.SetActive(openBottom); }

        if(closeRoom.CloseRoomLeft != null) closeRoom.CloseRoomLeft.SetActive(!openLeft);
        foreach (GameObject gam in closeRoom.ObjectToDisableLeft) { gam.SetActive(openLeft); }

        if (changeLight) {
            if (isLevelTwo) ChangeRandomBuildingsLightLV2();
            else ChangeRandomBuildingsLightLV1();
        }
    }

    /// <summary>
    /// Change the color of some buidling to pink
    /// </summary>
    private void ChangeRandomBuildingsLightLV1() {
        int randomNumber = Random.Range(2, 4);
        if(buildingColors.Count == 0) return;
        
        for (int i = 0; i < randomNumber; i++) {
            int randomBuilding = Random.Range(0, buildingColors.Count);
            buildingColors[randomBuilding].ChangeLightPink();
        }
    }
    
    /// <summary>
    /// Change the color of some buidling to pink
    /// </summary>
    private void ChangeRandomBuildingsLightLV2() {
        foreach (LightCol building in buildingColors) {
            building.ChangeLightRed();
        }
    }
    
    /// <summary>
    /// Change the color of all building to blue
    /// </summary>
    public void MakeEveryBuildingBlue() {
        foreach (LightCol building in buildingColors) {
            building.ChangeLightBlue();
        }
    }
}

[System.Serializable]
public class CloseRoomClass {
    [SerializeField] private GameObject closeRoomTop;
    public GameObject CloseRoomTop => closeRoomTop;
    
    [SerializeField] private GameObject closeRoomRight;
    public GameObject CloseRoomRight => closeRoomRight;
    
    [SerializeField] private GameObject closeRoomBottom;
    public GameObject CloseRoomBottom => closeRoomBottom;
    
    [SerializeField] private GameObject closeRoomLeft;
    public GameObject CloseRoomLeft => closeRoomLeft;

    [SerializeField] private List<GameObject> objectToDisableTop = new List<GameObject>();
    public List<GameObject> ObjectToDisableTop => objectToDisableTop;
    
    [SerializeField] private List<GameObject> objectToDisableRight = new List<GameObject>();
    public List<GameObject> ObjectToDisableRight => objectToDisableRight;
    
    [SerializeField] private List<GameObject> objectToDisableBottom = new List<GameObject>();
    public List<GameObject> ObjectToDisableBottom => objectToDisableBottom;
    
    [SerializeField] private List<GameObject> objectToDisableLeft = new List<GameObject>();
    public List<GameObject> ObjectToDisableLeft => objectToDisableLeft;
}
