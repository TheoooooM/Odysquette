
using System;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using Object = UnityEngine.Object;


public class EnemyStateManager : MonoBehaviour
{
    // Variable for Set Value and Object in States
 
    public List<BaseObject> baseObjectList = new List<BaseObject>();
    private Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary =
        new Dictionary<ExtensionMethods.ObjectInStateManager, Object>(); 
    
    //Main Stat
    [SerializeField] private Transform playerTransform;
    
    public int  health;
    

    public Vector3 spawnPosition;
    //State and Stat Condition
    public List<StateEnemySO> stateEnnemList = new List<StateEnemySO>();
    private int indexCurrentState;
   public StateEnemySO defaultState;
    public List<int> healthCondition = new List<int>();
    bool check;
    public bool IsCurrentStartPlayed;
    public bool IsCurrentStatePlayed;
    private bool IsFirstStartPlayed;
    
    //Delegate
    public delegate void CurrentState(Dictionary<ExtensionMethods.ObjectInStateManager, Object> 
        objectValue, out bool endStep);
    private CurrentState CurrentFixedState;
    private CurrentState CurrentUpdateState;
    //Timer
    public List<float> timeCondition = new List<float>();
    public List<float> timerCondition = new List<float>();
    private float timerCurrentStartState;
    private float timerCurrentState;
  
    

    private void Start()
    {
        spawnPosition = transform.position;
    }

    private void Update()
    {
        #region CheckStates
  if (!IsCurrentStatePlayed && !IsCurrentStartPlayed)
        {
                for (int i = 0; i < stateEnnemList.Count; i++)
                    {
                       
                        if (healthCondition.Count != 0)
                        {
                             if (healthCondition[i] != null)
                                                    {
                                                        if (healthCondition[i] > health)
                                                        {
                                                            Debug.Log(i);
                                                            continue;
                                                        }
                                                    }
                        }
                      
                        if (timeCondition.Count != 0 && timerCondition.Count != 0)

                        {
                            if (timeCondition[i] != null)
                            {
                                if (timeCondition[i] > timerCondition[i])
                                {
                                    
                                    continue;
                                }
                            }
                        }
                       
                        UpdateDictionaries(stateEnnemList[i]);
                        if (stateEnnemList[i].CheckCondition(objectDictionary ))
                        {
                           
                           
                            if (!stateEnnemList[i].isFixedUpdate)
                            {
                                
                                if (stateEnnemList[i].haveStartState)
                                {
                                    CurrentUpdateState += stateEnnemList[i].StartState;
                                    IsCurrentStartPlayed = true;
                                    IsFirstStartPlayed = true;
                                }
                                else
                                {
                                    CurrentUpdateState += stateEnnemList[i].PlayState;
                                    IsCurrentStatePlayed = true;
                                }
                            }
                            else
                            {
                                if (stateEnnemList[i].haveStartState)
                                {
                                    CurrentFixedState += stateEnnemList[i].StartState;
                                    IsCurrentStartPlayed = true;
                                    IsFirstStartPlayed = true;
                                }
                                else
                                {
                                    CurrentFixedState += stateEnnemList[i].PlayState;
                                    IsCurrentStatePlayed = true;
                                }

                            }
                            indexCurrentState = i;
                        }
                        else
                        {
                            objectDictionary.Clear();
                            
                        }
                    }
        }
        

        #endregion

        #region UpdateTimer

          for (int i = 0; i < timerCondition.Count; i++)
                {
                    timerCondition[i] += Time.deltaTime;
                    timerCondition[i] = Mathf.Min(timerCondition[i],timeCondition[i]);
                }
          if(IsCurrentStatePlayed && stateEnnemList[indexCurrentState].startTime != 0 ) timerCurrentState += Time.deltaTime;
          if(IsCurrentStartPlayed && stateEnnemList[indexCurrentState].playStateTime != 0) timerCurrentStartState += Time.deltaTime;
   

        #endregion
        
        ApplyState();
    }
    private void FixedUpdate()
    {     
       ApplyState();

       if (!IsCurrentStatePlayed && !IsCurrentStartPlayed)
       {
           UpdateDictionaries(defaultState);
           defaultState.PlayState( objectDictionary, out bool endStep);
           objectDictionary.Clear();
       
       }
    }

