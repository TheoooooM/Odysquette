using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class colScript : MonoBehaviour
{
    public Generation.open side;
    public RoomContainer part;

    private void Start()
    {
        part = GetComponentInParent<RoomContainer>();
    }

    private void Update()
    {
        if (!part.room.runningRoom)
        {
            GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (part.room.cameraRect == CameraControllers.Instance.currentRectLimitation && part.room.roomFinish)
        {
            switch (side)
            {
                case Generation.open.top : CameraControllers.Instance.currentRectLimitation.y += 3; break;
                case Generation.open.left : CameraControllers.Instance.currentRectLimitation.x -= 4.5f; break;
                case Generation.open.right : CameraControllers.Instance.currentRectLimitation.x += 4.5f; break;
                case Generation.open.bot : CameraControllers.Instance.currentRectLimitation.y -= 3; break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!part.playerInRoom)
        {
            Debug.Log("yo");
            switch (side)
            {
                case Generation.open.top :Generation.Instance.map[(int)part.roomMapPos.x, (int)part.roomMapPos.y + 1].GetComponent<RoomContainer>().room.runningRoom = true; break;
                case Generation.open.left :Generation.Instance.map[(int)part.roomMapPos.x-1, (int)part.roomMapPos.y].GetComponent<RoomContainer>().room.runningRoom = true; break;
                case Generation.open.right :Generation.Instance.map[(int)part.roomMapPos.x+1, (int)part.roomMapPos.y].GetComponent<RoomContainer>().room.runningRoom = true; break;
                case Generation.open.bot :Generation.Instance.map[(int)part.roomMapPos.x, (int)part.roomMapPos.y - 1].GetComponent<RoomContainer>().room.runningRoom = true; break;
            }
            part.room.runningRoom = false;
        }
    }
}
