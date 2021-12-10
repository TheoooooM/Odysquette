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
    Debug.Log(bossParticleSystem.gameObject.activeSelf);
    Debug.Log( baseturrets[0].enabled);
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
     Debug.Log(bossParticleSystem.gameObject.activeSelf);
     Debug.Log("tu es lu là");
   
   
        
     
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
  

}
