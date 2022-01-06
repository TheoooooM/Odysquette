using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretStateManager : EnemyStateManager
{
 public BoxCollider2D boxCollider2D;
 public Color FxColor;

 private void Awake()
 {
  boxCollider2D = GetComponent<BoxCollider2D>();
 }

 public override void Start()
 {
 
  enemyFeedBack = GetComponent<EnemyFeedBack>();
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


  }

  for (int i = 0; i < baseObjectListCondition.Count; i++)
  {
   switch (baseObjectListCondition[i].objectInStateManager)
   {
    case ExtensionMethods.ObjectInStateManager.TransformPlayer:
    {
     baseObjectListCondition[i]._object = HealthPlayer.Instance.transform;
     break;
    }
    case ExtensionMethods.ObjectInStateManager.RigidBodyPlayer:
    {
     baseObjectListCondition[i]._object = HealthPlayer.Instance.rb;

     break;
    }
    case ExtensionMethods.ObjectInStateManager.PlayerController:
    {
     baseObjectListState[i]._object = HealthPlayer.Instance.playerController;
     break;
    }
   }
  }

  for (int i = 0; i < EMainStatsSo.stateEnnemList.Count; i++)
  {
   if (EMainStatsSo.stateEnnemList[i].objectInStateManagersCondition.Count > 0)
   {
    foreach (ExtensionMethods.ObjectInStateManager objectInStateManager in EMainStatsSo.stateEnnemList[i]
     .objectInStateManagersCondition)
    {
     if (!objectDictionaryCondition.ContainsKey(objectInStateManager))
     {
      for (int j = 0; j < baseObjectListCondition.Count; j++)
      {
       if (baseObjectListCondition[j].objectInStateManager == objectInStateManager)
       {

        objectDictionaryCondition.Add(objectInStateManager, baseObjectListCondition[j]._object);
       }
      }
     }


    }
   }
  }

  for (int i = 0; i < baseObjectListState.Count; i++)
  {
   switch (baseObjectListState[i].objectInStateManager)
   {
    case ExtensionMethods.ObjectInStateManager.TransformPlayer:
    {
     baseObjectListState[i]._object = HealthPlayer.Instance.transform;
     break;
    }
    case ExtensionMethods.ObjectInStateManager.RigidBodyPlayer:
    {
     baseObjectListState[i]._object = HealthPlayer.Instance.rb;
     break;
    }
    case ExtensionMethods.ObjectInStateManager.PlayerController:
    {
     baseObjectListState[i]._object = HealthPlayer.Instance.playerController;
     break;
    }
   }
  }


  if (EMainStatsSo.baseState != null)
   UpdateDictionaries(EMainStatsSo.baseState);


 }

 private void OnEnable()
 {
  health = EMainStatsSo.maxHealth;

  isDead = false;
 }

 public override void OnDeath(bool byFall = false)
 {
  if (!isDead)
  {
   spriteRenderer.color = FxColor;
   BossManager.instance.inUpdatePhase = true;

//  GetComponent<Animator>().Play(enemyFeedBack.stateDeathName);
   isDead = true;
   if (IsCurrentStartPlayed)
   {

    if (EMainStatsSo.stateEnnemList[indexCurrentState].isFixedUpdate)
     CurrentFixedState -= EMainStatsSo.stateEnnemList[indexCurrentState].StartState;
    else
     CurrentUpdateState -= EMainStatsSo.stateEnnemList[indexCurrentState].StartState;
    IsCurrentStartPlayed = false;
    ;

   }
   else if(IsCurrentStatePlayed)
   {
    if (EMainStatsSo.stateEnnemList[indexCurrentState].isFixedUpdate)
     CurrentFixedState -= EMainStatsSo.stateEnnemList[indexCurrentState].PlayState;
    else
     CurrentUpdateState -= EMainStatsSo.stateEnnemList[indexCurrentState].PlayState;
    IsCurrentStatePlayed = false;
   }
   objectDictionaryState.Clear();
   timerCurrentState = 0;
   timerCondition[indexCurrentState] = 0;
   timerCurrentStartState = 0;
   indexCurrentState = 0;
  }
 }
}
