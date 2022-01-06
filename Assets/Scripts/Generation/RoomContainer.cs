using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Pathfinding;
using UnityEngine;

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
    [SerializeField] public GameObject closeBot;

    [SerializeField] public GameObject closeLeft;
    [SerializeField] public GameObject closeRight;
    [SerializeField] public GameObject closeTop;
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

        if (partTop) {
            openTop.SetActive(false);
            closeTop.SetActive(false);
            exitTop = false;
        }
        else {
            if (exitTop) {
                openTop.SetActive(true);
                closeTop.SetActive(false);
                exitList.Add(exitAmount, Generation.open.top);
                exitAmount++;
            }
            else {
                openTop.SetActive(false);
                closeTop.SetActive(false);
            }
        }

        if (partLeft) {
            openLeft.SetActive(false);
            closeLeft.SetActive(false);
            exitLeft = false;
        }
        else {
            if (exitLeft) {
                openLeft.SetActive(true);
                closeLeft.SetActive(false);
                exitList.Add(exitAmount, Generation.open.left);
                exitAmount++;
            }
            else {
                openLeft.SetActive(false);
                closeLeft.SetActive(false);
            }
        }


        if (partRight) {
            openRight.SetActive(false);
            closeRight.SetActive(false);
            exitRight = false;
        }
        else {
            if (exitRight) {
                openRight.SetActive(true);
                closeRight.SetActive(false);
                exitList.Add(exitAmount, Generation.open.right);
                exitAmount++;
            }
            else {
                openRight.SetActive(false);
                closeRight.SetActive(false);
            }
        }


        if (partBot) {
            openBot.SetActive(false);
            closeBot.SetActive(false);
            exitBot = false;
        }
        else {
            if (exitBot) {
                openBot.SetActive(true);
                closeBot.SetActive(false);
                exitList.Add(exitAmount, Generation.open.bot);
                exitAmount++;
            }
            else {
                openBot.SetActive(false);
                closeBot.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Activate neighboors base on the position of the player
    /// </summary>
    /// <param name="active"></param>
    public void ActivateNeighbor(bool active) {
        if (Generator == null) return;
        
        if (!Generator.disableNeighboor) return;
        
        if (Generator.map[(int) roomMapPos.x - 1, (int) roomMapPos.y] != null) Generator.map[(int) roomMapPos.x - 1, (int) roomMapPos.y].neighbor = active;
        if (Generator.map[(int) roomMapPos.x + 1, (int) roomMapPos.y] != null) Generator.map[(int) roomMapPos.x + 1, (int) roomMapPos.y].neighbor = active;
        if (Generator.map[(int) roomMapPos.x, (int) roomMapPos.y - 1] != null) Generator.map[(int) roomMapPos.x, (int) roomMapPos.y - 1].neighbor = active;
        if (Generator.map[(int) roomMapPos.x, (int) roomMapPos.y + 1] != null) Generator.map[(int) roomMapPos.x, (int) roomMapPos.y + 1].neighbor = active;


        if (active) {
            if (Generator.map[(int) roomMapPos.x - 1, (int) roomMapPos.y] != null) Generator.map[(int) roomMapPos.x - 1, (int) roomMapPos.y].gameObject.SetActive(true);
            if (Generator.map[(int) roomMapPos.x + 1, (int) roomMapPos.y] != null) Generator.map[(int) roomMapPos.x + 1, (int) roomMapPos.y].gameObject.SetActive(true);
            if (Generator.map[(int) roomMapPos.x, (int) roomMapPos.y - 1] != null) Generator.map[(int) roomMapPos.x, (int) roomMapPos.y - 1].gameObject.SetActive(true);
            if (Generator.map[(int) roomMapPos.x, (int) roomMapPos.y + 1] != null) Generator.map[(int) roomMapPos.x, (int) roomMapPos.y + 1].gameObject.SetActive(true);
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