using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretStateManager : EnemyStateManager
{
 public BoxCollider2D boxCollider2D;
 public Color FxColor;
 private  EnemyFeedBack enemyFeedBack;
 private void Awake()
 {
  boxCollider2D = GetComponent<BoxCollider2D>();
 }

 public override void Start()
 {  enemyFeedBack = GetComponent<EnemyFeedBack>();
  spriteRenderer = GetComponent<SpriteRenderer>();

  spawnPosition = transform.position;
  rb = GetComponent<Rigidbody2D>();
  for (int i = 0; i < EMainStatsSo.stateEnnemList.Count; i++)
  {
   if (EMainStatsSo.stateEnnemList[i].useTimeCondition && i != 0)
   {
    Debug.Log("test");
    Debug.Log(gameObject.name + timerCondition.Count);
    timerCondition.Add(i, 0);
   }
   if (EMainStatsSo.stateEnnemList[i].useHealthCondition)
   {
    healthUse.Add(i, false);
   }
  }
  for (int i = 0; i < EMainStatsSo.stateEnnemList.Count; i++)
  {
   if (EMainStatsSo.stateEnnemList[i].objectInStateManagersCondition.Count > 0)
   {
    foreach (ExtensionMethods.ObjectInStateManager objectInStateManager in EMainStatsSo.stateEnnemList[i].objectInStateManagersCondition)
    {  if (!objectDictionaryCondition.ContainsKey(objectInStateManager))
     {
      for (int j = 0; j < baseObjectListCondition.Count; j++)
      {
       if (baseObjectListCondition[j].objectInStateManager == objectInStateManager)
       {
                                                                             
        objectDictionaryCondition.Add(objectInStateManager, baseObjectListCondition[j]._object ); 
       }
      }
     }
                                
                             
    }
   }
  }

  if(EMainStatsSo.baseState != null)
   UpdateDictionaries(EMainStatsSo.baseState);

  
 }

private void OnEnable()
{
 health = EMainStatsSo.maxHealth;
 

}

public override void OnDeath()
{
 spriteRenderer.color = FxColor;
  BossManager.instance.UpdateDuringPhase();
  GameManager.Instance.ultimateValue += EMainStatsSo.giverUltimateStrawPoints;
  GetComponent<Animator>().Play(enemyFeedBack.stateDeathName);
  
 }
}
