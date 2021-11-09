using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateFlash : StateEnemySO
{
    public float range;


    public override void StartState(Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary, out bool endStep)
    {
        base.StartState(objectDictionary, out endStep);
    }

    public override void PlayState(Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary, out bool endStep)
    {
        base.PlayState(objectDictionary, out endStep);
    }
}
