using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Pathfinding;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class RoomContainer : MonoBehaviour {
    public Generation Generator;
    public RoomManager room;
    public ABPath nasvMesh;
    public Vector2 roomMapPos;
    public bool neighbor = false;
    private bool playerIn = false;
    [HideInInspector] public bool firstRoom = false;

    [Space]
    [SerializeField] private bool outsideRoom = false;
    public bool OutsideRoom => outsideRoom;

    public RoomContainer neighboorContainer = null;
    
    #region Generation Variables

    public bool playerInRoom;

    public RoomCreator roomRef;
    public Vector2 roomPos;

    public RoomContainer partTop;
    public RoomContainer partLeft;
    public RoomContainer partRight;
    public RoomContainer partBot;

    [Header("=======================DEBUG=========================")]
    public bool exitTop = true;
    public bool exitLeft = true;
    public bool exitRight = true;
    public bool exitBot = true;

    public Dictionary<int, Generation.open> exitList;
    public Dictionary<Generation.open, bool> exitBool;
    [HideInInspector] public int exitAmount = 0;


    [Header("=======================PAS TOUCHE !!!=========================")]
    [SerializeField] public GameObject doorBot;
    [SerializeField] public GameObject doorLeft;
    [SerializeField] public GameObject doorRight;
    [SerializeField] public GameObject doorTop;
    [Space]
    [SerializeField] public GameObject lightBot;
    [SerializeField] public GameObject lightLeft;
    [SerializeField] public GameObject lightRight;
    [SerializeField] public GameObject lightTop;
    [Space]
    [SerializeField] private GameObject openBot;
    [SerializeField] private GameObject openLeft;
    [SerializeField] private GameObject openRight;
    [SerializeField] private GameObject openTop;

    #endregion

    [Space] public CloseRoomManager closeRoom = null;
    private bool hasDisable = false;
    
    private void Awake() {
        exitBool = new Dictionary<Generation.open, bool>();
        exitBool.Add(Generation.open.top, exitTop);
        exitBool.Add(Generation.open.left, exitLeft);
        exitBool.Add(Generation.open.right, exitRight);
        exitBool.Add(Generation.open.bot, exitBot);

        room = GetComponentInParent<RoomManager>();
    }

    private void Update() {
        if (!outsideRoom) {
            if (!neighbor && !room.runningRoom && Generator.endGeneration && !playerIn && Generator.disableNeighboor) gameObject.SetActive(false);
            if (firstRoom && Generator.endGeneration) {
                ActivateNeighbor(true);
                firstRoom = false;
            }
        }
        else if(Generator.disableNeighboor && !hasDisable){
            gameObject.SetActive(false);
            hasDisable = true;
        }
    }

    /// <summary>
    /// Update door collision and trigger base on the room layout
    /// </summary>
    public void UpdatePart() {
        exitList = new Dictionary<int, Generation.open>();
        exitAmount = 0;

        //TOP
        if (partTop) {
            openTop.SetActive(false);
            doorTop.SetActive(false);
            exitTop = false;
        }
        else { 
            if (exitTop) {
                openTop.SetActive(true);
                doorTop.SetActive(true);
                lightTop.SetActive(true);
                exitList.Add(exitAmount, Generation.open.top);
                exitAmount++;
            }
            else {
                openTop.SetActive(false);
                doorTop.SetActive(false);
            }
        }

        //LEFT
        if (partLeft) {
            openLeft.SetActive(false);
            doorLeft.SetActive(false);
            exitLeft = false;
        }
        else {
            if (exitLeft) {
                openLeft.SetActive(true);
                doorLeft.SetActive(true);
                lightLeft.SetActive(true);
                exitList.Add(exitAmount, Generation.open.left);
                exitAmount++;
            }
            else {
                openLeft.SetActive(false);
                doorLeft.SetActive(false);
            }
        }

        //RIGHT
        if (partRight) {
            openRight.SetActive(false);
            doorRight.SetActive(false);
            exitRight = false;
        }
        else {
            if (exitRight) {
                openRight.SetActive(true);
                doorRight.SetActive(true);
                lightRight.SetActive(true);
                exitList.Add(exitAmount, Generation.open.right);
                exitAmount++;
            }
            else {
                openRight.SetActive(false);
                doorRight.SetActive(false);
            }
        }

        //BOT
        if (partBot) {
            openBot.SetActive(false);
            doorBot.SetActive(false);
            exitBot = false;
        }
        else {
            if (exitBot) {
                openBot.SetActive(true);
                doorBot.SetActive(true);
                lightBot.SetActive(true);
                exitList.Add(exitAmount, Generation.open.bot);
                exitAmount++;
            }
            else {
                openBot.SetActive(false);
                doorBot.SetActive(false);
            }
        }
    }
    
    
    /// <summary>
    /// Activate neighboors base on the position of the player
    /// </summary>
    /// <param name="active"></param>
    public void ActivateNeighbor(bool active){
        if (Generator == null) return;
        if (!Generator.disableNeighboor) return;

        Vector2Int[] posName = new Vector2Int[] {
            new Vector2Int(-1, 0),
            new Vector2Int(1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(0, 1),
            new Vector2Int(-1, 1),
            new Vector2Int(-1, -1),
            new Vector2Int(1, 1),
            new Vector2Int(1, -1)
        };

        foreach (Vector2Int pos in posName) {
            RoomContainer neighRoom = Generator.map[(int) roomMapPos.x + pos.x, (int) roomMapPos.y + pos.y];
            if (neighRoom != null) neighRoom.neighbor = active;
            if (active && neighRoom != null) neighRoom.gameObject.SetActive(true);
            
            if(neighRoom.OutsideRoom && neighRoom.neighboorContainer != null && neighRoom.neighboorContainer == this && !active) neighRoom.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        //Debug.Log("enter collider with " + other.name);
        if (other.CompareTag("Player")) {
            playerIn = true;
            neighbor = true;
            playerInRoom = true;

            ActivateNeighbor(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            playerIn = false;
            playerInRoom = false;

            ActivateNeighbor(false);
        }
    }
}