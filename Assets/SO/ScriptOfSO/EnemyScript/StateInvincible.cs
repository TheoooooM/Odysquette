using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "StateInvincible", menuName = "Boss/StateInvincibleSO", order = 0)]
public class StateInvincible : StateEnemySO
{
    public ExtensionMethods.PhaseBoss phaseBoss;
    
    public override void StartState(Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary, out bool endStep)
    {
    Debug.Log("try");
        BossManager.instance.SetPhase(phaseBoss);
       
            endStep = true;
            
            
        
            
    }

    public override void PlayState(Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary, out bool endStep)
    {
        endStep = true;
    }
}
