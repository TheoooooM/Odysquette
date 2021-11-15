using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Generation : MonoBehaviour
{
    public enum open
    {
        None, top, left, right, bot
    }
    
    public GameObject StartingRoom;
    public RoomCreator[] normalRoom;
    public GameObject[] endpathRoom;

    public Transform roomPool;
    
    private GameObject[,] map;
    public int mapSize;
    public int nbrOfRoom = 10;

    private GameObject currentRoom;


    private Vector2 currentPos;
    private open needOpen;
    

    private void Start()
    {
        Random.InitState(1);
        map = new GameObject[mapSize, mapSize];
        StartCoroutine("GeneratePath", nbrOfRoom);
    }

    IEnumerator GeneratePath(int size)
    {
        currentPos = new Vector2(mapSize/2, mapSize/2);
        Debug.Log(" middl: " + currentPos);
        RoomContainer firstRC = Instantiate(StartingRoom, new Vector2(currentPos.x*18.4f, currentPos.y*10.4f), Quaternion.identity , roomPool).GetComponent<RoomCreator>().partList[0].RoomGO;
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
        for (int i = 1; i <= size; i++)
        {
            if (reset)
            {
                i--;
                Debug.Log("reset");
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
            }
            else
            {
                Debug.Log("room with " + needOpen);
                RoomCreator newRoom = normalRoom[Random.Range(0, normalRoom.Length)];
                if(newRoom.exitDicitonnary.Count == 0) {newRoom.DictionaryUpdate(); newRoom.PartUpdate();}
                if (newRoom.exitDicitonnary[needOpen].Count != 0)
                {
                    Debug.Log(newRoom.name + " work");
                    RoomContainer enterPart = newRoom.exitDicitonnary[needOpen][Random.Range(0, newRoom.exitDicitonnary[needOpen].Count - 1)];
                    Debug.Log("EnterPart :" + enterPart);
                    

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
                                        Debug.Log( "Cant Spawn");
                                        break;
                                    }
                                }
                            }
                        }
                    Debug.Log("Can Spawn :" + canSpawn);

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
                                Debug.Log("Exit Part : " + exitPart.roomPos + " ExitSide : " + exitSide);
                                
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
                                    Debug.Log(enablePos + " pos of " + map[(int)enablePos.x, (int) enablePos.y]);
                                    if (map[(int)enablePos.x, (int)enablePos.y] == null)
                                    {

                                        currentRoom = Instantiate(new GameObject(), roomPool);
                                        currentRoom.name = "room " + i;
                                        Debug.Log("Instantiate " + currentRoom);
                                        foreach (Room rom in newRoom.partList)
                                        {
                                            RoomContainer RC = Instantiate(rom.RoomGO.gameObject, new Vector2((currentPos.x+rom.RoomGO.roomPos.x-enterPart.roomPos.x)*18.4f,(currentPos.y+rom.RoomGO.roomPos.y-enterPart.roomPos.y)*10.4f), Quaternion.identity, currentRoom.transform).GetComponent<RoomContainer>();
                                            //Debug.Log("currentPos.x:" + currentPos.x + "  partPos.x:" + rom.RoomGO.roomPos.x + "  enterPartPos.x" + enterPart.roomPos.x);
                                            //Debug.Log("currentPos.y:" + currentPos.y + "  partPos.y:" + rom.RoomGO.roomPos.y + "  enterPartPos.y" + enterPart.roomPos.y);
                                            //Debug.Log("Instantiate " + RC.name + " at " +" x:"+ (currentPos.x+rom.RoomGO.roomPos.x-enterPart.roomPos.x)*18.4f + " y:"+ (currentPos.y+rom.RoomGO.roomPos.y-enterPart.roomPos.y)*10.4f);
                                            map[(int)(currentPos.x+rom.RoomGO.roomPos.x-enterPart.roomPos.x),(int)(currentPos.y+rom.RoomGO.roomPos.y-enterPart.roomPos.y)] = RC.gameObject;
                                            
                                            
                                            /*
                                            if (RC != exitPart && exitSide != open.top)RC.exitTop = false;
                                            else Debug.Log("Top");
                                            
                                            if (RC != exitPart && exitSide != open.left)RC.exitLeft = false;
                                            else Debug.Log("Left");
                                            
                                            if (RC != exitPart && exitSide != open.right)RC.exitRight = false;
                                            else Debug.Log("Right");
                                            
                                            if (RC != exitPart && exitSide != open.bot)RC.exitBot = false;
                                            else Debug.Log("Bot");*/

                                            Debug.Log("RC:" +RC.roomPos + " exiPart:" + exitPart.roomPos);
                                            RC.exitLeft = false;
                                            RC.exitRight = false;
                                            RC.exitTop = false;
                                            RC.exitBot = false;
                                            
                                            if(RC.roomPos == exitPart.roomPos)
                                            {
                                                Debug.Log("Rc = exitPart & exitSide =" + exitSide);
                                                if(exitSide == open.top)RC.exitTop = true;
                                                if(exitSide == open.left)RC.exitLeft = true;
                                                if(exitSide == open.right)RC.exitRight = true;
                                                if(exitSide == open.bot)RC.exitBot = true;
                                            }
                                            
                                            if (RC.roomPos == enterPart.roomPos)
                                            {
                                                if(needOpen == open.top)RC.exitTop = true;
                                                if(needOpen == open.left)RC.exitLeft = true;
                                                if(needOpen == open.right)RC.exitRight = true;
                                                if(needOpen == open.bot)RC.exitBot = true;
                                            }
                                            
                                            
                                            RC.UpdatePart();

                                        }
                                        
                                        switch (exitSide)
                                        {
                                            case open.top : needOpen = open.bot; break;
                                            case open.left : needOpen = open.right; break;
                                            case open.right : needOpen = open.left; break;
                                            case open.bot : needOpen = open.top; break;
                                        }

                                        currentPos =
                                            new Vector2((int) (exitPart.roomPos.x - enterPart.roomPos.x + currentPos.x + movePos.x),
                                                        (int) (exitPart.roomPos.y - enterPart.roomPos.y + currentPos.y + movePos.y));
                                        
                                        Debug.Log("Finish");
                                        ready = true;
                                        break;
                                    }
                                }
                                
                            }
                            
                            x++;
                            if(x == 20)
                            {
                                Debug.Log("anti while break");
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
                Debug.Log("anti while break at " + i);
                break;
            }
        }
    }
}
