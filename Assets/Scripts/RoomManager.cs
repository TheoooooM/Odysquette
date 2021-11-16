using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    
    public List<GameObject> ennemiesList = new List<GameObject>();
    public bool runningRoom;

    public GameObject exitGO;
    public GameObject enterGO;

    private bool done = false;
    private void Update()
    {
        
        if (runningRoom)
        {
            if (!done)
            {
                exitGO.SetActive(true);
                enterGO.SetActive(true);
                //exitClose.SetActive(true);
                done = true;
            }

            if (ennemiesList.Count == 0)
            {
                Debug.Log("finish Room");
                exitGO.SetActive(false);
                //exitClose.SetActive(false);
            }

        }
    }
}
