using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using UnityEditor.Timeline.Actions;
using Object = UnityEngine.Object;

public class StateEnemySO : ScriptableObject
{
    public ExtensionMethods.EventFeedBackEnum[] eventFeedBackInState = new ExtensionMethods.EventFeedBackEnum[2]; 
    public bool needEnemyMovement;
    public bool openBasePanel;
    public bool openDebugPanel;
    public bool openSpecPanel;
    public float timeCondition;
    public bool editFeedBackList;
    public float healthCondition;
    public bool openKnobDebugPanel;
    public bool useHealthCondition;
    public bool useTimeCondition;
    public bool isKnockUpInState =true;
    //debug
    public List<ExtensionMethods.ObjectInStateManager> objectInStateManagersCondition = new List<ExtensionMethods.ObjectInStateManager>();
    public List<ExtensionMethods.ObjectInStateManager> objectInStateManagersState = new List<ExtensionMethods.ObjectInStateManager>();
    //1
    //fixed state during default state
    //start parameter 
    //play state
    public bool oneStartState;
    public float startTime;
    public float playStateTime;
    public bool isFixedUpdate;
    public bool haveStartState;
    private float timeForTimerState;
    public bool duringDefaultState;
    public virtual bool CheckCondition (Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary)
    {
      
        return true;
    }

    public virtual void StartState( Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary,  out bool endStep, EnemyFeedBack enemyFeedBack)
    {
        
        endStep = false;
    }
        
    public virtual void PlayState(Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary, out bool endStep, EnemyFeedBack enemyFeedBack)
    {
        endStep = false;
    }

    public void CheckFeedBackEvent(EnemyFeedBack enemyFeedBack, ExtensionMethods.EventFeedBackEnum feedBackEnum)
    {
        if (enemyFeedBack != null)
        {
             EnemyFeedBack.FeedBackEventClassListClass[] feedBackEventClassListClass = enemyFeedBack.feedBackEvent;
                    for (int i = 0; i < feedBackEventClassListClass.Length; i++)
                    {
                        if (feedBackEventClassListClass[i].stateEnemySo == this.name)
                        {
                            EnemyFeedBack.FeedBackEventClass[] feedBackEventClass = feedBackEventClassListClass[i].feedBackEventClassList;
                            for (int j = 0; j < feedBackEventClass.Length; j++)
                            {
                                if(feedBackEventClass[j].typeFeedBackEvent == feedBackEnum)
                                 
                                    feedBackEventClass[j].feedBackEvent.Invoke();
                            }
                        }
                    }
        }

        }

       
    



        

 

   
}
