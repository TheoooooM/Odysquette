using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;

[CreateAssetMenu(fileName = "StateDashSO", menuName = "EnnemyState/StateDashSO", order = 0)]
public class StateDash : StateEnemySO
{
    private float range;
    private float dashspeed;
    private int beginAccelerationPercentage; 
    private int endAccelerationPercentage;
    private float dashDistance;
    private Vector3 beginDashPosition;
    public bool CheckCondition(Transform playerTransform, NavMeshAgent meshAgent, Vector3 spawnPosition)
    {
        return true;
    }
    public override void StartState(Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary, out bool endStep)
    {
        endStep = false;
    }
    public override void PlayState( Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary, out bool endStep)
    {
        endStep = false;
    }


}
