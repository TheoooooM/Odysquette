using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Generation : MonoBehaviour {
    #region VARIABLES

    public static Generation Instance;

    private void Awake() {
        Instance = this;
    }

    public enum open {
        None,
        top,
        left,
        right,
        bot
    }

    [Header("--- GENERATION")]
    [SerializeField] private bool generateRandomLevel = false;
    [SerializeField] private bool forceRedLight = false;
    public int seed = 0;
    public bool disableNeighboor = false;
    [HideInInspector] public bool endGeneration;

    [Header("--- ROOMS")] public GameObject StartingRoom;
    public RoomCreator[] normalRoom;
    public GameObject endpathRoom;
    private List<RoomManager> roomList = new List<RoomManager>();
    [SerializeField] private List<GameObject> OutsideRoom = new List<GameObject>();
    private RoomManager firstRoom = null;
    
    public List<RoomManager> RoomList => roomList;
    [Space] public int mapSize = 51;
    public int nbrOfRoom = 10;

    [HideInInspector] public Transform roomPool;
    public RoomContainer[,] map;
    private bool[,] outsideMap;
    private RoomManager currentRoom;
    [SerializeField] private int lastRoomId = -1;
    

    private Vector2 currentPos;
    private open needOpen;

    [Header("---- SCENE MANAGER")] public SceneManager sceneMan;
    [Header("---- CAMERA")] public Rect BasicRect;

    #endregion VARIABLES

    private void Start() {
        sceneMan.StartIdleOpen();
        AddCommandToConsole();
        
        lastRoomId = -1;
        map = new RoomContainer[mapSize, mapSize];
        outsideMap = new bool[mapSize, mapSize];
        if (NeverDestroy.Instance == null) Instantiate(Resources.Load<GameObject>("NeverDestroy"));
        NeverDestroy.Instance.level++;
        GenerateLevel();
    }

    private void AddCommandToConsole() {
        CommandConsole RELOAD = new CommandConsole("reload", "reload : Reload the actual generation of the level", null, (_) => { GenerateLevel(); });
        CommandConsole ENDROOM = new CommandConsole("endroom", "endroom : Kill all the ennemies in the actual room", null, (_) => {
            foreach (RoomManager room in roomList) {
                if (room.runningRoom) {
                    List<GameObject> enemyList = new List<GameObject>(room.ennemiesList);

                    foreach (GameObject enemy in enemyList) {
                        enemy.GetComponentInChildren<EnemyStateManager>().OnDeath();
                    }
            
                    room.ennemiesList.Clear();
                }
            }
        });
        
        CommandConsoleRuntime.Instance.AddCommand(RELOAD);
        CommandConsoleRuntime.Instance.AddCommand(ENDROOM);
    }
    

    /// <summary>
    /// Generate the level
    /// </summary>
    public void GenerateLevel() {
        int actualSeed = seed;
        if (generateRandomLevel) actualSeed = Random.Range(0, 1000000);
        
        Random.InitState(actualSeed);
        ResetGen();
        StartCoroutine("GeneratePath", nbrOfRoom);
    }
    
    private bool reset = false;
    /// <summary>
    /// Generate the rooms
    /// </summary>
    /// <param name="size"></param>
    /// <returns></returns>
    private IEnumerator GeneratePath(int size) {
        endGeneration = false;
        currentPos = new Vector2(mapSize / 2, mapSize / 2);

        UIManager.Instance.loadingBar.maxValue = nbrOfRoom;
        UIManager.Instance.loadingBar.value = 0;
        UIManager.Instance.LoadingScreen.SetActive(true);

        GenerateFirstRoom();
        
        reset = false;

        int k = 0;
        for (int i = 1; i <= nbrOfRoom; i++) {
            if (reset) {
                i--;
                reset = false;
            }
            else {
                yield return new WaitForSeconds(0.05f);
            }
            
            UIUpdate(i);
            
            if (i == size) {
                if (endpathRoom != null) GenerateLastRoom();
                foreach (RoomManager room in roomList) {
                    if (room.exitGO != null)room.exitGO.SetActive(true);
                }
                
                if(OutsideRoom.Count != 0) StartCoroutine(GenerateOutsideRooms());

                    yield return new WaitForSeconds(0.05f);
                    ReGeneratePath();
                
                foreach (RoomManager room in roomList)
                {
                    // Debug.Log("get exitGO from" + room.gameObject.name);
                    
                    if (room.exitGO != null)room.exitGO.SetActive(false);
                }
            }
            else GenerateRooms(i);

            k++;
            if (k == 10 * size) {
                ResetGen();
                StartCoroutine("GeneratePath", nbrOfRoom);
                break;
            }
        }

        endGeneration = true;
        UIUpdate();
    }
    
    #region ROOM GENERATION
    /// <summary>
    /// Generate the first room
    /// </summary>
    private void GenerateFirstRoom() {
        GameObject nR = new GameObject();
        nR.AddComponent(typeof(RoomManager));
        nR.transform.parent = roomPool;
        currentRoom = nR.GetComponent<RoomManager>();
        firstRoom = currentRoom;
        if(GameManager.Instance != null && GameManager.Instance.Player != null) GameManager.Instance.Player.transform.position = firstRoom.transform.position;

        currentRoom.name = "First Room";
        currentRoom.runningRoom = true;
        currentRoom.cameraRect = BasicRect;
        currentRoom.cameraRect.x = -BasicRect.x / 2;
        currentRoom.cameraRect.y = -BasicRect.y / 2;

        RoomContainer firstRC = Instantiate(StartingRoom, new Vector2((currentPos.x - mapSize / 2) * 62, (currentPos.y - mapSize / 2) * 40), Quaternion.identity, currentRoom.transform).GetComponent<RoomCreator>().partList[0].RoomGO;
        firstRC.room = currentRoom;
        firstRC.Generator = this;
        firstRC.roomMapPos = currentPos;
        firstRC.firstRoom = true;
        map[(int) currentPos.x, (int) currentPos.y] = firstRC;


        int exit = Random.Range(0, 3);

        switch (exit) {
            case 0:
                firstRC.exitTop = true;
                needOpen = open.bot;
                firstRC.UpdatePart();
                firstRC.closeRoom.UpdateCloseRoom(true, false, false, false, true, NeverDestroy.Instance.level == 2 || forceRedLight);
                currentPos.y++;

                break;

            case 1:
                firstRC.exitLeft = true;
                needOpen = open.right;
                firstRC.UpdatePart();
                firstRC.closeRoom.UpdateCloseRoom(false, false, false, true, true, NeverDestroy.Instance.level == 2 || forceRedLight);
                currentPos.x--;
                break;

            case 2:
                firstRC.exitRight = true;
                needOpen = open.left;
                firstRC.UpdatePart();
                firstRC.closeRoom.UpdateCloseRoom(false, true, false, false, true, NeverDestroy.Instance.level == 2 || forceRedLight);
                currentPos.x++;
                break;

            case 3:
                firstRC.exitBot = true;
                needOpen = open.top;
                firstRC.UpdatePart();
                firstRC.closeRoom.UpdateCloseRoom(false, false, true, false, true, NeverDestroy.Instance.level == 2 || forceRedLight);
                currentPos.y--;
                break;
        }

    }

    /// <summary>
    /// Generate the last room
    /// </summary>
    private void GenerateLastRoom() {
        GameObject GO = Instantiate(new GameObject(), roomPool);
        GO.transform.name = "LastRoom";
        GO.AddComponent<RoomManager>();
        roomList.Add(GO.GetComponent<RoomManager>());         
        RoomContainer RC = Instantiate(endpathRoom.GetComponent<RoomCreator>().partList[0].RoomGO, new Vector2((currentPos.x- mapSize/2)*62,(currentPos.y - mapSize/2)*40f), Quaternion.identity, GO.transform).GetComponent<RoomContainer>();
        RC.roomMapPos = new Vector2((currentPos.x), (int) (currentPos.y));
        RC.Generator = this;
                    
        currentRoom.partList.Add(RC);
        map[(int)(currentPos.x),(int)(currentPos.y)] = RC;
                    
        RC.room = currentRoom;
        RC.exitLeft = false;
        RC.exitRight = false;
        RC.exitTop = false;
        RC.exitBot = false;
                    
        switch (needOpen) {
            case open.right:
                RC.exitRight = true;
                RC.closeRoom.UpdateCloseRoom(false, true, false, false, true, NeverDestroy.Instance.level == 2 || forceRedLight);
                break;
            case open.left:
                RC.exitLeft = true;
                RC.closeRoom.UpdateCloseRoom(false, false, false, true, true, NeverDestroy.Instance.level == 2 || forceRedLight);
                break;
            case open.bot:
                RC.exitBot = true;
                RC.closeRoom.UpdateCloseRoom(false, false, true, false, true, NeverDestroy.Instance.level == 2 || forceRedLight);
                break;
            case open.top:
                RC.exitTop = true;
                RC.closeRoom.UpdateCloseRoom(true, false, false, false, true, NeverDestroy.Instance.level == 2 || forceRedLight);
                break;
        }

        RC.UpdatePart();
    }

    /// <summary>
    /// Generate all the room
    /// </summary>
    private void GenerateRooms(int i) {
        List<int> validRooms = new List<int>();
        for (int j = 0; j < normalRoom.Length; j++) {
            validRooms.Add(j);
        }

        if (lastRoomId != -1) validRooms.Remove(lastRoomId);
        
        int newRoomID = validRooms[Random.Range(0, validRooms.Count)];
        RoomCreator newRoom = normalRoom[newRoomID];

        if (newRoom.exitDicitonnary.Count == 0) {
            newRoom.DictionaryUpdate();
            newRoom.PartUpdate();
        }

        if (newRoom.exitDicitonnary[needOpen].Count != 0) {
            //debug.Log(newRoom.name + " work");
            RoomContainer enterPart = newRoom.exitDicitonnary[needOpen][Random.Range(0, newRoom.exitDicitonnary[needOpen].Count - 1)];
            //debug.Log("EnterPart :" + enterPart);


            bool canSpawn = true;
            foreach (Room Rom in newRoom.partList) {
                RoomContainer rc = Rom.RoomGO;
                if (rc != enterPart) {
                    if ((rc.roomPos.x - enterPart.roomPos.x + currentPos.x) >= 0 && (rc.roomPos.x - enterPart.roomPos.x + currentPos.x) < mapSize &&
                        (rc.roomPos.y - enterPart.roomPos.y + currentPos.y) >= 0 && (rc.roomPos.y - enterPart.roomPos.y + currentPos.y) < mapSize) {
                        if (map[(int) (rc.roomPos.x - enterPart.roomPos.x + currentPos.x), (int) (rc.roomPos.y - enterPart.roomPos.y + currentPos.y)] != null) {
                            canSpawn = false;
                            //debug.Log( "Cant Spawn");
                            break;
                        }
                    }
                }
            }
            //debug.Log("Can Spawn :" + canSpawn);

            if (canSpawn) {
                bool ready = false;
                int x = 0;
                while (!ready) {
                    RoomContainer exitPart = newRoom.partList[Random.Range(0, newRoom.partList.Length)].RoomGO;

                    if (exitPart != enterPart || newRoom.partList.Length == 1) {
                        open exitSide = exitPart.exitList[Random.Range(0, exitPart.exitAmount)];
                        //debug.Log("Exit Part : " + exitPart.roomPos + " ExitSide : " + exitSide);

                        Vector2 movePos = Vector2.zero;
                        switch (exitSide) {
                            case open.top:
                                movePos = new Vector2(0, 1);
                                break;
                            case open.left:
                                movePos = new Vector2(-1, 0);
                                break;
                            case open.right:
                                movePos = new Vector2(1, 0);
                                break;
                            case open.bot:
                                movePos = new Vector2(0, -1);
                                break;
                        }

                        if ((exitPart.roomPos.x - enterPart.roomPos.x + currentPos.x + movePos.x) >= 0 && (exitPart.roomPos.x - enterPart.roomPos.x + currentPos.x + movePos.x) < mapSize && (exitPart.roomPos.y - enterPart.roomPos.y + currentPos.y + movePos.y) >= 0 && (exitPart.roomPos.y - enterPart.roomPos.y + currentPos.y + movePos.y) < mapSize) {
                            Vector2 enablePos = new Vector2((int) (exitPart.roomPos.x - enterPart.roomPos.x + currentPos.x + movePos.x), (int) (exitPart.roomPos.y - enterPart.roomPos.y + currentPos.y + movePos.y));

                            if (map[(int) enablePos.x, (int) enablePos.y] == null) {
                                GameObject newR = new GameObject();
                                newR.AddComponent(typeof(RoomManager));
                                newR.transform.parent = roomPool;
                                currentRoom = newR.GetComponent<RoomManager>();
                                roomList.Add(currentRoom);
                                currentRoom.chest = newRoom.chest;
                                currentRoom.exit = newRoom.exit;

                                currentRoom.ennemiesList = new List<GameObject>(newRoom.ennemiList);
                                currentRoom.name = "room " + i;
                                Rect roomRect = BasicRect;

                                //Debug.Log("current pos x -  mapsize :" + (currentPos.x - mapSize / 2) + "  basicRect x :" + BasicRect.x + "  so " + ((currentPos.x - mapSize / 2) * BasicRect.x - (BasicRect.x * 1.5f)));
                                roomRect.x += (currentPos.x - mapSize / 2) * BasicRect.x - (BasicRect.x * 1.5f);
                                roomRect.y += (currentPos.y - mapSize / 2) * BasicRect.y - (BasicRect.y * 1.5f);

                                //debug.Log("Instantiate " + currentRoom);
                                foreach (Room rom in newRoom.partList) {
                                    Vector2 posInRoom = new Vector2(rom.RoomGO.roomPos.x - enterPart.roomPos.x, rom.RoomGO.roomPos.y - enterPart.roomPos.y);
                                    //.Log(posInRoom);
                                    if (posInRoom == new Vector2(0, 1)) roomRect.height += BasicRect.height;
                                    else if (posInRoom == new Vector2(0, -1)) {
                                        roomRect.y -= BasicRect.y;
                                        roomRect.height += BasicRect.height;
                                    }
                                    else if (posInRoom == new Vector2(1, 0)) roomRect.width += BasicRect.width;
                                    else if (posInRoom == new Vector2(-1, 0)) {
                                        roomRect.x -= BasicRect.x;
                                        roomRect.width += BasicRect.width;
                                    }

                                    RoomContainer RC = Instantiate(rom.RoomGO.gameObject, new Vector2((currentPos.x + rom.RoomGO.roomPos.x - enterPart.roomPos.x - mapSize / 2) * 62, (currentPos.y + rom.RoomGO.roomPos.y - enterPart.roomPos.y - mapSize / 2) * 40), Quaternion.identity, currentRoom.transform).GetComponent<RoomContainer>();
                                    RC.roomMapPos = new Vector2((currentPos.x + rom.RoomGO.roomPos.x - enterPart.roomPos.x), (int) (currentPos.y + rom.RoomGO.roomPos.y - enterPart.roomPos.y));
                                    RC.Generator = this;
                                    currentRoom.partList.Add(RC);
                                    map[(int) (currentPos.x + rom.RoomGO.roomPos.x - enterPart.roomPos.x), (int) (currentPos.y + rom.RoomGO.roomPos.y - enterPart.roomPos.y)] = RC;
                                    RC.room = currentRoom;


                                    RC.exitLeft = false;
                                    RC.exitRight = false;
                                    RC.exitTop = false;
                                    RC.exitBot = false;

                                    if (RC.roomPos == exitPart.roomPos) {
                                        switch (exitSide) {
                                            case open.top:
                                                RC.exitTop = true;
                                                RC.room.exitGO = RC.closeTop.gameObject;
                                                //Debug.Log("set exit top at room n°" + i);
                                                break;

                                            case open.left:
                                                RC.exitLeft = true;
                                                RC.room.exitGO = RC.closeLeft.gameObject;
                                                //Debug.Log("set exit left at room n°" + i);
                                                break;

                                            case open.right:
                                                RC.exitRight = true;
                                                RC.room.exitGO = RC.closeRight.gameObject;
                                                //Debug.Log("set exit right at room n°" + i);
                                                break;

                                            case open.bot:
                                                RC.exitBot = true;
                                                RC.room.exitGO = RC.closeBot.gameObject;
                                                //Debug.Log("set exit bot at room n°" + i);
                                                break;
                                        }
                                    }

                                    if (RC.roomPos == enterPart.roomPos) {
                                        switch (needOpen) {
                                            case open.top:
                                                RC.exitTop = true;
                                                RC.room.enterGO = RC.closeTop.gameObject;
                                                RC.closeRoom.UpdateCloseRoom(true, exitSide == open.right, exitSide == open.bot, exitSide == open.left, true, NeverDestroy.Instance.level == 2 || forceRedLight);
                                                break;

                                            case open.left:
                                                RC.exitLeft = true;
                                                RC.room.enterGO = RC.closeLeft.gameObject;
                                                RC.closeRoom.UpdateCloseRoom(exitSide == open.top, exitSide == open.right, exitSide == open.bot, true, true, NeverDestroy.Instance.level == 2 || forceRedLight);
                                                break;

                                            case open.right:
                                                RC.exitRight = true;
                                                RC.room.enterGO = RC.closeRight.gameObject;
                                                RC.closeRoom.UpdateCloseRoom(exitSide == open.top, true, exitSide == open.bot, exitSide == open.left, true, NeverDestroy.Instance.level == 2 || forceRedLight);
                                                break;

                                            case open.bot:
                                                RC.exitBot = true;
                                                RC.room.enterGO = RC.closeBot.gameObject;
                                                RC.closeRoom.UpdateCloseRoom(exitSide == open.top, exitSide == open.right, true, exitSide == open.left, true, NeverDestroy.Instance.level == 2 || forceRedLight);
                                                break;
                                        }
                                    }

                                    RC.UpdatePart();
                                }

                                currentRoom.cameraRect = roomRect;
                                needOpen = exitSide switch {open.top => open.bot, open.left => open.right, open.right => open.left, open.bot => open.top, _ => needOpen};
                                currentPos = new Vector2((int) (exitPart.roomPos.x - enterPart.roomPos.x + currentPos.x + movePos.x), (int) (exitPart.roomPos.y - enterPart.roomPos.y + currentPos.y + movePos.y));
                                ready = true;
                                lastRoomId = newRoomID;
                                break;
                            }
                        }
                    }

                    x++;
                    if (x == 20) reset = true;
                }
            }
            else reset = true;
        }
        else reset = true;
    }

    private enum RoomDirection { North, NorthWest, West, SouthWest, South, SouthEast, East, NorthEast };
    
    /// <summary>
    /// Generate Outside Rooms
    /// </summary>
    private IEnumerator GenerateOutsideRooms() {
        Vector2Int mapPos = new Vector2Int((int) firstRoom.transform.GetChild(0).GetChild(0).GetComponent<RoomContainer>().roomMapPos.x, (int) firstRoom.transform.GetChild(0).GetChild(0).GetComponent<RoomContainer>().roomMapPos.y);
        SearchNeighboorForInstanatiation(mapPos, firstRoom.transform.GetChild(0).GetChild(0).transform.position);
        
        foreach (RoomManager room in roomList) {
            for (int i = 0; i < room.transform.childCount; i++) {
                mapPos = new Vector2Int((int) room.transform.GetChild(i).GetComponent<RoomContainer>().roomMapPos.x, (int) room.transform.GetChild(i).GetComponent<RoomContainer>().roomMapPos.y);
                SearchNeighboorForInstanatiation(mapPos, room.transform.GetChild(i).transform.position);
            }
        }
        yield return null;
    }

    /// <summary>
    /// Search if a neighboor exist in all directions
    /// </summary>
    /// <param name="mapPos"></param>
    /// <param name="pos"></param>
    private void SearchNeighboorForInstanatiation(Vector2Int mapPos, Vector3 pos) {
        if (map[mapPos.x + 1, mapPos.y] == null && !outsideMap[mapPos.x + 1, mapPos.y]) {
            //Debug.Log(map[mapPos.x + 1, mapPos.y]);
            SpawnOutsideRoom(RoomDirection.West, pos, new Vector2Int(mapPos.x + 1, mapPos.y));
        }

        if (map[mapPos.x - 1, mapPos.y] == null && !outsideMap[mapPos.x - 1, mapPos.y]) {
            //Debug.Log(map[mapPos.x - 1, mapPos.y]);
            SpawnOutsideRoom(RoomDirection.East, pos, new Vector2Int(mapPos.x - 1, mapPos.y));
        }

        if (map[mapPos.x, mapPos.y + 1] == null && !outsideMap[mapPos.x, mapPos.y + 1]) {
            //Debug.Log(map[mapPos.x, mapPos.y + 1]);
            SpawnOutsideRoom(RoomDirection.North, pos, new Vector2Int(mapPos.x, mapPos.y + 1));
        }

        if (map[mapPos.x, mapPos.y - 1] == null && !outsideMap[mapPos.x, mapPos.y - 1]) {
            //Debug.Log(map[mapPos.x, mapPos.y - 1]);
            SpawnOutsideRoom(RoomDirection.South, pos, new Vector2Int(mapPos.x, mapPos.y - 1));
        }

        if (map[mapPos.x + 1, mapPos.y + 1] == null && !outsideMap[mapPos.x + 1, mapPos.y + 1]) {
            //Debug.Log(map[mapPos.x + 1, mapPos.y + 1]);
            SpawnOutsideRoom(RoomDirection.NorthWest, pos, new Vector2Int(mapPos.x + 1, mapPos.y + 1));
        }

        if (map[mapPos.x - 1, mapPos.y + 1] == null && !outsideMap[mapPos.x - 1, mapPos.y + 1]) {
            //Debug.Log(map[mapPos.x - 1, mapPos.y + 1]);
            SpawnOutsideRoom(RoomDirection.NorthEast, pos, new Vector2Int(mapPos.x - 1, mapPos.y + 1));
        }

        if (map[mapPos.x + 1, mapPos.y - 1] == null && !outsideMap[mapPos.x + 1, mapPos.y - 1]) {
            //Debug.Log(map[mapPos.x + 1, mapPos.y - 1]);
            SpawnOutsideRoom(RoomDirection.SouthWest, pos, new Vector2Int(mapPos.x + 1, mapPos.y - 1));
        }

        if (map[mapPos.x - 1, mapPos.y - 1] == null && !outsideMap[mapPos.x - 1, mapPos.y - 1]) {
            //Debug.Log(map[mapPos.x - 1, mapPos.y - 1]);
            SpawnOutsideRoom(RoomDirection.SouthEast, pos, new Vector2Int(mapPos.x - 1, mapPos.y - 1));
        }
    }
    
    /// <summary>
    /// Generate outside Rooms if possible
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="basePos"></param>
    private void SpawnOutsideRoom(RoomDirection dir, Vector3 basePos, Vector2Int mapPos) {
        int randomRoom = Random.Range(0, OutsideRoom.Count);
        Vector3 finalRoomPos = basePos;
        switch (dir) {
            case RoomDirection.North:
                finalRoomPos += new Vector3(0,40);
                break;
            case RoomDirection.West:
                finalRoomPos += new Vector3(62,0);
                break;
            case RoomDirection.South:
                finalRoomPos += new Vector3(0,-40);
                break;
            case RoomDirection.East:
                finalRoomPos += new Vector3(-62,0);
                break;
            case RoomDirection.NorthWest:
                finalRoomPos += new Vector3(62,40);
                break;
            case RoomDirection.SouthWest:
                finalRoomPos += new Vector3(62,-40);
                break;
            case RoomDirection.SouthEast:
                finalRoomPos += new Vector3(-62,-40);
                break;
            case RoomDirection.NorthEast:
                finalRoomPos += new Vector3(-62,40);
                break;
        }
        GameObject outsideR = Instantiate(OutsideRoom[randomRoom], finalRoomPos, Quaternion.identity, roomPool);
        RoomContainer RC = outsideR.GetComponent<RoomContainer>();
        RC.Generator = this;
        if (outsideR != null) {
            outsideMap[mapPos.x, mapPos.y] = true;
            map[mapPos.x, mapPos.y] = RC;
        }
    }
    #endregion ROOM GENERATION
    
    /// <summary>
    /// Regenerate NavMesh
    /// </summary>
    private void ReGeneratePath() => AstarPath.active.Scan();

    /// <summary>
    /// Reset the generation by destroying the default rommPool and creating a new one
    /// </summary>
    private void ResetGen() {
        if (roomPool != null) Destroy(roomPool.gameObject);
        GameObject poolGam  = new GameObject();
        poolGam.transform.parent = transform;
        roomPool = poolGam.transform;
        roomPool.name = "RoomPool";
        
        roomList.Clear();
        
        map = new RoomContainer[mapSize, mapSize];
        outsideMap = new bool[mapSize, mapSize];
    }
    
    private void UIUpdate(int value = 0){
        if (UIManager.Instance == null) return;
        UIManager UI = UIManager.Instance;
        if (value > UI.loadingBar.value) UI.loadingValue = value;
        if (endGeneration) UI.LoadingScreen.SetActive(false);
        sceneMan.OpenScene();
    }
}