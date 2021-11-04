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

    public bool hasSpawn = true;
    
    [SerializeField] private GameObject[] roomArray;
    [SerializeField] private GameObject[] firstRoom;
    [SerializeField] private GameObject[] multiExitRoom;

    private List<GameObject> createRoomList = new List<GameObject>();

    private open needOpen = open.none;
    private Transform spawnPoint;
    private Transform newSpawnPoint;


    private void Start()
    {
        GenerateSegment(4);
    }

    void GenerateSegment(int roomAmount)
    {
        RoomClass generateRoom = null;
        int antiwhile = 0;
        for (int i = 1; i < roomAmount; i++)
        {
            Debug.Log("start createRoom n°" + i);
            
            if (i > 1)
            {
                
                if (hasSpawn && generateRoom != null)
                {
                    spawnPoint = newSpawnPoint;
                    createRoomList.Add(generateRoom.gameObject);
                }
                else
                {
                    i--;
                    hasSpawn = true;
                Debug.Log("less 1 = " + i);
                }
            
            }
            
            if (i == 1)
            {
                Debug.Log("firstRoom");
                int num = Random.Range(0, firstRoom.Length);
                generateRoom =  Instantiate(firstRoom[num]).GetComponent<RoomClass>();
                switch (generateRoom.GetComponent<RoomClass>().ExitArray[0])
                {
                    case open.bot : spawnPoint = generateRoom.GetComponent<RoomClass>().GenBot.transform; break;
                    case open.top : spawnPoint = generateRoom.GetComponent<RoomClass>().GenTop.transform; break;
                    case open.left : spawnPoint = generateRoom.GetComponent<RoomClass>().GenLeft.transform; break;
                    case open.right : spawnPoint = generateRoom.GetComponent<RoomClass>().GenRight.transform; break; 
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
                    Instantiate(generateRoom, posSpawn, quaternion.identity);
                }
                else i--;
            }
            else
            {
                Debug.Log("Room");
                generateRoom = AvailableRoom();
                if (generateRoom != null)
                {
                    newSpawnPoint = GetGen(generateRoom.GetComponent<RoomClass>()).transform;
                    Vector2 posSpawn = spawnPoint.position - newSpawnPoint.position;
                    Instantiate(generateRoom, posSpawn, quaternion.identity);


                    foreach (open op in generateRoom.GetComponent<RoomClass>().ExitArray)
                    {
                        if (op != needOpen)
                        {
                            needOpen = op;
                            break;
                        }
                    }
                        
                    
                
                }
                else i--;
                
                
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
        int num = Random.Range(0, firstRoom.Length);
        if (multiExit) room = multiExitRoom[num].GetComponent<RoomClass>();
        else room = roomArray[num].GetComponent<RoomClass>();
        

        if (!room.ExitArray.Contains(needOpen))
        {
            room = null;
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
