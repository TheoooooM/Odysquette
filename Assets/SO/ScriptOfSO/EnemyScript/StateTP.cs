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
        if (enemyTransform.position == firstTransmitter.position)
        {
            secondTransmitter.GetComponent<SpriteRenderer>().color = Color.blue;
        }
        else
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

        if (enemyTransform.position == firstTransmitter.position)
        {
            enemyTransform.position = secondTransmitter.position;
            secondTransmitter.GetComponent<SpriteRenderer>().color = Color.cyan;
        }
        else
        {
            enemyTransform.position = firstTransmitter.position;
            firstTransmitter.GetComponent<SpriteRenderer>().color = Color.cyan;
        }
        endStep = true;
    }
}
