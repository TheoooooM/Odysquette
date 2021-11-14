using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomContainer : MonoBehaviour
{
    
    
     public bool partTop = false;
     public bool partLeft = false;
     public bool partRight = false;
     public bool partBot = false;
    
    [Header("=======================DEBUG=========================")]
    public bool exitTop = true;
    public bool exitLeft = true;
    public bool exitRight = true;
    public bool exitBot = true;
    
    [Header("=======================PAS TOUCHE !!!=========================")]
    [SerializeField] private GameObject closeBot;
    [SerializeField] private GameObject closeLeft;
    [SerializeField] private GameObject closeRight;
    [SerializeField] private GameObject closeTop;
    [SerializeField] private GameObject openBot;
    [SerializeField] private GameObject openLeft;
    [SerializeField] private GameObject openRight;
    [SerializeField] private GameObject openTop;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePart()
    {
        
        if (partTop)
        {
           openTop.SetActive(false); 
           closeTop.SetActive(false);
        }
        else
        {
            if (exitTop)
            {
                openTop.SetActive(true);
                closeTop.SetActive(false);
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
        }
        else
        {
            if (exitLeft)
            {
                openLeft.SetActive(true);
                closeLeft.SetActive(false);
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
        }
        else
        {
            if (exitRight)
            {
                openRight.SetActive(true);
                closeRight.SetActive(false);
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
        }
        else
        {
            if (exitBot)
            {
                
                openBot.SetActive(true);
                closeBot.SetActive(false);
            }
            else
            {
                openBot.SetActive(false);
                closeBot.SetActive(true);
            }
        }
        
        

    }
}
