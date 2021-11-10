using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using Object = UnityEngine.Object;

public class StateEnemySO : ScriptableObject
{
    public bool isKnockUpInState =true;
    public List<ExtensionMethods.ObjectInStateManager> objectInStateManagersCondition = new List<ExtensionMethods.ObjectInStateManager>();
    public List<ExtensionMethods.ObjectInStateManager> objectInStateManagersState = new List<ExtensionMethods.ObjectInStateManager>();
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

    public virtual void StartState( Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary,  out bool endStep)
    {
        endStep = false;
    }
        
    public virtual void PlayState(Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary, out bool endStep)
    {
        endStep = false;
    }
}
