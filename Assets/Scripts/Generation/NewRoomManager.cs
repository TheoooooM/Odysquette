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

    public bool reset = false;
    
    [SerializeField] private GameObject[] roomArray;
    [SerializeField] private GameObject[] firstRoom;
    [SerializeField] private GameObject[] multiExitRoom;

    private List<GameObject> createRoomList = new List<GameObject>();

    private open needOpen = open.none;
    [SerializeField] private Transform spawnPoint;
    private Transform newSpawnPoint;


    private void Start()
    {
        GenerateSegment(10);
    }

    

  

    void GenerateSegment(int roomAmount)
    {
        RoomClass generateRoom = null;
        int antiwhile = 0;
        for (int i = 1; i <= roomAmount; i++)
        {
            Debug.Log("start createRoom n°" + i);
            
            if (i > 1)
            {
                
                if (!reset && generateRoom != null)
                {
                    spawnPoint = newSpawnPoint;
                    createRoomList.Add(generateRoom.gameObject);
                }
                else
                {
                    i--;
                    reset = false;
                Debug.Log("i - 1 = " + i);
                }
            
            }
            
            if (i == 1)
            {
                Debug.Log("firstRoom");
                int num = Random.Range(0, firstRoom.Length);
                generateRoom =  Instantiate(firstRoom[num]).GetComponent<RoomClass>();
                switch (generateRoom.GetComponent<RoomClass>().ExitArray[0])
                {
                    case open.bot : newSpawnPoint = generateRoom.GetComponent<RoomClass>().GenBot.transform; needOpen = open.top; break;
                    case open.top : newSpawnPoint = generateRoom.GetComponent<RoomClass>().GenTop.transform; needOpen = open.bot; break;
                    case open.left : newSpawnPoint = generateRoom.GetComponent<RoomClass>().GenLeft.transform; needOpen = open.right; break;
                    case open.right : newSpawnPoint = generateRoom.GetComponent<RoomClass>().GenRight.transform; needOpen = open.left; break; 
                } 
                
                

            }
            else if (i == roomAmount)
            {
                Debug.Log("LastRoom");
                generateRoom = AvailableRoom(true);
                if (generateRoom != null)
                {
                    newSpawnPoint = GetGen(generateRoom.GetComponent<RoomClass>()).transform;
                    Vector2 posSpawn = spawnPoint.position - newSpawnPoint.position;
                    Debug.Log("generate " + generateRoom.name + " from " + spawnPoint.name + " to " + newSpawnPoint);
                    generateRoom = Instantiate(generateRoom, posSpawn, quaternion.identity);
                    newSpawnPoint = GetGen(generateRoom.GetComponent<RoomClass>()).transform;
                    
                }
                else reset = true;
            }
            else
            {
                Debug.Log("NormalRoom");
                generateRoom = AvailableRoom();
                if (generateRoom != null)
                {
                    newSpawnPoint = GetGen(generateRoom.GetComponent<RoomClass>());
                    Vector2 posSpawn = spawnPoint.position - newSpawnPoint.position;
                    Debug.Log("generate " + generateRoom.name + " from " + spawnPoint.name + " to " + newSpawnPoint);
                    generateRoom = Instantiate(generateRoom, posSpawn, quaternion.identity);


                    foreach (open op in generateRoom.GetComponent<RoomClass>().ExitArray)
                    {
                        if (op != needOpen)
                        {
                            needOpen = op;
                            newSpawnPoint = GetGen(generateRoom.GetComponent<RoomClass>());
                            break;
                        }
                    }

                    switch (needOpen)
                    {
                        case open.bot : needOpen = open.top; break;
                        case open.top : needOpen = open.bot; break;
                        case open.right : needOpen = open.left; break;
                        case open.left : needOpen = open.right; break;
                    }
                }
                else reset=true;
                
                
            }

            antiwhile++;
            if (antiwhile > 100)
            {
                Debug.Log("antiwhile break at" + i);
                break;
                
            }
        }
    }

   

    RoomClass AvailableRoom(bool multiExit = false)
    {
        RoomClass room;
        int counter = 0;
        if (multiExit)
        {
            int num = Random.Range(0, multiExitRoom.Length);
            room = multiExitRoom[num].GetComponent<RoomClass>();
            
        }
        else
        {
            int num = Random.Range(0, roomArray.Length);
            room = roomArray[num].GetComponent<RoomClass>();
        }
        Debug.Log(room.name + needOpen);
        

        if (!room.ExitArray.Contains(needOpen))
        {
            room = null;
            Debug.Log("Dont' contain open");
        }
        
        
        return room;

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
        Transform gen = null;
            switch (needOpen)
            {
                case open.none : break;
                    
                case open.bot : gen = room.GenBot.transform; break;
                case open.top : gen = room.GenTop.transform; break;
                case open.left : gen = room.GenLeft.transform; break;
                case open.right : gen = room.GenRight.transform; break;
            }
        
        return gen;
    }
}
