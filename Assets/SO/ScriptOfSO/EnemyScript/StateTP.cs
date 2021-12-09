using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "StateTPSO", menuName = "EnnemyState/StateTPSO", order = 0)]
public class StateTP : StateEnemySO
{
    
    public override void StartState(Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary, out bool endStep, EnemyFeedBack enemyFeedBack)
    {
        Transform firstTransmitter = (Transform) objectDictionary[ExtensionMethods.ObjectInStateManager.FirstTransmitter];
        Transform secondTransmitter = (Transform) objectDictionary[ExtensionMethods.ObjectInStateManager.SecondTransmitter];
        Transform enemyTransform = (Transform) objectDictionary[ExtensionMethods.ObjectInStateManager.TransformEnemy];
  
        
        if (Vector3.Distance(enemyTransform.position, firstTransmitter.position)<1f)
        {
            Debug.Log("startTP second");
            CheckFeedBackEvent(enemyFeedBack, ExtensionMethods.EventFeedBackEnum.StartSecondPosition);
           
        }
        else if(Vector3.Distance(enemyTransform.position, secondTransmitter.position)<1f)
        {
            Debug.Log("startTP first");
            CheckFeedBackEvent(enemyFeedBack, ExtensionMethods.EventFeedBackEnum.StartFirstPosition);
          
        }
    
    
        
        base.StartState(objectDictionary, out endStep, enemyFeedBack);
    }

    public override void PlayState(Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary, out bool endStep, EnemyFeedBack enemyFeedBack)
    { 
        Transform firstTransmitter = (Transform) objectDictionary[ExtensionMethods.ObjectInStateManager.FirstTransmitter];
        Transform secondTransmitter = (Transform) objectDictionary[ExtensionMethods.ObjectInStateManager.SecondTransmitter];
        Transform enemyTransform = (Transform) objectDictionary[ExtensionMethods.ObjectInStateManager.TransformEnemy];

        if (Vector3.Distance(enemyTransform.position, firstTransmitter.position)<1f)
        {
            CheckFeedBackEvent(enemyFeedBack, ExtensionMethods.EventFeedBackEnum.SecondPosition);
            enemyTransform.position = secondTransmitter.position;
            Debug.Log("TP second");
            UnityEvent u;
            
        }
        else if(Vector3.Distance(enemyTransform.position, secondTransmitter.position)<1f)
        {
            CheckFeedBackEvent(enemyFeedBack, ExtensionMethods.EventFeedBackEnum.FirstPosition);
            enemyTransform.position = firstTransmitter.position;
            Debug.Log("sTP first");
        }
        
        endStep = true;
    }
}
