using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
  [SerializeField]
  private TurretStateManager[] baseturrets = new TurretStateManager[4];
  [SerializeField]
  private ParticleSystem bossParticleSystem;
  private int currentIndexEnabledTurret;

  [SerializeField]
 float timeBetweenShootTurret;

 [SerializeField] private float timeOffset;
  [SerializeField]
  private ParticleSystem[] EffectInvincible = new ParticleSystem[4];
  private EnemyStateManager enemyStateManager;
  public int[] numberEnabledTurrets;
  [SerializeField]
  private EMainStatsSO turret;
static  public BossManager instance;
public ExtensionMethods.PhaseBoss currentBossPhase;
[SerializeField]
private List<HealthCondition> healthConditionList = new List<HealthCondition>();
private int currentMaxEnabledTurret;
  private void Awake()
  {
    if (instance == null)
    {
         instance = this;
    }
 
  }

  private void Start()
  {
    
    enemyStateManager = GetComponent<EnemyStateManager>();
   
  }

  private void Update()
  {
    if (enemyStateManager.isActivate)
    {
         if (!healthConditionList[0].firstUse)
          {
                if (healthConditionList[0].healthTrigger >= enemyStateManager.health)
                {
                  Debug.Log("test");
                  SetPhase(ExtensionMethods.PhaseBoss.First);
                  healthConditionList[0].firstUse = true;
                }
          }
         else if (!healthConditionList[1].firstUse)
          {
            if (healthConditionList[1].healthTrigger >= enemyStateManager.health)
            {
              SetPhase(ExtensionMethods.PhaseBoss.Second);
              healthConditionList[1].firstUse = true;
            }
          }
      
          else if (!healthConditionList[2].firstUse)
          {
            if (healthConditionList[2].healthTrigger >= enemyStateManager.health)
            {
              SetPhase(ExtensionMethods.PhaseBoss.Third);
              healthConditionList[2].firstUse = true;
            }
          }
      
          else if (!healthConditionList[3].firstUse)
          {
            if (healthConditionList[3].healthTrigger >= enemyStateManager.health)
            {
              SetPhase(ExtensionMethods.PhaseBoss.Four);
              healthConditionList[3].firstUse = true;
            }
          }
    }
 
  }


  public void SetPhase(ExtensionMethods.PhaseBoss phaseBoss)
  {
 

     currentMaxEnabledTurret =(int) phaseBoss;
     currentIndexEnabledTurret = 0;
 
     
     for (int i = 0; i < currentMaxEnabledTurret; i++)
     {
   
     baseturrets[i].enabled = true;    Debug.Log( baseturrets[i].enabled);
     baseturrets[i].boxCollider2D.enabled = true;
       
       baseturrets[i].GetComponent<TurretStateManager>().timerCondition.Remove(0);
       baseturrets[i].GetComponent<TurretStateManager>().timerCondition[0] =
         turret.stateEnnemList[0].timeCondition - timeOffset- timeBetweenShootTurret * i;
      


       if (i != 0)
       { EffectInvincible[i].gameObject.SetActive(true);
         ParticleSystem.MainModule mainModule = EffectInvincible[i].main;
         mainModule.startColor = baseturrets[0].FxColor;

       }
      
     }
     ParticleSystem.MainModule main = bossParticleSystem.main;
     
     var mainModuleBoss = bossParticleSystem.main;
     mainModuleBoss.startColor = baseturrets[0].FxColor;
     bossParticleSystem.gameObject.SetActive(true);

      currentBossPhase = phaseBoss;


  }

  public void UpdateDuringPhase()
  {
    Debug.Log("aled");
    baseturrets[currentIndexEnabledTurret].enabled = false;
    baseturrets[currentIndexEnabledTurret].boxCollider2D.enabled = false;
    currentIndexEnabledTurret++;
    
      if (currentIndexEnabledTurret == currentMaxEnabledTurret)
      {
        
        bossParticleSystem.gameObject.SetActive(false);
        return;
      }
      EffectInvincible[currentIndexEnabledTurret].gameObject.SetActive(false);
      for (int i = currentIndexEnabledTurret; i < currentMaxEnabledTurret; i++)
    {
      var mainModuleTurret = EffectInvincible[i].main;
      mainModuleTurret.startColor =
        baseturrets[currentIndexEnabledTurret].FxColor;
    }

    var mainModule = bossParticleSystem.main;
    mainModule.startColor=
      baseturrets[currentIndexEnabledTurret].FxColor;
 
  }
[Serializable]
  public class HealthCondition
  {
    public ExtensionMethods.PhaseBoss phaseBoss;
    public bool firstUse;
    public int healthTrigger;
  }
}