    bool CheckTimer(float timer, float time)
    {
        if (timer >= time)
            return true;
        return false;
    }

    void ApplyState()
    {
       

        if (IsCurrentStartPlayed 
            || IsCurrentStatePlayed )
        {
            bool _endstep = false;
            if (CurrentUpdateState != null || CurrentFixedState != null)
            {
               
                if (stateEnnemList[indexCurrentState].isFixedUpdate)
                {
                      CurrentFixedState( objectDictionary,out bool endStep); _endstep = endStep;
                      //Debug.Log(_endstep);
                }
                  
                else 
                {
                    CurrentUpdateState( objectDictionary,out bool endStep); _endstep = endStep;
                }
               
            }
         
            if (IsCurrentStartPlayed)
            { 
                if (IsFirstStartPlayed)
                              {
                                    if (stateEnnemList[indexCurrentState].isFixedUpdate) 
                                                      CurrentFixedState -= stateEnnemList[indexCurrentState].StartState;
                                                  else 
                                                      CurrentUpdateState -= stateEnnemList[indexCurrentState].StartState;
                                    IsFirstStartPlayed = false;
                              }
              
                
                
                if (CheckTimer(timerCurrentStartState, stateEnnemList[indexCurrentState].startTime))
                {  objectDictionary.Clear();
                   
                    UpdateDictionaries(stateEnnemList[indexCurrentState]);
                    if (stateEnnemList[indexCurrentState].isFixedUpdate) 
                        CurrentFixedState += stateEnnemList[indexCurrentState].PlayState;
                    
                  
                    else 
                        CurrentUpdateState += stateEnnemList[indexCurrentState].PlayState;
                    
                  
                    IsCurrentStartPlayed = false;
                    IsCurrentStatePlayed = true;
                    timerCurrentStartState = 0;
                }
            }

            else if (IsCurrentStatePlayed)
            {
                check = stateEnnemList[indexCurrentState].playStateTime == 0
                    ? _endstep
                    : CheckTimer(timerCurrentState, stateEnnemList[indexCurrentState].playStateTime);
                if (check)
                {
                    IsCurrentStatePlayed = false;

                    if (stateEnnemList[indexCurrentState].isFixedUpdate)
                        CurrentFixedState -= stateEnnemList[indexCurrentState].PlayState;
                    
                    

                    else 
                        CurrentUpdateState -= stateEnnemList[indexCurrentState].PlayState;
                    if (timerCondition[indexCurrentState] != null)
                    {
                        timerCondition[indexCurrentState] = 0;
                    }

                
                    objectDictionary.Clear();
                  Debug.Log("aa");
                    timerCurrentState = 0;
                    timerCondition[indexCurrentState] = 0;
                    indexCurrentState = 0;
                }
            }

        }
       
    }

    void UpdateDictionaries(StateEnemySO stateEnemySo)
    {
        foreach (ExtensionMethods.ObjectInStateManager objectInStateManager in stateEnemySo.objectInStateManagers)
            {  if (!objectDictionary.ContainsKey(objectInStateManager))
                                 {
                                     for (int i = 0; i < baseObjectList.Count; i++)
                                     {
                                                       
                                         if (baseObjectList[i].objectInStateManager == objectInStateManager)
                                         {
                                                             
                                             objectDictionary.Add(objectInStateManager, baseObjectList[i]._object ); 
                                         }
                                     }
                                 }
                
             
            }

    }
[Serializable]
  public class BaseObject
  {
      public ExtensionMethods.ObjectInStateManager objectInStateManager;
      public Object _object;

  }
  
  

 
}
