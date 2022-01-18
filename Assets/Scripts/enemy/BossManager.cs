using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossManager : MonoBehaviour {
    [SerializeField] private List<GameObject> CircleSimonPart;
    private bool useShootSound;
    private EnemyFeedBack enemyFeedBack;
    private EnemyFeedBackMovement enemyFeedBackMovement;
    private const string turretActivation = "Activation";
    private const string turretAttack = "Attack";
    private const string bossName = "BOSS_";
    [SerializeField] private GameObject drone;
    private const string shieldName = "SHIELD_";
    private const string transitionShield = "TO";
    private const string transitionName = "_TRANSITIONTO";
    private const string baseName = "BASE";
    private const string shootName = "_SHOOT";
    private const string beginShootName = "_BEGINSHOOT";
    private const string beginSpinName = "_BEGINSPIN";
    public Animator sliderLockLife;
    private const string baseShieldTransition = shieldName + "BASE" + transitionShield + "BLUE";
    [SerializeField] private string[] colorName;
    [SerializeField] private string[] walkName;

    private const string sliderName = "SLIDER_";
    private const string idleSliderName = "_IDLE";
    private const string activationSliderName = "_ACTIVATION";
    private const string desactivationSliderName = "_DESACTIVATION";
    [SerializeField] private string[] inSpinName;
    [SerializeField] private string[] spinName;
    [SerializeField] private string[] endSpinName;

    private Animator animator;
    private ExtensionMethods.AngleAnimation[] moveFeedback;
    private ExtensionMethods.AngleAnimation[] spinFeedback;
    private ExtensionMethods.AngleAnimation[] inSpinFeedback;

    private ExtensionMethods.AngleAnimation[] endSpinFeedback;

//---------------------
    [SerializeField] private TurretStateManager[] baseturrets = new TurretStateManager[4];
    [SerializeField] public Animator shieldBoss;
    private int currentIndexEnabledTurret;
    [SerializeField] public Slider healthBar;
    [SerializeField] float timeBetweenShootTurret;

    public bool inSetPhase;
    public bool inUpdatePhase;
    public bool isTriggerDoor;


    [SerializeField] private float timeOffset;
    [SerializeField] private Animator[] shieldTurrets = new Animator[4];
    public BossStateManager enemyStateManager;
    public int[] numberEnabledTurrets;
    [SerializeField] private EMainStatsSO turret;
    static public BossManager instance;
    public ExtensionMethods.PhaseBoss currentBossPhase;
    [SerializeField] private List<HealthCondition> healthConditionList = new List<HealthCondition>();
    private int currentMaxEnabledTurret;
    private PlayerDetector playerDetector;
    private float beginTimer;
    [SerializeField] private float beginTime;

    [HideInInspector] public bool prepareShootAnimation;
    [HideInInspector] public bool inShootAnimation;
    [HideInInspector] public float shootAnimationTimer;

    public bool isBeginSpin;
    public float inSpinAnimationTime;
    public float inSpinAnimationTimer;
    public float shootAnimationTime;
    
    private bool canStartTimer = false;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    private void Start() {
        animator = GetComponent<Animator>();
        playerDetector = GetComponent<PlayerDetector>();
        enemyStateManager = GetComponent<BossStateManager>();
        enemyFeedBack = GetComponent<EnemyFeedBack>();
        enemyFeedBackMovement = GetComponent<EnemyFeedBackMovement>();

        spinFeedback = enemyFeedBackMovement.AnimationStatesList[1].angleAnimation;
        moveFeedback = enemyFeedBackMovement.AnimationStatesList[0].angleAnimation;
        inSpinFeedback = enemyFeedBackMovement.AnimationStatesListOneTime[0].angleAnimation;
        endSpinFeedback = enemyFeedBackMovement.AnimationStatesListOneTime[1].angleAnimation;

        healthBar.gameObject.SetActive(false);

        healthBar.maxValue = enemyStateManager.EMainStatsSo.maxHealth;
        healthBar.value = healthBar.maxValue;
        TransitionFeedback(0, false);
    }

    
    /// <summary>
    /// Start the boss
    /// </summary>
    public void StartBoss() {
        canStartTimer = true;
    }
    
    private void Update() {
        if (currentBossPhase == ExtensionMethods.PhaseBoss.Begin && canStartTimer) {
            if (beginTime > beginTimer) {
                beginTimer += Time.deltaTime;
            }
            else {
                currentBossPhase = ExtensionMethods.PhaseBoss.EndBegin;
                healthBar.maxValue = enemyStateManager.EMainStatsSo.maxHealth;
                playerDetector.enabled = true;
                healthBar.gameObject.SetActive(true);
            }
        }

        if (enemyStateManager.isActivate) {
            if (prepareShootAnimation) {
                if (shootAnimationTimer < shootAnimationTime)
                    shootAnimationTimer += Time.deltaTime;
                else {
                    inShootAnimation = true;
                    prepareShootAnimation = false;
                    shootAnimationTimer = 0;
                }
            }

            if (isBeginSpin) {
                if (inSpinAnimationTimer < inSpinAnimationTime) {
                    inSpinAnimationTimer += Time.deltaTime;
                }
            }

            for (int i = 0; i < healthConditionList.Count; i++) {
                CheckSetPhase(i);
            }
        }
    }

    void CheckSetPhase(int i) {
        if (!healthConditionList[i].firstUse) {
            if (healthConditionList[i].healthTrigger >= enemyStateManager.health) {
                currentBossPhase = healthConditionList[i].phaseBoss;
                healthConditionList[i].firstUse = true;
                inSetPhase = true;
            }
        }
    }


    public void SetPhase(ExtensionMethods.PhaseBoss phaseBoss) {
        currentMaxEnabledTurret = (int) phaseBoss;
        currentIndexEnabledTurret = 0;
        sliderLockLife.Play(sliderName + colorName[1] + activationSliderName);


        for (int i = 0; i < currentMaxEnabledTurret; i++) {
            baseturrets[i].enabled = true;


            baseturrets[i].GetComponent<TurretStateManager>().timerCondition.Remove(0);
            baseturrets[i].GetComponent<TurretStateManager>().timerCondition[0] =
                turret.stateEnnemList[0].timeCondition - timeOffset - timeBetweenShootTurret * i;


            if (i != 0) {
                shieldTurrets[i].Play(baseShieldTransition);
            }
            else if (i == 0) {
                CircleSimonPart[0].SetActive(true);
                baseturrets[i].boxCollider2D.enabled = true;
                shieldBoss.Play(baseShieldTransition);
                AudioManager.Instance.PlayBossSound(AudioManager.BossSoundEnum.ShieldOn);
            }

            baseturrets[i].animator.Play(turretActivation);
        }

        TransitionFeedback(1, false);


        inSetPhase = false;
    }

    public void UpdateDuringPhase() {
        Debug.Log("aled");
        baseturrets[currentIndexEnabledTurret].enabled = false;

        baseturrets[currentIndexEnabledTurret].boxCollider2D.enabled = false;
        CircleSimonPart[currentIndexEnabledTurret].SetActive(false);
        currentIndexEnabledTurret++;

        if (currentIndexEnabledTurret == currentMaxEnabledTurret) {
            drone.SetActive(true);
            shieldBoss.Play(shieldName + colorName[currentIndexEnabledTurret] + transitionShield + colorName[0]);
            sliderLockLife.Play(sliderName + colorName[currentIndexEnabledTurret] + desactivationSliderName);
            AudioManager.Instance.PlayBossSound(AudioManager.BossSoundEnum.ShieldOff);
            TransitionFeedback(currentIndexEnabledTurret, true);
            inUpdatePhase = false;
            return;
        }

        CircleSimonPart[currentIndexEnabledTurret].SetActive(true);
        AudioManager.Instance.PlayBossSound(AudioManager.BossSoundEnum.ShieldOff);
        shieldTurrets[currentIndexEnabledTurret].Play(shieldName + colorName[currentIndexEnabledTurret] + transitionShield + colorName[0]);
        sliderLockLife.Play(sliderName + colorName[currentIndexEnabledTurret + 1] + idleSliderName);
        baseturrets[currentIndexEnabledTurret].boxCollider2D.enabled = true;
        for (int i = currentIndexEnabledTurret + 1; i < currentMaxEnabledTurret; i++) {
            shieldTurrets[i].Play(shieldName + colorName[currentIndexEnabledTurret] + transitionShield + colorName[currentIndexEnabledTurret + 1]);
        }

        shieldBoss.Play(shieldName + colorName[currentIndexEnabledTurret] + transitionShield + colorName[currentIndexEnabledTurret + 1]);
        TransitionFeedback(currentIndexEnabledTurret + 1, false);

        inUpdatePhase = false;
    }

    void TransitionFeedback(int index, bool toBaseState) {
        string currentColorName = bossName + colorName[index];


        if (toBaseState) {
            UpdateStateName(0, bossName + colorName[0]);
            if (index != 0) {
                animator.Play(currentColorName + transitionName + colorName[0]);
            }
        }
        else {
            UpdateStateName(index, currentColorName);
            if (index != 0) {
                Debug.Log(bossName + colorName[index - 1] + transitionName + colorName[index]);
                animator.Play(bossName + colorName[index - 1] + transitionName + colorName[index]);
            }
        }


        //gestion du timer
    }

    void UpdateStateName(int index, string currentColorName) {
        for (int i = 0; i < moveFeedback.Length; i++)
            moveFeedback[i].stateName = currentColorName + walkName[i];

        for (int i = 0; i < spinFeedback.Length; i++)
            spinFeedback[i].stateName = currentColorName + spinName[i];

        for (int i = 0; i < endSpinFeedback.Length; i++)
            endSpinFeedback[i].stateName = currentColorName + endSpinName[i];

        for (int i = 0; i < inSpinFeedback.Length; i++)
            inSpinFeedback[i].stateName = currentColorName + inSpinName[i];

        enemyFeedBack.animationList[0] = currentColorName + beginShootName;
        enemyFeedBack.animationList[1] = currentColorName + shootName;
        enemyFeedBack.animationList[2] = currentColorName + beginSpinName;
    }

    [Serializable]
    public class HealthCondition {
        public ExtensionMethods.PhaseBoss phaseBoss;
        public bool firstUse;
        public int healthTrigger;
    }

    public void UpdateSlider(float value) {
        healthBar.value = value;
    }

    [Serializable]
    public class DoubleString {
        public string baseName;
        public string secondName;
    }

    [Serializable]
    public class ArrayString {
        public string[] stringList;
    }

    public void PlaySound(int soundIndex) {
        AudioManager.BossSoundEnum sound = (AudioManager.BossSoundEnum) soundIndex;
        if (sound == AudioManager.BossSoundEnum.ShootBoss && useShootSound)
            return;
        if (sound == AudioManager.BossSoundEnum.ShootBoss)
            useShootSound = true;

        AudioManager.Instance.PlayBossSound(sound);
    }

    public void PlaySoundWithoutCheck(int soundIndex)
    {
        AudioManager.BossSoundEnum sound = (AudioManager.BossSoundEnum) soundIndex;
        AudioManager.Instance.PlayBossSound(sound);
    }

    public void CancelSoundBossShoot() {
        useShootSound = false;
    }
}