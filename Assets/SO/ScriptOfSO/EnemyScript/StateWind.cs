using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
[CreateAssetMenu(fileName = "StateWindSO", menuName = "EnnemyState/StateWindSO", order = 0)]
public class StateWind : StateEnemySO
{
   
    public float speedWind;
    public Vector2 direction;
    //public float height;
    //public float height;

        
    public override void PlayState(Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary, out bool endStep)
    {
        WindParticleManager windParticleManager = (WindParticleManager) objectDictionary[ExtensionMethods.ObjectInStateManager.WindParticleManager];
        windParticleManager.enabled = true;
        endStep = false;
    }
}
