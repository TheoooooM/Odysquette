using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "StateInvincible", menuName = "Boss/StateInvincibleSO", order = 0)]
public class StateInvincible : StateEnemySO
{
   

    public override bool CheckCondition(Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary)
    {
        if (BossManager.instance.inSetPhase || BossManager.instance.inUpdatePhase)
            return true;
        return false;


    }

    public override void StartState(Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary, out bool endStep, EnemyFeedBack enemyFeedBack)
    {
        
        if(BossManager.instance.inSetPhase)
            BossManager.instance.SetPhase(BossManager.instance.currentBossPhase);
        if (BossManager.instance.inUpdatePhase)
            BossManager.instance.UpdateDuringPhase();
     
      endStep =  false;
    

    }

    public override void PlayState(Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary, out bool endStep, EnemyFeedBack enemyFeedBack)
    {
        endStep = true;
    }
}
