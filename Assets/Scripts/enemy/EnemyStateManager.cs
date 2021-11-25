
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Pathfinding;
using UnityEngine;
using Object = UnityEngine.Object;


public class EnemyStateManager : MonoBehaviour
{

    public bool isActivate = false;
    
    public EMainStatsSO EMainStatsSo;
    // Variable for Set Value and Object in States
    public Vector2 forceApply;
    public List<BaseObject> baseObjectListCondition = new List<BaseObject>();
    public List<BaseObject> baseObjectListState = new List<BaseObject>();
    private Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionaryCondition =
        new Dictionary<ExtensionMethods.ObjectInStateManager, Object>();
    private Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionaryState =
        new Dictionary<ExtensionMethods.ObjectInStateManager, Object>();
    public bool isInWind;
   public Vector2 windDirection;
    public float windSpeed;
    public float health;
    public bool isDragKnockUp;
    public bool isConvey;
    public Vector2 conveyBeltSpeed;
   
     
    //Main Stat
    [SerializeField] private Transform playerTransform;
    private bool knockUpInState = true;
 

   
    public Vector3 spawnPosition;
    //State and Stat Condition
    public int indexCurrentState;
  
   
    bool check;
    public bool IsCurrentStartPlayed;
    public bool IsCurrentStatePlayed;
    [SerializeField]
    private bool IsFirstStartPlayed;

    [HideInInspector] public RoomManager roomParent;
    //Delegate
    public delegate void CurrentState(Dictionary<ExtensionMethods.ObjectInStateManager, Object> 
        objectValue, out bool endStep);
    private CurrentState CurrentFixedState;
    private CurrentState CurrentUpdateState;
    //Timer

    public Dictionary<int, float > timerCondition = new Dictionary<int, float>();
    [SerializeField]
    private float timerCurrentStartState;
    private float timerCurrentState;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private void OnValidate()
    {
     //   spriteRenderer = GetComponent<SpriteRenderer>();
       // spriteRenderer.sprite = EMainStatsSo.sprite;
    }

    private void Start()
    { 
        
        spawnPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        health = EMainStatsSo.maxHealth;

        for (int i = 0; i < EMainStatsSo.stateEnnemList.Count; i++)
        {
            if (EMainStatsSo.stateEnnemList[i].useTimeCondition)
                        { 
                            timerCondition.Add(i, 0);
                        }
                      
        }
        

            
        
       
        for (int i = 0; i < EMainStatsSo.stateEnnemList.Count; i++)
        {
            if (EMainStatsSo.stateEnnemList[i].objectInStateManagersCondition.Count > 0)
            {
                 foreach (ExtensionMethods.ObjectInStateManager objectInStateManager in EMainStatsSo.stateEnnemList[i].objectInStateManagersCondition)
                            {  if (!objectDictionaryCondition.ContainsKey(objectInStateManager))
                                {
                                    for (int j = 0; j < baseObjectListCondition.Count; j++)
                                    {
                                        if (baseObjectListCondition[j].objectInStateManager == objectInStateManager)
                                        {
                                                                             
                                            objectDictionaryCondition.Add(objectInStateManager, baseObjectListCondition[j]._object ); 
                                        }
                                    }
                                }
                                
                             
                            }
            }
            }
GameObject player = GameObject.FindWithTag("Player");
        for (int i = 0; i < baseObjectListCondition.Count; i++)
        {
            switch (baseObjectListCondition[i].objectInStateManager)
            {
                case ExtensionMethods.ObjectInStateManager.TransformPlayer:
                {
                    baseObjectListState[i]._object = player.transform;
                    break;
                }
                case ExtensionMethods.ObjectInStateManager.RigidBodyPlayer:
                {
                    baseObjectListState[i]._object = player.GetComponent<Rigidbody2D>();
                    break;
                }
                case ExtensionMethods.ObjectInStateManager.PlayerController:
                {
                    baseObjectListState[i]._object = player.GetComponent<Playercontroller>();
                    break;
                }
            }
        }

        for (int i = 0; i < baseObjectListState.Count; i++)
        {
            switch (baseObjectListCondition[i].objectInStateManager)
            {
                case ExtensionMethods.ObjectInStateManager.TransformPlayer:
                {
                    baseObjectListState[i]._object = player.transform;
                    break;
                }
                case ExtensionMethods.ObjectInStateManager.RigidBodyPlayer:
                {
                    baseObjectListState[i]._object = player.GetComponent<Rigidbody2D>();
                    break;
                }
                case ExtensionMethods.ObjectInStateManager.PlayerController:
                {
                    baseObjectListState[i]._object = player.GetComponent<Playercontroller>();
                    break;
                }
            }
        }

 if(EMainStatsSo.baseState != null)
        UpdateDictionaries(EMainStatsSo.baseState);
           
    }

