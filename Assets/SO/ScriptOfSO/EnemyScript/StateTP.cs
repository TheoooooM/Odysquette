using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "StateTPSO", menuName = "EnnemyState/StateTPSO", order = 0)]
public class StateTP : StateEnemySO
{
    
    public override void StartState(Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary, out bool endStep)
    {
        Transform firstTransmitter = (Transform) objectDictionary[ExtensionMethods.ObjectInStateManager.FirstTransmitter];
        Transform secondTransmitter = (Transform) objectDictionary[ExtensionMethods.ObjectInStateManager.SecondTransmitter];
        Transform enemyTransform = (Transform) objectDictionary[ExtensionMethods.ObjectInStateManager.TransformEnemy];
        if (Vector3.Distance(enemyTransform.position, firstTransmitter.position)<0.3f)
        {
            
            secondTransmitter.GetComponent<SpriteRenderer>().color = Color.blue;
        }
        else if(Vector3.Distance(enemyTransform.position, secondTransmitter.position)<0.3f)
        {
            firstTransmitter.GetComponent<SpriteRenderer>().color = Color.blue;
        }
    
    
        
        base.StartState(objectDictionary, out endStep);
    }

    public override void PlayState(Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary, out bool endStep)
    { 
        Transform firstTransmitter = (Transform) objectDictionary[ExtensionMethods.ObjectInStateManager.FirstTransmitter];
        Transform secondTransmitter = (Transform) objectDictionary[ExtensionMethods.ObjectInStateManager.SecondTransmitter];
        Transform enemyTransform = (Transform) objectDictionary[ExtensionMethods.ObjectInStateManager.TransformEnemy];
Debug.Log("bonsoir");
        if (Vector3.Distance(enemyTransform.position, firstTransmitter.position)<0.3f)
        {
            enemyTransform.position = secondTransmitter.position;
            secondTransmitter.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else if(Vector3.Distance(enemyTransform.position, secondTransmitter.position)<0.3f)
        {
            enemyTransform.position = firstTransmitter.position;
            firstTransmitter.GetComponent<SpriteRenderer>().color = Color.white;
        }
        
        endStep = true;
    }
}
