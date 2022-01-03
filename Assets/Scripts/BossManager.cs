using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossManager : MonoBehaviour
{


  [SerializeField] private DoubleString[] transitionAnimationList;
  [SerializeField] private ArrayString[] moveAnimationList;
  [SerializeField] private ArrayString[] spinAnimationList;
  [SerializeField] private string[] beginShootAnimationList;
  [SerializeField] private string[] beginSpinAnimationList;
  [SerializeField] private string[] shootAnimationList;
  [SerializeField] private ArrayString[] endSpinAnimationList;
  
//---------------------
  [SerializeField]
  
  private TurretStateManager[] baseturrets = new TurretStateManager[4];
  [SerializeField]
  private ParticleSystem bossParticleSystem;
  private int currentIndexEnabledTurret;
  [SerializeField]
  private Slider healthBar;
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
private PlayerDetector playerDetector;
private float beginTimer;
[SerializeField]
private float beginTime;
  private void Awake()
  {
    if (instance == null)
    {
         instance = this;
    }
 
  }

  private void Start()
  {
    playerDetector = GetComponent<PlayerDetector>();
    enemyStateManager = GetComponent<EnemyStateManager>();         
    bossParticleSystem.gameObject.SetActive(true);
                                                                      
    healthBar.gameObject.SetActive(false);   
                                                                      
    healthBar.maxValue = enemyStateManager.EMainStatsSo.maxHealth;
    healthBar.value = healthBar.maxValue;

  }

  private void Update()
  {
    if (currentBossPhase == ExtensionMethods.PhaseBoss.Begin)
    {
      if (beginTime > beginTimer)
      {
                beginTimer += Time.deltaTime;  
   
      }
      else
      {
        currentBossPhase = ExtensionMethods.PhaseBoss.EndBegin;
         healthBar.maxValue = enemyStateManager.EMainStatsSo.maxHealth;
         playerDetector.enabled = true;
         healthBar.gameObject.SetActive(true);   
         bossParticleSystem.gameObject.SetActive(false);
       
      }
     
      
    }
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
     enemyStateManager.collider2D.enabled = false;
 
     
     for (int i = 0; i < currentMaxEnabledTurret; i++)
     {
   
     baseturrets[i].enabled = true;    Debug.Log( baseturrets[i].enabled);
     
       
       baseturrets[i].GetComponent<TurretStateManager>().timerCondition.Remove(0);
       baseturrets[i].GetComponent<TurretStateManager>().timerCondition[0] =
         turret.stateEnnemList[0].timeCondition - timeOffset- timeBetweenShootTurret * i;
      


       if (i != 0)
       { 
         EffectInvincible[i].gameObject.SetActive(true);
         ParticleSystem.MainModule mainModule = EffectInvincible[i].main;
         mainModule.startColor = baseturrets[0].FxColor;

       }
       else if (i == 0)
       {
         Debug.Log("testaa");
         baseturrets[i].boxCollider2D.enabled = true;
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
        enemyStateManager.collider2D.enabled = true;
        bossParticleSystem.gameObject.SetActive(false);
        return;
      }
      EffectInvincible[currentIndexEnabledTurret].gameObject.SetActive(false);
      baseturrets[currentIndexEnabledTurret].boxCollider2D.enabled = true;
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

  public void UpdateSlider(float value)
  {
    healthBar.value = value;
  }

  [Serializable]
  public class DoubleString
  {
    
    public string firstString;
    public string secondString;
  }
  
  [Serializable]
  public class ArrayString
  {
    public int index; 
    public string[] stringList;
  }
  
}
