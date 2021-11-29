using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Generation : MonoBehaviour
{
    public static Generation Instance;
    private void Awake()
    {
        Instance = this;
    }

    public enum open
    {
        None, top, left, right, bot
    }
    [Header("===================Rooms===================")]
    public GameObject StartingRoom;
    public RoomCreator[] normalRoom;
    public GameObject endpathRoom;
    [Header("======================================")]


    [HideInInspector] public Transform roomPool;
    
    public GameObject[,] map;
    public int mapSize = 51;
    public int nbrOfRoom = 10;

    private RoomManager currentRoom;


    private Vector2 currentPos;
    private open needOpen;

    [Header("====================Camera====================")]
    public Rect BasicRect;

    private void Start()
    {
        //Random.InitState();
        map = new GameObject[mapSize, mapSize];
        //StartCoroutine("GeneratePath", nbrOfRoom);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ResetGen();
            StartCoroutine("GeneratePath", nbrOfRoom);
        }
    }

    IEnumerator GeneratePath(int size)
    {
        currentPos = new Vector2(mapSize/2, mapSize/2);
        //Debug.Log(" middl: " + currentPos);
        GameObject nR = new GameObject();
        nR.AddComponent(typeof(RoomManager));
        nR.transform.parent = roomPool;
        currentRoom = nR.GetComponent<RoomManager>();
        
        currentRoom.name = "First Room";
        currentRoom.runningRoom = true;
        currentRoom.cameraRect = BasicRect;
        currentRoom.cameraRect.x = -BasicRect.x / 2;
        currentRoom.cameraRect.y = -BasicRect.y / 2;
        
        RoomContainer firstRC = Instantiate(StartingRoom, new Vector2((currentPos.x - mapSize/2)*62, (currentPos.y- mapSize/2)*40), Quaternion.identity , currentRoom.transform).GetComponent<RoomCreator>().partList[0].RoomGO;
        firstRC.room = currentRoom;
        firstRC.roomMapPos = currentPos;
        map[(int) currentPos.x, (int) currentPos.y] = firstRC.gameObject;
        
        
        int exit = Random.Range(0, 3);

        switch (exit)
        {
            case 0 : 
                firstRC.exitTop = true; 
                needOpen = open.bot; 
                firstRC.UpdatePart();
                currentPos.y++;
                break;
            
            case 1 : firstRC.exitLeft = true; 
                needOpen = open.right; 
                firstRC.UpdatePart(); 
                currentPos.x--;
                break;
            
            case 2 : firstRC.exitRight = true; 
                needOpen = open.left; 
                firstRC.UpdatePart(); 
                currentPos.x++;
                break;
            
            case 3 : firstRC.exitBot = true; 
                needOpen = open.top; 
                firstRC.UpdatePart(); 
                currentPos.y--;
                break;
        }

        bool reset = false;

        int k = 0;
        for (int i = 1; i <= nbrOfRoom; i++)
        {
            if (reset)
            {
                i--;
                //Debug.Log("reset");
                reset = false;
            }
            else
            {
                yield return new WaitForSeconds(0.05f);
            }

            Debug.Log("Create Room n°" + i);
            
            if (i == size)
            {
                //dernière salle
                //RoomManager RM = Instantiate(new GameObject(), roomPool).AddComponent<RoomManager>();
                //RM.transform.name = "LastRoom";
                RoomContainer RC = Instantiate(endpathRoom.GetComponent<RoomCreator>().partList[0].RoomGO, new Vector2((currentPos.x- mapSize/2)*9.92f,(currentPos.y - mapSize/2)*6.4f), Quaternion.identity, roomPool).GetComponent<RoomContainer>();
                
                RC.exitLeft = false;
                RC.exitRight = false;
                RC.exitTop = false;
                RC.exitBot = false;
                switch (needOpen)
                {
                    case open.right : RC.exitRight = true; break;
                    case open.left : RC.exitLeft = true; break;
                    case open.bot : RC.exitBot = true; break;
                    case open.top : RC.exitTop = true; break;
                }
                RC.UpdatePart();
            }
            else
            {
                //Debug.Log("room with " + needOpen);
                RoomCreator newRoom = normalRoom[Random.Range(0, normalRoom.Length)];
                if(newRoom.exitDicitonnary.Count == 0) {newRoom.DictionaryUpdate(); newRoom.PartUpdate();}
                if (newRoom.exitDicitonnary[needOpen].Count != 0)
                {
                    //debug.Log(newRoom.name + " work");
                    RoomContainer enterPart = newRoom.exitDicitonnary[needOpen][Random.Range(0, newRoom.exitDicitonnary[needOpen].Count - 1)];
                    //debug.Log("EnterPart :" + enterPart);
                    

                    bool canSpawn = true;
                    foreach (Room Rom in newRoom.partList)
                        {
                            RoomContainer rc = Rom.RoomGO;
                            if (rc != enterPart)
                            {
                                if ((rc.roomPos.x-enterPart.roomPos.x + currentPos.x) >= 0 && (rc.roomPos.x-enterPart.roomPos.x + currentPos.x) < mapSize && 
                                    (rc.roomPos.y-enterPart.roomPos.y + currentPos.y) >= 0 && (rc.roomPos.y-enterPart.roomPos.y + currentPos.y) < mapSize)
                                {
                                    if (map[(int)(rc.roomPos.x-enterPart.roomPos.x + currentPos.x), (int)(rc.roomPos.y-enterPart.roomPos.y + currentPos.y)] != null)
                                    {
                                        canSpawn = false;
                                        //debug.Log( "Cant Spawn");
                                        break;
                                    }
                                }
                            }
                        }
                    //debug.Log("Can Spawn :" + canSpawn);

                    if (canSpawn)
                    {
                        bool ready = false;
                        int x = 0;
                        while (!ready)
                        {
                            RoomContainer exitPart = newRoom.partList[Random.Range(0, newRoom.partList.Length)].RoomGO;
                            
                            if (exitPart != enterPart || newRoom.partList.Length == 1)
                            {
                                open exitSide = exitPart.exitList[Random.Range(0, exitPart.exitAmount)];
                                //debug.Log("Exit Part : " + exitPart.roomPos + " ExitSide : " + exitSide);
                                
                                Vector2 movePos = Vector2.zero;
                                switch (exitSide)
                                {
                                    case open.top : movePos = new Vector2(0, 1); break;
                                    case open.left : movePos = new Vector2(-1, 0); break;
                                    case open.right : movePos = new Vector2(1, 0); break;
                                    case open.bot : movePos = new Vector2(0, -1); break;
                                }

                                if ((exitPart.roomPos.x-enterPart.roomPos.x + currentPos.x + movePos.x) >= 0 && (exitPart.roomPos.x-enterPart.roomPos.x + currentPos.x + movePos.x) < mapSize && 
                                    (exitPart.roomPos.y-enterPart.roomPos.y + currentPos.y + movePos.y) >= 0 && (exitPart.roomPos.y-enterPart.roomPos.y + currentPos.y + movePos.y) < mapSize)
                                {
                                    Vector2 enablePos = new Vector2(
                                        (int) (exitPart.roomPos.x - enterPart.roomPos.x + currentPos.x + movePos.x),
                                        (int) (exitPart.roomPos.y - enterPart.roomPos.y + currentPos.y + movePos.y));
                                    //debug.Log(enablePos + " pos of " + map[(int)enablePos.x, (int) enablePos.y]);
                                    if (map[(int)enablePos.x, (int)enablePos.y] == null)
                                    {
                                        nR = new GameObject();
                                        nR.AddComponent(typeof(RoomManager));
                                        nR.transform.parent = roomPool;
                                        currentRoom = nR.GetComponent<RoomManager>();
                                        
                                        currentRoom.ennemiesList = new List<GameObject>(newRoom.ennemiList);
                                        currentRoom.name = "room " + i;
                                        Rect roomRect = BasicRect;
                                        Debug.Log("current pos x -  mapsize :" + (currentPos.x - mapSize / 2) + "  basicRect x :" + BasicRect.x + "  so " + ((currentPos.x - mapSize / 2) * BasicRect.x - (BasicRect.x*1.5f)));
                                        roomRect.x += (currentPos.x - mapSize / 2) * BasicRect.x -(BasicRect.x*1.5f); //(currentPos.x - Mathf.CeilToInt(mapSize / 2)) * BasicRect.x - (BasicRect.x*1.5f);
                                        roomRect.y += (currentPos.y - mapSize / 2) * BasicRect.y -(BasicRect.y*1.5f); //(currentPos.y - mapSize / 2) * BasicRect.y - (BasicRect.y*1.5f);
                                        
                                        //debug.Log("Instantiate " + currentRoom);
                                        foreach (Room rom in newRoom.partList)
                                        {
                                            Vector2 posInRoom = new Vector2(rom.RoomGO.roomPos.x - enterPart.roomPos.x, rom.RoomGO.roomPos.y - enterPart.roomPos.y);
                                            Debug.Log(posInRoom);
                                            if (posInRoom == new Vector2(0, 1)) roomRect.height += BasicRect.height;
                                            else if (posInRoom == new Vector2(0, -1))
                                            {
                                                roomRect.y -= BasicRect.y;
                                                roomRect.height += BasicRect.height;
                                            }
                                            else if(posInRoom == new Vector2(1,0)) roomRect.width += BasicRect.width;
                                            else if (posInRoom == new Vector2(-1, 0))
                                            {
                                                roomRect.x -= BasicRect.x;
                                                roomRect.width += BasicRect.width;
                                            }
                                            RoomContainer RC = Instantiate(rom.RoomGO.gameObject, new Vector2((currentPos.x+rom.RoomGO.roomPos.x-enterPart.roomPos.x- mapSize/2)*62,(currentPos.y+rom.RoomGO.roomPos.y-enterPart.roomPos.y- mapSize/2)*40), Quaternion.identity, currentRoom.transform).GetComponent<RoomContainer>();
                                            RC.roomMapPos = new Vector2((currentPos.x + rom.RoomGO.roomPos.x - enterPart.roomPos.x), (int) (currentPos.y + rom.RoomGO.roomPos.y - enterPart.roomPos.y));
                                            map[(int)(currentPos.x+rom.RoomGO.roomPos.x-enterPart.roomPos.x),(int)(currentPos.y+rom.RoomGO.roomPos.y-enterPart.roomPos.y)] = RC.gameObject;
                                            RC.room = currentRoom;
                                            
                                            
                                            RC.exitLeft = false;
                                            RC.exitRight = false;
                                            RC.exitTop = false;
                                            RC.exitBot = false;
                                            
                                            if(RC.roomPos == exitPart.roomPos)
                                            {
                                                switch (exitSide)
                                                {
                                                    case open.top : 
                                                        RC.exitTop = true; 
                                                        RC.room.exitGO = RC.closeTop.gameObject; break;
                                                    
                                                    case open.left : 
                                                        RC.exitLeft = true; 
                                                        RC.room.exitGO = RC.closeLeft.gameObject; break;
                                                    
                                                    case open.right : 
                                                        RC.exitRight = true; 
                                                        RC.room.exitGO = RC.closeRight.gameObject; break;
                                                    
                                                    case open.bot : 
                                                        RC.exitBot = true; 
                                                        RC.room.exitGO = RC.closeBot.gameObject; break;
                                                }
                                            }
                                            
                                            if (RC.roomPos == enterPart.roomPos)
                                            {
                                                switch (needOpen)
                                                {
                                                    case open.top : 
                                                        RC.exitTop = true;
                                                        RC.room.enterGO = RC.closeTop.gameObject; break;
                                                    
                                                    case open.left : 
                                                        RC.exitLeft = true;
                                                        RC.room.enterGO = RC.closeLeft.gameObject; break;
                                                    
                                                    case open.right : 
                                                        RC.exitRight = true;
                                                        RC.room.enterGO = RC.closeRight.gameObject; break;
                                                    
                                                    case open.bot : 
                                                        RC.exitBot = true;
                                                        RC.room.enterGO = RC.closeBot.gameObject; break;
                                                }
                                            }
                                            
                                            
                                            RC.UpdatePart();

                                        }

                                        currentRoom.cameraRect = roomRect;
                                        
                                        switch (exitSide)
                                        {
                                            case open.top : needOpen = open.bot; break;
                                            case open.left : needOpen = open.right; break;
                                            case open.right : needOpen = open.left; break;
                                            case open.bot : needOpen = open.top; break;
                                        }

                                        currentPos = new Vector2((int) (exitPart.roomPos.x - enterPart.roomPos.x + currentPos.x + movePos.x),
                                                        (int) (exitPart.roomPos.y - enterPart.roomPos.y + currentPos.y + movePos.y));
                                        
                                        //Debug.Log("Finish");
                                        ready = true;
                                        break;
                                    }
                                }
                                
                            }
                            
                            x++;
                            if(x == 20)
                            {
                                //debug.Log("anti while break");
                                reset = true;
                                break;
                            } 
                        }
                        
                        
                    }
                    else
                    {
                        reset = true;
                    }
                }
                else
                {
                    reset = true;
                }
            }

            k++;
            if(k == 10*size)
            {
                
                //debug.Log("anti while break at " + i);
                ResetGen();
                StartCoroutine("GeneratePath", nbrOfRoom);
                break;
            }
        }
    }

    /// <summary>
    /// Reset the generation by destroying the default rommPool and creating a new one
    /// </summary>
    void ResetGen()
    {
        if(roomPool != null) Destroy(roomPool.gameObject);
        roomPool = Instantiate(new GameObject(), transform).transform;
        roomPool.name = "RoomPool";
    }
}
