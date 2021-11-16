using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomContainer : MonoBehaviour
{
    public RoomManager room;
    public Vector2 roomMapPos;


    #region Generation Variables
    
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
    
    void Awake()
    {
        exitBool = new Dictionary<Generation.open, bool>();
        exitBool.Add(Generation.open.top, exitTop);
        exitBool.Add(Generation.open.left, exitLeft);
        exitBool.Add(Generation.open.right, exitRight);
        exitBool.Add(Generation.open.bot, exitBot);

        room = GetComponentInParent<RoomManager>();
    }

   
    void Update()
    {
        
    }

    public void UpdatePart()
    {
        exitList = new Dictionary<int, Generation.open>();
        exitAmount = 0;
        
        if (partTop)
        {
           openTop.SetActive(false); 
           closeTop.SetActive(false);
           exitTop = false;
        }
        else
        {
            if (exitTop)
            {
                openTop.SetActive(true);
                closeTop.SetActive(false);
                exitList.Add(exitAmount,Generation.open.top);
                exitAmount++;
            }
            else
            {
                openTop.SetActive(false);
                closeTop.SetActive(true);
            }
        }
        if (partLeft)
        {
            openLeft.SetActive(false); 
            closeLeft.SetActive(false);
            exitLeft = false;
        }
        else
        {
            if (exitLeft)
            {
                openLeft.SetActive(true);
                closeLeft.SetActive(false);
                exitList.Add(exitAmount,Generation.open.left);
                exitAmount++;
            }
            else
            {
                openLeft.SetActive(false);
                closeLeft.SetActive(true);
            }
        }

        
        if (partRight)
        {
            openRight.SetActive(false); 
            closeRight.SetActive(false);
            exitRight = false;
        }
        else
        {
            if (exitRight)
            {
                openRight.SetActive(true);
                closeRight.SetActive(false);
                exitList.Add(exitAmount,Generation.open.right);
                exitAmount++;
            }
            else
            {
                openRight.SetActive(false);
                closeRight.SetActive(true);
            }
        }

        
        if (partBot)
        {
            openBot.SetActive(false); 
            closeBot.SetActive(false);
            exitBot = false;
        }
        else
        {
            if (exitBot)
            {
                
                openBot.SetActive(true);
                closeBot.SetActive(false);
                exitList.Add(exitAmount,Generation.open.bot);
                exitAmount++;
            }
            else
            {
                openBot.SetActive(false);
                closeBot.SetActive(true);
            }
        }
        
        

    }
}
