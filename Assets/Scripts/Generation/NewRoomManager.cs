using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class NewRoomManager : MonoBehaviour
{
    #region Singletone
    
    public static NewRoomManager instance;
    private void Awake()
    {
        instance = this;
    }
    
    #endregion
    
    public enum open
    {
        none, left, right, top, bot
    }
    
    
    [SerializeField] private GameObject[] roomArray;
    [SerializeField] private GameObject[] firstRoom;
    [SerializeField] private GameObject[] multiExitRoom;

    private open needOpen = open.none;
    private Transform spawnPoint;


    private void Start()
    {
        GenerateSegment(4);
    }

    void GenerateSegment(int roomAmount)
    {
        
        for (int i = 1; i < roomAmount; i++)
        {
            if (i == 1)
            {
                int num = Random.Range(0, firstRoom.Length);
                Instantiate(firstRoom[num]);
                switch (firstRoom[num].GetComponent<RoomClass>().ExitArray[0])
                {
                    case open.bot : spawnPoint = firstRoom[num].GetComponent<RoomClass>().GenBot.transform; break;
                    case open.top : spawnPoint = firstRoom[num].GetComponent<RoomClass>().GenTop.transform; break;
                    case open.left : spawnPoint = firstRoom[num].GetComponent<RoomClass>().GenLeft.transform; break;
                    case open.right : spawnPoint = firstRoom[num].GetComponent<RoomClass>().GenRight.transform; break; 
                } 
                

            }
            else if (i == roomAmount)
            {
                GameObject generateRoom = AvailableRoom(true);
                if (generateRoom != null)
                {
                    Vector2 posSpawn = spawnPoint.position - GetGen(generateRoom.GetComponent<RoomClass>()).transform.position;
                    Instantiate(generateRoom, posSpawn, quaternion.identity);
                }
                else i--;
            }
            else
            {
                GameObject generateRoom = AvailableRoom();
                if (generateRoom != null)
                {
                    Vector2 posSpawn = spawnPoint.position - GetGen(generateRoom.GetComponent<RoomClass>()).transform.position;
                    Instantiate(generateRoom, posSpawn, quaternion.identity);
                    
                }
                else i--;
                
                
            }
        }
    }

    GameObject AvailableRoom(bool multiExit = false)
    {
        RoomClass room;
        int counter = 0;
        int num = Random.Range(0, firstRoom.Length);
        if (multiExit) room = multiExitRoom[num].GetComponent<RoomClass>();
        else room = roomArray[num].GetComponent<RoomClass>();
        

        if (room.ExitArray.Contains(needOpen))
        {
            Instantiate(room.gameObject, spawnPoint);
        }
        else
        {
            room = null;
        }
        
        
        return room.gameObject;

        /*while (room.ExitArray.Contains(needOpen))
        {
            num = Random.Range(0, firstRoom.Length);
            room = multiExitRoom[num].GetComponent<RoomClass>();
            counter++;
            if (counter > 500)
            {
                Debug.Log("break while");
                break; 
            }
        }*/
    }

    Transform GetGen(RoomClass room)
    {
        GameObject gen = null;
            switch (needOpen)
            {
                case open.none : break;
                    
                case open.bot : gen = room.GenBot; break;
                case open.top : gen = room.GenTop; break;
                case open.left : gen = room.GenLeft; break;
                case open.right : gen = room.GenRight; break;
            }

        return gen.transform;
    }
}
