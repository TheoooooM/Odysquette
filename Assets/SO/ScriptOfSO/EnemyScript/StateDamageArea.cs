using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
[CreateAssetMenu(fileName = "StateDamageArea", menuName = "Boss/StateDamageArea", order = 0)]
public class StateDamageArea : StateEnemySO
{
    public GameObject prefab;

    public IntervalDistance[] intervalDistancesList;
    public DelayAtSpecificNumberShoot[] delayList;
    public int numberShoot;
    public int totalProbability;
 
    public override void StartState(Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary, out bool endStep, EnemyFeedBack enemyFeedBack)
    {
        BossManager.instance.prepareShootAnimation = true;
        if(BossManager.instance.shootAnimationTime == 0)
        BossManager.instance.shootAnimationTime = startTime;
        if (!BossManager.instance.inShootAnimation)
        {
            CheckFeedBackEvent(enemyFeedBack, ExtensionMethods.EventFeedBackEnum.DuringStartState);
            endStep = false;
            return;
        }
        
        Transform transformPlayer = (Transform) objectDictionary[ExtensionMethods.ObjectInStateManager.TransformPlayer];
       EnemyStateManager enemyStateManager = (EnemyStateManager) objectDictionary[ExtensionMethods.ObjectInStateManager.EnemyStateManager];
       enemyStateManager.StartCoroutine(DelayShoot(transformPlayer.position));
     
        // dure une frame pour launche animation et faire tout le bordel
        endStep = true;  
    }

    public override void PlayState(Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary, out bool endStep, EnemyFeedBack enemyFeedBack)
    {  BossManager.instance.inShootAnimation = false;
        CheckFeedBackEvent(enemyFeedBack, ExtensionMethods.EventFeedBackEnum.DuringPlayState);
      endStep = false;
    }

    IEnumerator DelayShoot(Vector3 playerPosition)
    {
        List<Vector3> previousPosition = new List<Vector3>();
        

        for (int i = 0; i < numberShoot; i++)
        {  if (delayList.Length != 0)
                     {
                         for (int j = 0; j < delayList.Length; j++)
                         {
                             if (i+1 == delayList[j].numberShoot)
                             {
                                 yield return new WaitForSeconds(delayList[j].delayShoot); 
                                 break;
                             }
                         }
                     }
            bool isWalkable =false;
            Vector3 finalPosition = new Vector3();
            int test = 0;
            while (!isWalkable)
            {
                test++;
                if (test >= 50) 
                Debug.LogError("Don't find correct repartition of area");
                int intervalRand = Random.Range(0, totalProbability+1);
                
                            int index = 0;
                            int test2 = 0;
                            while (intervalRand > 0)
                            {
                               
                                intervalRand -= intervalDistancesList[index].probability;
                                index++;
                                test2++;
                                if(test2 >= 20)
                                    Debug.LogError("Don't find correct repartition of area");
                                
                                if (index == intervalDistancesList.Length)
                                {
                                    index--;
                                    break;
                                }
                                
                            }
                            float finalDistance = Random.Range(intervalDistancesList[index].minDistance,
                                intervalDistancesList[index].maxDistance);
                            Vector2 positionNormalized = Random.insideUnitCircle;
                    finalPosition = playerPosition+((Vector3)positionNormalized)*finalDistance ;


                    if (AstarPath.active.GetNearest(finalPosition).node.Walkable)
                    {
                     
                            isWalkable = true;
                        
                            
                        
                    }
            }
            
            
            
            EnemySpawnerManager.Instance.SpawnBossShootPool(finalPosition);
          

        }
      
    }
[Serializable]
  public class IntervalDistance
    {
    public float minDistance;
    public float maxDistance;
    public int probability;
    }

  [Serializable]
  public class DelayAtSpecificNumberShoot
  {
      public int numberShoot;
      public float delayShoot;
  }
}
