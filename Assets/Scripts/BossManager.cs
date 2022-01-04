using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BossManager : MonoBehaviour
{
  private EnemyFeedBack enemyFeedBack;
 
  private EnemyFeedBackMovement enemyFeedBackMovement;

  private const string bossName = "BOSS_";
  private const string transitionName = "_TRANSITIONTO";
  private const string baseName = "BASE";
  private const string shootName = "_SHOOT";
  private const string beginShootName = "_BEGINSHOOT";
  private const string beginSpinName = "_BEGINSPIN";
    [SerializeField] private string[] colorName;
  [SerializeField]
  private string[] walkName;

  [SerializeField] private string[] spinName;
  [SerializeField] private string[] endSpinName;

  private Animator animator;
  private ExtensionMethods.AngleAnimation[] moveFeedback;
  private ExtensionMethods.AngleAnimation[] spinFeedback;
  private ExtensionMethods.AngleAnimation[] endSpinFeedback;
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
  private BossStateManager enemyStateManager;
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

[HideInInspector]
public bool prepareShootAnimation;
[HideInInspector]
public bool inShootAnimation  ;
[HideInInspector]
public float shootAnimationTimer;

public float shootAnimationTime;
private float currentTransitionAnimationTimer;
private float currentTransitionAnimationTime;
  private void Awake()
  {
    if (instance == null)
    {
      
         instance = this;
    }
 
  }

  private void Start()
  {
    
    animator = GetComponent<Animator>();
    playerDetector = GetComponent<PlayerDetector>();
    enemyStateManager = GetComponent<BossStateManager>();
    enemyFeedBack = GetComponent<EnemyFeedBack>();
    enemyFeedBackMovement = GetComponent<EnemyFeedBackMovement>();
    bossParticleSystem.gameObject.SetActive(true);
    spinFeedback = enemyFeedBackMovement.AnimationStatesList[1].angleAnimation;
      moveFeedback = enemyFeedBackMovement.AnimationStatesList[0].angleAnimation;
      endSpinFeedback = enemyFeedBackMovement.AnimationStatesListOneTime[0].angleAnimation;
                                                                      
    healthBar.gameObject.SetActive(false);   
                                                                      
    healthBar.maxValue = enemyStateManager.EMainStatsSo.maxHealth;
    healthBar.value = healthBar.maxValue;
  TransitionFeedback(0, false);
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
      if (enemyStateManager.inTransition)
      {
        currentTransitionAnimationTimer += Time.deltaTime;
        if (currentTransitionAnimationTime < currentTransitionAnimationTimer)
          enemyStateManager.inTransition = false;
      }

      if (prepareShootAnimation)
      {
        if (shootAnimationTimer < shootAnimationTime)
          shootAnimationTimer += Time.deltaTime;
        else
        {
          inShootAnimation = true;
          prepareShootAnimation = false;
          shootAnimationTimer = 0;
        }
      }
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

     TransitionFeedback(1,  false);
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
        TransitionFeedback(currentIndexEnabledTurret, true);
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
      TransitionFeedback(currentIndexEnabledTurret,  false);

    var mainModule = bossParticleSystem.main;
    mainModule.startColor=
      baseturrets[currentIndexEnabledTurret].FxColor;
 
  }

  void TransitionFeedback(int index,  bool toBaseState)
  {
    string currentColorName = bossName+colorName[index];
   
    for (int i = 0; i <moveFeedback.Length; i++)
      moveFeedback[i].stateName = currentColorName+walkName[i];

      for (int i = 0; i < spinFeedback.Length; i++)
     spinFeedback[i].stateName = currentColorName+spinName[i];

         for (int i = 0; i <  endSpinFeedback.Length; i++)
      endSpinFeedback[i].stateName =  currentColorName+endSpinName[i];
         
    enemyFeedBack.animationList[0] = currentColorName+beginShootName;
    enemyFeedBack.animationList[1] = currentColorName+shootName;
    enemyFeedBack.animationList[2] = currentColorName+beginSpinName;

    if (toBaseState)
    {
      animator.Play(currentColorName+transitionName+colorName[0]);
    }
    else
    {
      animator.Play(currentColorName+transitionName+colorName[currentIndexEnabledTurret+1]);
    }
    StartCoroutine(ShowCurrentClipLength());
    //gestion du timer

  }

  IEnumerator ShowCurrentClipLength()
  {
    yield return new WaitForEndOfFrame();
 currentTransitionAnimationTime=  animator.GetCurrentAnimatorStateInfo(0).length;
 currentTransitionAnimationTimer = 0; 
  enemyStateManager.inTransition = true;

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
    
    public string baseName;
    public string secondName;
  }
  
  [Serializable]
  public class ArrayString
  {
    
    public string[] stringList;
  }
  
}
