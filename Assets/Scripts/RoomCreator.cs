using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Salle Dim 18.4 x 10.4

[Serializable]
public class Room
{
    public RoomContainer RoomGO;
    public Vector2 ArrayPosition;


    public bool exitTop = true;
    public bool exitLeft = true;
    public bool exitRight = true;
    public bool exitBot = true;
    
}


[ExecuteInEditMode]
public class RoomCreator : MonoBehaviour
{
    private bool Exist = false;
    public Room[] partList;
    public GameObject[,] partMap = new GameObject[5,5];

    public GameObject roomPref;

    public bool update = false;
    
    //public Vector2 arrayPos;
    //public bool debugMapPos;

    

    private void Start()
    {
        if (!Exist)
        {
            transform.position = Vector3.zero;
            partList = new Room[2];
            partList[0] = new Room();
            roomPref = Resources.Load<GameObject>("Room/StartRoom");
            partList[0].RoomGO = Instantiate(roomPref, transform).GetComponent<RoomContainer>();
            partList[0].ArrayPosition = new Vector2(2,2);
            partMap[2,2] = partList[0].RoomGO.gameObject;
            Exist = true;
        }
    }

    private void Update()
    {
        if (update)
        {
            foreach (Room rom in partList)
            {

                if (rom.RoomGO == null)
                {
                    rom.RoomGO = Instantiate(roomPref, new Vector2((rom.ArrayPosition.x-2)*18.4f , (rom.ArrayPosition.y-2)*10.4f), Quaternion.identity, transform).GetComponent<RoomContainer>();
                    partMap[(int) rom.ArrayPosition.x, (int) rom.ArrayPosition.y] = rom.RoomGO.gameObject;
                    
                }
            }

            foreach (Room rom in partList)
            {
                Debug.Log(rom.ArrayPosition);
                Debug.Log(rom.ArrayPosition.x - 1);
                if ((rom.ArrayPosition.x - 1) >= 0)
                {
                    Debug.Log(rom.RoomGO.partLeft);
                    if (partMap[(int) rom.ArrayPosition.x - 1, (int) rom.ArrayPosition.y] != null) rom.RoomGO.partLeft = true;
                    else rom.RoomGO.partLeft = false;
                }
                else rom.RoomGO.partLeft = false;

                if (rom.ArrayPosition.x + 1 < partMap.GetLength(0))
                {
                    if (partMap[(int) rom.ArrayPosition.x + 1, (int) rom.ArrayPosition.y] != null) rom.RoomGO.partRight = true;
                    else rom.RoomGO.partRight = false;
                }
                else rom.RoomGO.partRight = false;

                if ((rom.ArrayPosition.y - 1) >= 0)
                {
                    if (partMap[(int) rom.ArrayPosition.x, (int) rom.ArrayPosition.y - 1] != null) rom.RoomGO.partBot = true;
                    else rom.RoomGO.partBot = false;
                }
                else rom.RoomGO.partBot = false;


                if (rom.ArrayPosition.y + 1 < partMap.GetLength(1))
                {
                    if (partMap[(int) rom.ArrayPosition.x, (int) rom.ArrayPosition.y + 1] != null) rom.RoomGO.partTop = true;
                    else rom.RoomGO.partTop = false;
                    Debug.Log(rom.RoomGO.partTop);
                }
                else rom.RoomGO.partTop = false;

                rom.RoomGO.exitTop = rom.exitTop;
                rom.RoomGO.exitLeft = rom.exitLeft;
                rom.RoomGO.exitRight = rom.exitRight;
                rom.RoomGO.exitBot = rom.exitBot;

                rom.RoomGO.UpdatePart();
            }

            Debug.Log("Update");
            update = false;
        }

        
    }
}
