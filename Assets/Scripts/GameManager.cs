using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    #region Singleton

    public static GameManager Instance;

    private void Awake() {
        if (NeverDestroy.Instance == null) Instantiate(Resources.Load<GameObject>("NeverDestroy"));
        else GetND();
        Instance = this;
    }

    #endregion

    #region Enum

    public enum Effect {
        none,
        bounce,
        pierce,
        explosion,
        poison,
        ice
    }

    public enum Straw {
        basic,
        bubble,
        snipaille,
        eightPaille,
        tripaille,
        mitra
    }

    #endregion

    #region StrawClass

    [System.Serializable]
    public class StrawClass // Class regroupant toute les informations concernant une paille
    {
        public String StrawName;
        public Straw StrawType;
        public GameObject StrawParent; // GameObject de la paille
        public StrawSO strawSO;
        public int sizeUltimatePool;
        public StrawSO ultimateStrawSO;
        public Transform spawnerTransform; // Transform ou spawn les balles

        public int sizeShootPool; //Taille du nombre de prefabs a instancier au lancement
    }

    Vector3 lastInput;

    #endregion

    public bool isUltimate;
    public float maxUltimateValue;

    float _ultimateValue;

    public float ultimateValue {
        get { return _ultimateValue; }
        set {
            _ultimateValue = Mathf.Clamp(value, 0, maxUltimateValue);

            UIManager.Instance.UltSlider.value = _ultimateValue;
        }
    }

    [Header("mouse")] [SerializeField] private float offsetPadViewFinder;
    private Vector2 mousepos; //position de la souris sur l'écran
    public float angle; //angle pour orienter la paille
    public float viewFinderDistance;
    [SerializeField] private Camera main;

    [Header("Juices")] [SerializeField] public Effect firstEffect;


    [SerializeField] public Effect secondEffect;
    [SerializeField] CombinaisonColorEffect[] colorEffectsList;
    public Color currentColor;
    [Header("Straw")] public Straw actualStraw;
    public List<StrawClass> strawsClass; //Liste de toute les pailles
    public float timerUltimate;
    public bool shooting;
    public bool utlimate;
    private int countShootRate;

    private float shootLoading;

    private bool EndLoading;


    public float shootCooldown;

    [Header("Input")] public bool isMouse = true;
    public Vector2 ViewPad;


    //Bullet
    [Header("Settings")] [SerializeField] int ShootRate;
    [HideInInspector]public GameObject Player;

    [Header("Curves")] 
    [SerializeField] private AnimationCurve endRoomTime;
    private float timer;
    private bool animate;


    [Header("----------------DEBUG---------------")]
    public Vector2 _lookDir;

    public StrawClass actualStrawClass;

    private void OnValidate() {
        foreach (StrawClass str in strawsClass) {
            if (str.strawSO != null)
                str.StrawName = str.strawSO.strawName;
        }
    }

    private void Start()
    {
        if (Playercontroller.Instance != null) Player = Playercontroller.Instance.gameObject;
        ChangeStraw(actualStraw);
        lastInput = Vector3.right * viewFinderDistance;
        foreach (StrawClass str in strawsClass) //active la bonne paille au début
        {
            if (str == actualStrawClass) {
                str.StrawParent.SetActive((true));
            }
            else {
                str.StrawParent.SetActive((false));
            }
        }

        for (int i = 0; i < colorEffectsList.Length; i++) {
            if ((colorEffectsList[i].firstEffect == firstEffect && colorEffectsList[i].secondEffect == secondEffect)
                || (colorEffectsList[i].firstEffect == secondEffect && colorEffectsList[i].secondEffect == firstEffect)) {
                currentColor = colorEffectsList[i].combinaisonColor;
            }
        }
    }

    private void Update()
    {
            if (timer > 1)
            {
                animate = false;
            }
            if (animate)
        {
            timer += Time.deltaTime * (1 / Time.timeScale);
            Time.timeScale = endRoomTime.Evaluate(timer);
        }
        
        
        if (isUltimate) {
            if (actualStrawClass.ultimateStrawSO.ultimateTime > timerUltimate) {
                timerUltimate += Time.deltaTime;
            }
            else {
                timerUltimate = 0;
                isUltimate = false;
            }
        }

        mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (shooting && !isUltimate) {
            switch (actualStrawClass.strawSO.rateMode) {
                case StrawSO.RateMode.FireLoading:{
                    shootLoading += Time.deltaTime;
                    if (shootLoading >= 0.25f) {
                        EndLoading = true;
                    }

                    if (shootLoading >= actualStrawClass.strawSO.timeValue - 0.1f) {
                        EndLoading = false;
                    }

                    if (shootLoading >= actualStrawClass.strawSO.timeValue) {
                        actualStrawClass.strawSO.Shoot(actualStrawClass.spawnerTransform, this, shootLoading);
                        shootLoading = 0;
                    }


                    break;
                }
                case StrawSO.RateMode.FireRate:{
                    if (shootCooldown >= actualStrawClass.strawSO.timeValue) {
                        shootCooldown = 0;
                        if (countShootRate == actualStrawClass.strawSO.effectAllNumberShoot && (actualStrawClass.strawSO.rateMainParameter || actualStrawClass.strawSO.rateSecondParameter)) {
                            actualStrawClass.strawSO.Shoot(actualStrawClass.spawnerTransform, this, 1);
                            countShootRate = 0;
                        }
                        else {
                            actualStrawClass.strawSO.Shoot(actualStrawClass.spawnerTransform, this, 0);
                            countShootRate++;
                        }
                    }

                    break;
                }
            }
        }

        if (actualStrawClass.ultimateStrawSO != null && actualStrawClass.ultimateStrawSO.rateMode == StrawSO.RateMode.Ultimate && utlimate) {
            Debug.Log("testssss");
            if (ultimateValue >= actualStrawClass.ultimateStrawSO.timeValue) {
                Debug.Log("ouhahahahah");
                actualStrawClass.ultimateStrawSO.Shoot(actualStrawClass.spawnerTransform, this, 0);
                isUltimate = true;
                ultimateValue -= actualStrawClass.ultimateStrawSO.timeValue;
            }

            utlimate = false;
        }

        if (!shooting) {
            if (EndLoading) {
                actualStrawClass.strawSO.Shoot(actualStrawClass.spawnerTransform, this, shootLoading);
                shootLoading = 0;

                EndLoading = false;
            }
        }

        shootCooldown += Time.deltaTime;

        shootCooldown = Mathf.Min(shootCooldown, actualStrawClass.strawSO.timeValue);
        
        if(actualStrawClass.StrawType != actualStraw) ChangeStraw(actualStraw);
    }

    private void FixedUpdate() {
        //---------------- Oriente la paille ------------------------
        if (isMouse) {
            Vector2 Position = new Vector2(actualStrawClass.StrawParent.transform.position.x, actualStrawClass.StrawParent.transform.position.y);
            _lookDir = new Vector2(mousepos.x, mousepos.y) - Position;
            angle = Mathf.Atan2(_lookDir.y, _lookDir.x) * Mathf.Rad2Deg;
            if (UIManager.Instance != null) UIManager.Instance.cursor.transform.position = main.WorldToScreenPoint(mousepos);


            actualStrawClass.StrawParent.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
        else {
            if (ViewPad.magnitude > 0.5f) {
                angle = Mathf.Atan2(ViewPad.y, ViewPad.x) * Mathf.Rad2Deg;

                lastInput = ViewPad.normalized;

                Debug.Log("test");

                UIManager.Instance.cursor.transform.position = main.WorldToScreenPoint(actualStrawClass.spawnerTransform.position + (Vector3) ViewPad.normalized * viewFinderDistance);
                lastInput = ViewPad.normalized;
            }


            UIManager.Instance.cursor.transform.position = main.WorldToScreenPoint(actualStrawClass.spawnerTransform.position + (Vector3) lastInput.normalized * viewFinderDistance);
        }

        actualStrawClass.StrawParent.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        //--------------------------------------------------------------
    }

    public void GetND() {
        firstEffect = NeverDestroy.Instance.firstEffect;
        secondEffect = NeverDestroy.Instance.secondEffect;
        actualStraw = NeverDestroy.Instance.actualStraw;
    }

    public void SetND() {
        NeverDestroy.Instance.firstEffect = firstEffect;
        NeverDestroy.Instance.secondEffect = secondEffect;
        NeverDestroy.Instance.actualStraw = actualStraw;
        NeverDestroy.Instance.life = HealthPlayer.Instance.healthPlayer;
    }

    public void endRoom()
    {
        timer = 0;
        animate = true;
    }

    void ChangeStraw(Straw straw) //change la paille 
    {
        //dictionnaire
        if(actualStrawClass.StrawParent != null) actualStrawClass.StrawParent.SetActive(false);
        foreach (StrawClass strawC in strawsClass) {
            if (strawC.StrawType == straw) actualStrawClass = strawC;
        }
        actualStrawClass.StrawParent.GetComponent<SpriteRenderer>().sprite = actualStrawClass.strawSO.strawRenderer;
        actualStrawClass.StrawParent.SetActive(true);
        for (int i = 0; i < colorEffectsList.Length; i++) {
            if (colorEffectsList[i].firstEffect == firstEffect && colorEffectsList[i].secondEffect == secondEffect
                || colorEffectsList[i].firstEffect == secondEffect && colorEffectsList[i].secondEffect == firstEffect) {
                currentColor = colorEffectsList[i].combinaisonColor;
            }
        }
    }

    public enum ShootMode {
        BasicShoot,
        CurveShoot,
        AreaShoot,
        AngleAreaShoot
    }

    [Serializable]
    public class CombinaisonColorEffect {
        public Effect firstEffect;
        public Effect secondEffect;
        public Color combinaisonColor;
    }
}