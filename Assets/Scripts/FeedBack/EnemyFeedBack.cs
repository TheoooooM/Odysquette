using System;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(EnemyStateManager))]
public class EnemyFeedBack : MonoBehaviour
{

    public FeedBackEventClassListClass[] feedBackEvent;
    private Animator animator;
    public string stateDeathName;
    public string[] animationList;
    

    public void LaunchAnimationWithVariable(int index)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animationList[index]))
            return;
        animator.Play(animationList[index]);
    }


    private void Start()
    {
      animator =  GetComponent<Animator>();
    }

    public void LaunchAnimation(string stateName)
    {
        Debug.Log(stateName);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
            return;
  
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




