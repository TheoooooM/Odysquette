using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Events;

public class EnemyFeedBack : MonoBehaviour
{
    public FeedBackEventClassListClass[] feedBackEvent;
    private Animator animator;
    public string stateDeathName;
    private void Start()
    {
      animator =  GetComponent<Animator>();
    }

    public void LaunchAnimation(string stateName)
    {
        animator.Play(stateName);
    }
    
    public void InstantiateFX(GameObject fx)
    {
        Instantiate(fx, transform.position, Quaternion.identity);
    }
    public void PlaySound(AudioClip audioClip)
    {
        //PlaySound
    }
 [Serializable]
 public class FeedBackEventClass
 {
   
     public ExtensionMethods.EventFeedBackEnum typeFeedBackEvent;
     public UnityEvent feedBackEvent;
    
 
 }
 [Serializable]
 public class FeedBackEventClassListClass
 {
     public string stateEnemySo;
     public FeedBackEventClass[] feedBackEventClassList;
 }


}