    private void Update()
    {
        if (isActivate)
        {

            #region CheckStates
            if (!IsCurrentStatePlayed && !IsCurrentStartPlayed && EMainStatsSo.stateEnnemList.Count != 0)
            {
                for (int i = 0; i < EMainStatsSo.stateEnnemList.Count; i++)
                {
                      
                    if (EMainStatsSo.stateEnnemList[i].useHealthCondition)
                    {
                          
                            
                        if (EMainStatsSo.stateEnnemList[i].healthCondition > health)
                        {
                                                           
                            continue;
                        }
                                                    
                    }
                      
                    if (EMainStatsSo.stateEnnemList[i].useTimeCondition)

                    {
                        
                           
                        if (EMainStatsSo.stateEnnemList[i].timeCondition > timerCondition[i])
                        {
                                   
                            continue;
                                
                        }
                    }
                       
                   
                    if (EMainStatsSo.stateEnnemList[i].CheckCondition(objectDictionaryCondition ))
                    {
                      
                           
                        if (!EMainStatsSo.stateEnnemList[i].isFixedUpdate)
                        {
                                
                            if (EMainStatsSo.stateEnnemList[i].haveStartState)
                            {
                                    
                                CurrentUpdateState += EMainStatsSo.stateEnnemList[i].StartState;
                                 
                                IsCurrentStartPlayed = true;
                                if(EMainStatsSo.stateEnnemList[i].oneStartState)
                                    IsFirstStartPlayed = true;
                                 
                            }
                            else
                            {
                                CurrentUpdateState += EMainStatsSo.stateEnnemList[i].PlayState;
                                IsCurrentStatePlayed = true;
                            }
                        }
                        else
                        {
                            if (EMainStatsSo.stateEnnemList[i].haveStartState)
                            {
                                CurrentFixedState += EMainStatsSo.stateEnnemList[i].StartState;
                                IsCurrentStartPlayed = true;
                                if(EMainStatsSo.stateEnnemList[i].oneStartState)
                                    IsFirstStartPlayed = true;
                                 
                            }
                            else
                            {
                                CurrentFixedState += EMainStatsSo.stateEnnemList[i].PlayState;
                                IsCurrentStatePlayed = true;
                            }

                        }
                        objectDictionaryState.Clear();
                        ;
                                  
                        knockUpInState = EMainStatsSo.stateEnnemList[i].isKnockUpInState;
                        indexCurrentState = i; 
                        UpdateDictionaries(EMainStatsSo.stateEnnemList[indexCurrentState]);
                    }
                      
                }
            }
        

            #endregion

            #region UpdateTimer

            for (int i = 0; i < EMainStatsSo.stateEnnemList.Count; i++)
            {
                if (EMainStatsSo.stateEnnemList[i].useTimeCondition)
                {
                       
                    timerCondition[i] += Time.deltaTime;
                    timerCondition[i] = Mathf.Min(timerCondition[i],EMainStatsSo.stateEnnemList[i].timeCondition);
                        
                }
                 
            }
            if(IsCurrentStatePlayed && EMainStatsSo.stateEnnemList[indexCurrentState].playStateTime != 0 ) timerCurrentState += Time.deltaTime;
            if (IsCurrentStartPlayed && EMainStatsSo.stateEnnemList[indexCurrentState].startTime != 0)
            {
          
                timerCurrentStartState += Time.deltaTime;
            }
   

            #endregion
        
            ApplyState();
        }
    }

  

