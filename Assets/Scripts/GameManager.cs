using System;
using System.Collections;
using System.Collections.Generic;
using LootLocker.Requests;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour {
    #region Instance

    public static GameManager Instance;

    private void Awake() {
        Instance = this;
    }

    #endregion

    #region Enum

    public enum Effect {
        none,
        bouncing,
        piercing,
        explosive,
        poison
    }

    public enum Straw {
        basic,
        bubble,
        sniper,
        helix,
        tri,
        riffle
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

    #region VARIABLES

    public bool isUltimate;
    public float maxUltimateValue;
   
    float _ultimateValue;

    public float ultimateValue {
        get { return _ultimateValue; }
        set {
            _ultimateValue = Mathf.Clamp(value, 0, maxUltimateValue);
            if (UIManager.Instance != null) UIManager.Instance.ultimateValue = _ultimateValue / maxUltimateValue;
           
            
        }
    }

    public int Score;

    [Header("---- GAME")] public bool gameIsPause = false;


    [Header("---- MOUSE")] [SerializeField]
    private float offsetPadViewFinder;

    [HideInInspector] public Vector2 mousepos; //position de la souris sur l'écran
    public float angle; //angle pour orienter la paille
    public float viewFinderDistance;
    [SerializeField] private Camera main;

    [Header("---- JUICES")] [SerializeField]
    public Effect firstEffect;


    [SerializeField] public Effect secondEffect;
    public CombinaisonColorEffect[] colorEffectsList;
    public Color currentColor;

    [Header("---- STRAW")] [SerializeField]
    private bool disableStraw = false;

    public Straw actualStraw;
    public List<StrawClass> strawsClass; //Liste de toute les pailles
    public Transform strawTRansform;
    public float timerUltimate;
    public bool shooting;
    public bool utlimate;
    public float shootCooldown;
    private int countShootRate;
    [SerializeField] private GameObject snipStrawFx;
    private float shootLoading;
    private bool EndLoading;
    public SpriteRenderer strawSprite;

    [Header("---- INPUT")] public bool isMouse = true;
    public Vector2 ViewPad;


    //Bullet
    [Header("---- SETTINGS")] [SerializeField]
    private int shootRate;

    public GameObject Player;

    [Header("---- CURVES")] [SerializeField]
    private AnimationCurve endRoomTime;

    private float timer;
    private bool animate;

    [Header("---- IN GAME EFFECT")] [SerializeField]
    private Volume endRoomPostProcess = null;

    [SerializeField] private AnimationCurve endRoomWeigthCurve = null;

    [Header("---- DEBUG")] public Vector2 _lookDir;
    public StrawClass actualStrawClass;

    #endregion

    private void OnValidate() {
        foreach (StrawClass str in strawsClass) {
            if (str.strawSO != null)
                str.StrawName = str.strawSO.strawName;
        }
    }

    private void Start() {
        AddCommandConsole();

        UIManager.Instance.backgroundUltimateUI.material.SetFloat("_Thikness", 0);
        //LeaderBoard Setup
        /*LootLockerSDKManager.StartSession("Player", (response) =>
        {
            while (!response.success) {
                Debug.Log("ok");
            }
        });*/

        if (NeverDestroy.Instance == null) Instantiate(Resources.Load<GameObject>("NeverDestroy"));
        else GetND();
        if (Playercontroller.Instance != null) {
            Player = Playercontroller.Instance.gameObject;
            if (strawTRansform == null) strawTRansform = Playercontroller.Instance.strawTransform;
        }

        animate = false;
        timer = 0;

        ChangeStraw(actualStraw);
        lastInput = Vector3.right * viewFinderDistance;

        foreach (StrawClass str in strawsClass) {
            str.StrawParent.SetActive(str == actualStrawClass);
        }

        for (int i = 0; i < colorEffectsList.Length; i++) {
            if ((colorEffectsList[i].firstEffect == firstEffect && colorEffectsList[i].secondEffect == secondEffect)
                || (colorEffectsList[i].firstEffect == secondEffect && colorEffectsList[i].secondEffect == firstEffect)) {
                currentColor = colorEffectsList[i].combinaisonColor;
            }
        }
    }


    /// <summary>
    /// Add the command to the console
    /// </summary>
    private void AddCommandConsole() {
        CommandConsole STRAW = new CommandConsole("setstraw", "setstraw : Set the actual Straw of the player", new List<CommandClass>() {new CommandClass(typeof(Straw))}, (value) => { actualStraw = (Straw) Enum.Parse(typeof(Straw), value[0]); });
        CommandConsole SCORE = new CommandConsole("addScore", "addScore : add value score to the actual score", new List<CommandClass>() {new CommandClass(typeof(int))}, (value) => { AddScore(int.Parse(value[0])); });
        CommandConsole RESOURCE = new CommandConsole("addResource", "addResource : add value to the actual Resource", new List<CommandClass>() {new CommandClass(typeof(int))}, (value) => {
            if (NeverDestroy.Instance != null) NeverDestroy.Instance.AddRessource(int.Parse(value[0]));
        });

        CommandConsole EFFECT = new CommandConsole("seteffect", "seteffect : Set the actual Effects of the player", new List<CommandClass>() {new CommandClass(typeof(Effect)), new CommandClass(typeof(Effect))}, (value) => {
            firstEffect = (Effect) Enum.Parse(typeof(Effect), value[0]);
            secondEffect = (Effect) Enum.Parse(typeof(Effect), value[1]);
        });

        CommandConsole ARSENAL = new CommandConsole("setarsenal", "setarsenal : Set the Straw and the Effects of the player", new List<CommandClass>() {new CommandClass(typeof(Straw)), new CommandClass(typeof(Effect)), new CommandClass(typeof(Effect))}, (value) => {
            actualStraw = (Straw) Enum.Parse(typeof(Straw), value[0]);
            firstEffect = (Effect) Enum.Parse(typeof(Effect), value[1]);
            secondEffect = (Effect) Enum.Parse(typeof(Effect), value[2]);
        });

        CommandConsole ULTIMATE = new CommandConsole("ultimate", "ultimate : charge the ultimate to the full value", null, (_) => {
            ultimateValue = maxUltimateValue;
        });
        
        
        CommandConsole GOTO = new CommandConsole("go", "go : Go to another area in the game", new List<CommandClass>() {new CommandClass(typeof(Area))}, (value) => {
            switch ((Area) Enum.Parse(typeof(Area), value[0])) {
                case Area.hub:
                    NeverDestroy.Instance.level = 0;
                    SetND();
                    UnityEngine.SceneManagement.SceneManager.LoadScene("HUB");
                    break;
                case Area.train:
                    NeverDestroy.Instance.level = 0;
                    SetND();
                    UnityEngine.SceneManagement.SceneManager.LoadScene("HubToLevelTransition");
                    break;
                case Area.level01:
                    NeverDestroy.Instance.level = 1;
                    SetND();
                    UnityEngine.SceneManagement.SceneManager.LoadScene("Level 01");
                    break;
                case Area.shop01:
                    NeverDestroy.Instance.level = 1;
                    SetND();
                    UnityEngine.SceneManagement.SceneManager.LoadScene("Shop");
                    break;
                case Area.level02:
                    NeverDestroy.Instance.level = 2;
                    SetND();
                    UnityEngine.SceneManagement.SceneManager.LoadScene("Level 02");
                    break;
                case Area.shop02:
                    NeverDestroy.Instance.level = 2;
                    SetND();
                    UnityEngine.SceneManagement.SceneManager.LoadScene("Shop");
                    break;
                case Area.boss:
                    NeverDestroy.Instance.level = 2;
                    SetND();
                    UnityEngine.SceneManagement.SceneManager.LoadScene("Boss");
                    break;
                case Area.lastroom:
                    foreach (RoomManager room in Generation.Instance.RoomList) {
                        if (room.runningRoom) {
                            room.runningRoom = false;
                            room.transform.GetChild(0).GetComponent<RoomContainer>().playerInRoom = false;
                            room.transform.GetChild(0).GetComponent<RoomContainer>().ActivateNeighbor(false);
                            room.transform.GetChild(0).gameObject.SetActive(false);
                        }
                        else if (room == Generation.Instance.RoomList[Generation.Instance.RoomList.Count - 1]) {
                            room.transform.GetChild(0).gameObject.SetActive(true);
                            room.runningRoom = true;
                            room.transform.GetChild(0).GetComponent<RoomContainer>().playerInRoom = true;
                            room.transform.GetChild(0).GetComponent<RoomContainer>().ActivateNeighbor(true);
                            Playercontroller.Instance.transform.position = room.transform.GetChild(0).position;
                        }
                    }

                    break;
                case Area.firstroom:
                    foreach (RoomManager room in Generation.Instance.RoomList) {
                        if (room.runningRoom) {
                            room.runningRoom = false;
                            room.transform.GetChild(0).GetComponent<RoomContainer>().playerInRoom = false;
                            room.transform.GetChild(0).GetComponent<RoomContainer>().ActivateNeighbor(false);
                            room.transform.GetChild(0).gameObject.SetActive(false);
                        }
                        else if (room == Generation.Instance.RoomList[0]) {
                            room.transform.GetChild(0).gameObject.SetActive(true);
                            room.runningRoom = true;
                            room.transform.GetChild(0).GetComponent<RoomContainer>().playerInRoom = true;
                            room.transform.GetChild(0).GetComponent<RoomContainer>().ActivateNeighbor(true);
                            Playercontroller.Instance.transform.position = room.transform.position;
                        }
                    }

                    break;
            }
        });

        CommandConsoleRuntime.Instance.AddCommand(STRAW);
        CommandConsoleRuntime.Instance.AddCommand(SCORE);
        CommandConsoleRuntime.Instance.AddCommand(RESOURCE);

        CommandConsoleRuntime.Instance.AddCommand(EFFECT);
        CommandConsoleRuntime.Instance.AddCommand(ARSENAL);
        CommandConsoleRuntime.Instance.AddCommand(ULTIMATE);
        CommandConsoleRuntime.Instance.AddCommand(GOTO);
    }

    private void Update() {
        if (!HealthPlayer.Instance.isDeath) {
            if (animate) EndRoomAnimation();

            if (isUltimate && !disableStraw) {
                if (actualStrawClass.ultimateStrawSO.timeValue > timerUltimate) {
                    timerUltimate += Time.deltaTime;
                }
                else {
                    timerUltimate = 0;
                    isUltimate = false;
                }
            }
                
            mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (shooting && !isUltimate && PoolManager.Instance != null && !disableStraw && Player.GetComponent<Playercontroller>().falling == false) {
                switch (actualStrawClass.strawSO.rateMode) {
                    case StrawSO.RateMode.FireLoading:{
                        shootLoading += Time.deltaTime;
                        if (shootLoading >= 0.35f) {
                            EndLoading = true;
                            snipStrawFx.SetActive(true);
                        }

                        if (shootLoading >= actualStrawClass.strawSO.timeValue) {
                            actualStrawClass.strawSO.Shoot(actualStrawClass.spawnerTransform, this, shootLoading);

                            shootLoading = 0;
                            snipStrawFx.SetActive(false);
                            EndLoading = false;
                        }


                        break;
                    }
                    case StrawSO.RateMode.FireRate:{
                        if (shootCooldown >= actualStrawClass.strawSO.timeValue) {
                            shootCooldown = 0;
                            if (countShootRate == actualStrawClass.strawSO.effectAllNumberShoot &&
                                (actualStrawClass.strawSO.rateMainParameter ||
                                 actualStrawClass.strawSO.rateSecondParameter)) {
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

            if (actualStrawClass.ultimateStrawSO != null && actualStrawClass.ultimateStrawSO.rateMode == StrawSO.RateMode.Ultimate && utlimate && !disableStraw && Player.GetComponent<Playercontroller>().falling == false) {
                if (ultimateValue >= 125) {
                    actualStrawClass.ultimateStrawSO.Shoot(actualStrawClass.spawnerTransform, this, 0);
                    HealthPlayer.Instance.ultimateAura.SetActive(true);
                    UIManager.Instance.backgroundUltimateUI.material.SetFloat("_Thikness", 0);
                    HealthPlayer.Instance.CancelUltimate();
                    isUltimate = true; 
                   
                    ultimateValue -= 125;
                }

                utlimate = false;
            }

            if (!shooting && !disableStraw && Player.GetComponent<Playercontroller>().falling == false) {
                if (EndLoading) {
                    actualStrawClass.strawSO.Shoot(actualStrawClass.spawnerTransform, this, shootLoading);

                    shootLoading = 0;
                    snipStrawFx.SetActive(false);
                    EndLoading = false;
                }
            }

            shootCooldown += Time.deltaTime;

            if (actualStrawClass.StrawName != "")
                shootCooldown = Mathf.Min(shootCooldown, actualStrawClass.strawSO.timeValue);

            if (actualStrawClass.StrawType != actualStraw) ChangeStraw(actualStraw);
        }
    }

    private void FixedUpdate() {
        if (!HealthPlayer.Instance.isDeath) {
            if (isMouse && actualStrawClass.StrawName != "") {
                Vector2 Position = new Vector2(actualStrawClass.StrawParent.transform.position.x,
                    actualStrawClass.StrawParent.transform.position.y);
                _lookDir = new Vector2(mousepos.x, mousepos.y) - Position;
                angle = Mathf.Atan2(_lookDir.y, _lookDir.x) * Mathf.Rad2Deg;
                if (UIManager.Instance != null) UIManager.Instance.cursor.transform.position = main.WorldToScreenPoint(mousepos);

                if (angle >= 90 && angle <= 180 || angle <= -90 && angle >= -180) strawSprite.flipY = true;
                else strawSprite.flipY = false;

                if (strawTRansform != null) strawTRansform.rotation = Quaternion.Euler(0f, 0f, angle);
            }
            else {
                if (ViewPad.magnitude > 0.5f) {
                    angle = Mathf.Atan2(ViewPad.y, ViewPad.x) * Mathf.Rad2Deg;
                    lastInput = ViewPad.normalized;
                    UIManager.Instance.cursor.transform.position = main.WorldToScreenPoint(
                        actualStrawClass.spawnerTransform.position + (Vector3) ViewPad.normalized * viewFinderDistance);
                    lastInput = ViewPad.normalized;
                }


                UIManager.Instance.cursor.transform.position = main.WorldToScreenPoint(
                    actualStrawClass.spawnerTransform.position + (Vector3) lastInput.normalized * viewFinderDistance);
            }

            actualStrawClass.StrawParent.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    #region END ROOM

    /// <summary>
    /// Make bloom in the room when the room end
    /// </summary>
    private void EndRoomAnimation() {
        timer += Time.deltaTime * (1 / Time.timeScale);
        Time.timeScale = endRoomTime.Evaluate(timer);
        if (endRoomPostProcess != null) endRoomPostProcess.weight = endRoomWeigthCurve.Evaluate(timer);

        if (timer > 1) animate = false;
    }

    /// <summary>
    /// Called when the room is finished
    /// </summary>
    public void endRoom() {
        timer = 0;
        animate = true;
    }

    #endregion END ROOM

    #region NeverDestroy

    public void GetND() {
        AddScore(NeverDestroy.Instance.Score);
        firstEffect = NeverDestroy.Instance.firstEffect;
        secondEffect = NeverDestroy.Instance.secondEffect;
        SetVisualEffect();
        actualStraw = NeverDestroy.Instance.actualStraw;
        ultimateValue = NeverDestroy.Instance.ultimateValue;
        UIManager.Instance.ultimateValue = _ultimateValue / maxUltimateValue;
        NeverDestroy.Instance.AddRessource(0);
    }

    public void SetND() {
        NeverDestroy.Instance.firstEffect = firstEffect;
        NeverDestroy.Instance.secondEffect = secondEffect;
        NeverDestroy.Instance.actualStraw = actualStraw;
        NeverDestroy.Instance.life = HealthPlayer.Instance.healthPlayer;
        NeverDestroy.Instance.ultimateValue = _ultimateValue;
        NeverDestroy.Instance.Score = Score;
    }

    #endregion

    /// <summary>
    /// Change the actual straw
    /// </summary>
    /// <param name="straw"></param>
    private void ChangeStraw(Straw straw) {
        //dictionnaire
        if (actualStrawClass.StrawParent != null) actualStrawClass.StrawParent.SetActive(false);
        foreach (StrawClass strawC in strawsClass) {
            if (strawC.StrawType == straw) actualStrawClass = strawC;
        }

        //actualStrawClass.StrawParent.GetComponent<SpriteRenderer>().sprite = actualStrawClass.strawSO.strawRenderer;
        actualStrawClass.StrawParent.SetActive(true);
        strawSprite = actualStrawClass.StrawParent.GetComponent<SpriteRenderer>();
        SetVisualEffect();
    }


    public void AddScore(int amount = 1) {
        Score += amount;
        UIManager.Instance.scoreText.text = Score.ToString();
    }


    public void SetVisualEffect() {
        for (int i = 0; i < colorEffectsList.Length; i++) {
            if (colorEffectsList[i].firstEffect == firstEffect && colorEffectsList[i].secondEffect == secondEffect
                || colorEffectsList[i].firstEffect == secondEffect && colorEffectsList[i].secondEffect == firstEffect) {
                currentColor = colorEffectsList[i].combinaisonColor;
            }
        }
    }

    /// <summary>
    /// Has the player the effect
    /// </summary>
    /// <param name="effet"></param>
    /// <returns></returns>
    public bool HasEffect(Effect effet) {
        return firstEffect == effet || secondEffect == effet;
    }
    

    [Serializable]
    public class CombinaisonColorEffect {
        public Effect firstEffect;
        public Effect secondEffect;
        public Color combinaisonColor;
    }
}

public enum Area {
    lastroom,
    firstroom,
    hub,
    train,
    level01,
    shop01,
    level02,
    shop02,
    boss
}