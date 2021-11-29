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
  public ExtensionMethods.PhaseBoss currentPhaseBoss;
  public float timeBegin;
  public float timeEnd;
  public float timerBegin;
  public float timerEnd;
  private EnemyStateManager enemyStateManager;
  public int[] numberEnabledTurrets;
  [SerializeField]
  private EMainStatsSO turret;
static  public BossManager instance;
private bool beginBoss = true;

private int currentMaxEnabledTurret;
  private void Awake()
  {
    instance = this;
  }

  private void Start()
  {
    enemyStateManager = GetComponent<EnemyStateManager>();
   
  }

  private void Update()
  {
    if (beginBoss)
    {
         if (timerBegin < timeBegin)
          {
            timerBegin += Time.deltaTime;
            bossParticleSystem.gameObject.SetActive(true);
          }
          else
          { 
            SetPhase(ExtensionMethods.PhaseBoss.Begin);
           
          }
    }
 
    
   
  }

  public void SetPhase(ExtensionMethods.PhaseBoss phaseBoss)
  {
    if (phaseBoss == ExtensionMethods.PhaseBoss.Begin)
    {
         enemyStateManager.enabled = true;
         
         bossParticleSystem.gameObject.SetActive(false);
         beginBoss = false;

    }
      
    

   else
   {
    
     currentMaxEnabledTurret =(int) phaseBoss;
     currentIndexEnabledTurret = 0;
     for (int i = 0; i < currentMaxEnabledTurret; i++)
     {
       
       
   
       baseturrets[i].GetComponent<TurretStateManager>().timerCondition.Remove(0);
       baseturrets[i].GetComponent<TurretStateManager>().timerCondition[0] =
         turret.stateEnnemList[0].timeCondition - timeOffset- timeBetweenShootTurret * i;
      
baseturrets[i].enabled = true;
baseturrets[i].boxCollider2D.enabled = true;
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
   }
   
        
     
  }

  public void UpdateDuringPhase()
  {
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
  

}
