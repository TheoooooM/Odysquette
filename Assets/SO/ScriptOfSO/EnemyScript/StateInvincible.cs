using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "StateInvincible", menuName = "Boss/StateInvincibleSO", order = 0)]
public class StateInvincible : StateEnemySO
{
    public ExtensionMethods.PhaseBoss phaseBoss;

    public override bool CheckCondition(Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary)
    { 
        if (BossManager.instance.currentBossPhase != phaseBoss)
        {  Debug.Log("je foncitonne");
            return true;
          
        }
        else
        {
              return false;
        }
      
      
    }

    public override void StartState(Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary, out bool endStep, EnemyFeedBack enemyFeedBack)
    {
       
        
     
      endStep =  false;
    

    }

    public override void PlayState(Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary, out bool endStep, EnemyFeedBack enemyFeedBack)
    {
          BossManager.instance.SetPhase(phaseBoss);
        endStep = true;  Debug.Log("setphase"+phaseBoss);
    }
}
