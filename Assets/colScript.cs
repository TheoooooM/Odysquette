using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colScript : MonoBehaviour
{
    public Generation.open side;
    public RoomContainer part;


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

    private void OnTriggerExit2D(Collider2D other)
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
