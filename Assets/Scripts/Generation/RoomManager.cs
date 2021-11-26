using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{

    public Rect cameraRect;
    
    public List<GameObject> ennemiesList = new List<GameObject>();
    public bool runningRoom;
    [HideInInspector] public bool roomFinish = false;

    public GameObject exitGO;
    public GameObject enterGO;

    private bool done = false;
    private void Update()
    {
        
        if (runningRoom)
        {
            if (!done)
            {
                CameraControllers.Instance.currentRectLimitation = cameraRect;
                if(exitGO != null)exitGO.SetActive(true);
                if (enterGO != null) enterGO.SetActive(true);
                //exitClose.SetActive(true);
                done = true;
            }

            if (ennemiesList.Count == 0)
            {
                //Debug.Log("finish Room");
                roomFinish = true;
                if(exitGO != null)exitGO.SetActive(false);
                //exitClose.SetActive(false);
            }

        }
    }
}