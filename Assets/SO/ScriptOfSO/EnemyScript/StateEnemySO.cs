using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using Object = UnityEngine.Object;

public class StateEnemySO : ScriptableObject
{
 
    public List<ExtensionMethods.ObjectInStateManager> objectInStateManagers = new List<ExtensionMethods.ObjectInStateManager>();
    public float startTime;
    public float playStateTime;
    public bool isFixedUpdate;
    public bool haveStartState;
    private float timeForTimerState;
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