    private void FixedUpdate()
    {
        if (isActivate)
        {


            ApplyState();

            if (EMainStatsSo.baseState!= null)
            {
                if (! IsCurrentStartPlayed && !IsCurrentStatePlayed )
                {
           
                    
                    EMainStatsSo.baseState.PlayState( objectDictionaryState, out bool endStep);
                    knockUpInState = EMainStatsSo.baseState.isKnockUpInState;
                   
                  
                }
                else if (((IsCurrentStartPlayed || IsCurrentStatePlayed) && EMainStatsSo.stateEnnemList[indexCurrentState].duringDefaultState ))

                {
                    UpdateDictionaries(EMainStatsSo.baseState);
                    EMainStatsSo.baseState.PlayState( objectDictionaryState, out bool endStep);
                    knockUpInState = EMainStatsSo.baseState.isKnockUpInState;
                }
         
            }
     


            if (EMainStatsSo.isKnockUp)
            {
                if (isDragKnockUp)
                {
                    rb.drag =EMainStatsSo.dragForKnockUp;
         
               
                    if (rb.velocity.magnitude <= 0.1f)
                    {
                        isDragKnockUp = false;
                        rb.drag = 0;
                        rb.velocity = Vector2.zero;
                    }
                       
               
                }
            }
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
               
                if (EMainStatsSo.stateEnnemList[indexCurrentState].isFixedUpdate)
                {
                      CurrentFixedState( objectDictionaryState,out bool endStep); _endstep = endStep;
                  
                }
                  
                else 
                {
                    Debug.Log(EMainStatsSo.stateEnnemList[indexCurrentState]);
                    CurrentUpdateState(objectDictionaryState,out bool endStep); _endstep = endStep;
                }
               
            }
         
            if (IsCurrentStartPlayed)
            { 
                if (IsFirstStartPlayed)
                              {
                                    if (EMainStatsSo.stateEnnemList[indexCurrentState].isFixedUpdate) 
                                        CurrentFixedState -= EMainStatsSo.stateEnnemList[indexCurrentState].StartState;
                                    else 
                                        CurrentUpdateState -= EMainStatsSo.stateEnnemList[indexCurrentState].StartState;
                                    IsFirstStartPlayed = false;
                                    ;
                              }
              
                
                
                if (CheckTimer(timerCurrentStartState, EMainStatsSo.stateEnnemList[indexCurrentState].startTime))
                {


                    if (EMainStatsSo.stateEnnemList[indexCurrentState].isFixedUpdate)
                    {
                        if(!EMainStatsSo.stateEnnemList[indexCurrentState].oneStartState)
                            CurrentFixedState -= EMainStatsSo.stateEnnemList[indexCurrentState].StartState;
                         CurrentFixedState += EMainStatsSo.stateEnnemList[indexCurrentState].PlayState;
                    }
                    else
                    {
                        if(!EMainStatsSo.stateEnnemList[indexCurrentState].oneStartState)
                            CurrentUpdateState -= EMainStatsSo.stateEnnemList[indexCurrentState].StartState;
                        CurrentUpdateState += EMainStatsSo.stateEnnemList[indexCurrentState].PlayState; 
                    }
                       
                    
                  
                    IsCurrentStartPlayed = false;
                    IsCurrentStatePlayed = true;
                    timerCurrentStartState = 0;
                }
            }

            else if (IsCurrentStatePlayed)
            {
               
                check = EMainStatsSo.stateEnnemList[indexCurrentState].playStateTime == 0
                    ? _endstep
                    : CheckTimer(timerCurrentState, EMainStatsSo.stateEnnemList[indexCurrentState].playStateTime);
                if (check)
                {
                    IsCurrentStatePlayed = false;

                    if (EMainStatsSo.stateEnnemList[indexCurrentState].isFixedUpdate)
                        CurrentFixedState -= EMainStatsSo.stateEnnemList[indexCurrentState].PlayState;
                    
                    

                    else 
                        CurrentUpdateState -= EMainStatsSo.stateEnnemList[indexCurrentState].PlayState;
                    if (timerCondition.ContainsKey(indexCurrentState))
                                         {
                                             timerCondition[indexCurrentState] = 0;
                                         }

                
                    objectDictionaryState.Clear();
                    if(EMainStatsSo.baseState != null)
                    UpdateDictionaries(EMainStatsSo.baseState);
                    timerCurrentState = 0;
                    timerCondition[indexCurrentState] = 0;
                    indexCurrentState = 0;
                }
            }

        }
       
    }

    void UpdateDictionaries(StateEnemySO stateEnemySo)
    
    {
        foreach (ExtensionMethods.ObjectInStateManager objectInStateManager in stateEnemySo.objectInStateManagersState)
            {  if (!objectDictionaryState.ContainsKey(objectInStateManager))
                                 {
                                     for (int i = 0; i < baseObjectListState.Count; i++)
                                     {
                                                       
                                         if (baseObjectListState[i].objectInStateManager == objectInStateManager)
                                         {
                                                             
                                             objectDictionaryState.Add(objectInStateManager, baseObjectListState[i]._object ); 
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

  public void OnDeath()
  {
      try
      {
          Destroy(gameObject.transform.parent.gameObject);
      }
      catch (Exception e)
      {
          Destroy(gameObject);
      }
      Debug.Log("Remove from List");
      roomParent.ennemiesList.Remove(gameObject);
        GameManager.Instance.ultimateValue += EMainStatsSo.giverUltimateStrawPoints;
  
      
  }

  public void TakeDamage( float damage, Vector2 position, float knockUpValue, bool knockup, bool isExplosion)
  {
      isActivate = true;
      if (health - damage <= 0)
      {
          OnDeath();
      }
      else
      {
          Knockup( position, knockUpValue, knockup, isExplosion);
          health -= damage;
      }
  }

      void Knockup(Vector2 position, float knockUpValue, bool knockUp, bool isExplosion)
      {
          if (!knockUp)
          {
              return;
          }
          if (EMainStatsSo.isKnockUp  && knockUpInState && EMainStatsSo.baseState != null && (!isDragKnockUp||isExplosion))
          {
           
              Vector2 direction =new Vector2();
        
              direction = (rb.position- position).normalized;
            
            rb.velocity  +=  direction*knockUpValue;

            
             isDragKnockUp = true;

          }
      }

      private void OnTriggerEnter2D(Collider2D other)
      {
          if(!HealthPlayer.Instance.playerController.InDash)
          if(other.gameObject.CompareTag("Player"))
          HealthPlayer.Instance.TakeDamagePlayer(1);
          if (other.CompareTag("Wind"))
          {
            
                StateWind stateWind = other.GetComponent<WindParticleManager>().StateWind;
                                       windDirection += stateWind.direction*stateWind.speedWind ;
                                     
                                       isInWind = true;     
          }

          if (other.CompareTag("Convey"))
          {
             
              conveyBeltSpeed += other.GetComponent<LDConveyorBelt>().direction;
              isConvey = true;   
          }

      }

      private void OnTriggerStay(Collider other)
      {
          if (other.CompareTag("Wind"))
          {
              isInWind = true; 
          }

          if (other.CompareTag("Convey"))
          {
              isConvey = true;
          }
      }

      private void OnTriggerExit2D(Collider2D other)
      {
          if (other.CompareTag("Convey"))
          {
              conveyBeltSpeed -= other.GetComponent<LDConveyorBelt>().direction;
              isConvey = false;
          }
          if (other.CompareTag("Wind"))
              
          {
              Debug.Log("exist");
              StateWind stateWind = other.GetComponent<WindParticleManager>().StateWind;
              windDirection -= stateWind.direction*stateWind.speedWind ;
              isInWind = false;
          
          }
         
      }
}
