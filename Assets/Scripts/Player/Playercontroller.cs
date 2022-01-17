using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Playercontroller : MonoBehaviour {
    #region Variables

    [HideInInspector] public static Playercontroller Instance;
    
    [Header("---- PLAYER DATA")]
    [SerializeField] private String CurrentController = "";
    public GameObject gun;
    private PlayerMapping playerInput;
    public Transform strawTransform;
    public float currentInputAngle;
    [Header("---- MOVEMENT")] 
    [SerializeField] private bool enableMovementAtLaunch = true;
    [SerializeField] float MouvementSpeed = 0.01f;
    public bool falling;
    private bool canMove = true;
    private bool startFallAnim;
    
    private CapsuleCollider2D capsuleCollider2D;
    private Rigidbody2D rb;
    private Vector2 moveVector;
    private Vector2 lastMoveVector = Vector3.right;
    private float defaultSpeed;
    private bool canPlayAnim = false;
    
    [Header("---- DASH")]
    public bool InDash;
    [SerializeField] private bool TryDash;
    [SerializeField] private AnimationCurve dashSpeedCurve;
    [SerializeField] private float maxDashSpeed;
    [SerializeField] private float timeDash;
    [SerializeField] private float timeBetweenDash;
    private Vector2 dashDirection;
    private float timerDash;
    private float timerBetweenDash;
    [SerializeField] private float distanceBetweenImage;
    bool shootIsPress;
    private bool ultimateIsPress;
    private Vector3 lastImagePosition;
    
    [Space] public bool scaleModif = true;
    [SerializeField] private AnimationCurve dashScaleX;
    [SerializeField] private AnimationCurve dashScaleY;
    
    [Header("---- OTHER")]
    public bool isInFlash;
    private bool isInEffectFlash;
    private float timerInEffectFlash;
    private float timerKnockBack;
    private bool inKnockback;
    [SerializeField] private GameObject stunFX;
    private float currentKnockbackDistance;
    private float timeKnockback;
    private AnimationCurve curveSpeedKnockback;
    private float speedKnockback;
    private bool contactWall;
    private Vector2 currentDirectionKnockback;
    
    [SerializeField] private bool isInWind;
    [SerializeField] Vector2 windDirection;
    private float windSpeed;
    
    public Vector2 conveyorBeltSpeed;
    private bool isConvey;

    [Header("---- ANIMATION")]
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Animation fallAnimation;
    public AngleAnimationPlayer[] baseAngleAnimation;
    [SerializeField] private AngleAnimationPlayer currentAngleAnimation;
    #endregion Variables
    
    #region Basic Method
    private void Awake() {
        canPlayAnim = false;
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        if(enableMovementAtLaunch) StartInput();
        if (CommandConsoleRuntime.Instance == null) Instantiate(Resources.Load("GodMod/Command"));
        if (AudioManager.Instance == null) Instantiate(Resources.Load("---- AUDIO MANAGER"));
    }

    public void StartInput() {
        canPlayAnim = true;
        playerInput = new PlayerMapping();
        playerInput.Player.Enable();
        playerInput.Interface.Enable();
        playerInput.Player.Shoot.performed += ShootOnperformed;
        playerInput.Player.Shoot.canceled += ShootOncanceled;
        playerInput.Player.ShootGamepad.performed += ShootGamepadOnperformed;
        playerInput.Player.ShootGamepad.canceled += ShootGamepadOncanceled;
        playerInput.Player.SpecialShoot.started += SpecialShootOnperformed;
        playerInput.Player.SpecialShootGamepad.started += SpecialShootGamepadOnperformed;
        playerInput.Player.SpecialShoot.canceled += SpecialShootOncanceled;
        playerInput.Player.SpecialShootGamepad.canceled += SpecialShootGamepadOncanceled;
        playerInput.Player.Dash.performed += DashOnperformed;
        playerInput.Player.DashGamepad.performed += DashGamepadOnperformed;
        playerInput.Player.Dash.canceled += DashCanceled;
        playerInput.Player.DashGamepad.canceled += DashCanceledGamepad;
        playerInput.Interface.Pause.performed += PauseOnperformed;
        
        defaultSpeed = MouvementSpeed;
    }

    private void PauseOnperformed(InputAction.CallbackContext obj)
    {
        Debug.Log("press Pause Button");
        if (!GameManager.Instance.gameIsPause) UIManager.Instance.Pause();
        else UIManager.Instance.Unpause();
    }

    public void ChangeInputState(bool activ) {
        if(activ) playerInput.Player.Enable();
        else playerInput.Player.Disable();
    }
    
    
    private void Start() {
        currentAngleAnimation = baseAngleAnimation[0];
        lastMoveVector = Vector2.right;
    }

    private void Update()
    {
        if (!HealthPlayer.Instance.isDeath)
        {
            if (playerInput == null) return;
            if (inKnockback)
            {
                if (falling)
                    ResetKnockack();
                else if (timerKnockBack < timeKnockback)
                {
                    
                    timerKnockBack += Time.deltaTime;
                    rb.velocity = currentDirectionKnockback * curveSpeedKnockback.Evaluate(timerKnockBack / timeKnockback) * speedKnockback;
                }
                else ResetKnockack();

                if (contactWall)
                    ResetKnockack();
                return;
            }

            moveVector = Vector2.zero;

            //if (GameManager.Instance != null) GameManager.Instance.isMouse = true;
            if (playerInput.Player.Movement.ReadValue<Vector2>() != Vector2.zero && canMove)
            {
                GameManager.Instance.isMouse = true;
                moveVector = playerInput.Player.Movement.ReadValue<Vector2>();
                AudioManager.Instance.PlayPlayerSound(AudioManager.PlayerSoundEnum.Move);
                lastMoveVector = moveVector;
                if (canPlayAnim) CheckForPlayAnimation(moveVector, true);
            }
            else if (playerInput.Player.MovementGamepad.ReadValue<Vector2>() != Vector2.zero && canMove)
            {
                moveVector = playerInput.Player.MovementGamepad.ReadValue<Vector2>();
                AudioManager.Instance.PlayPlayerSound(AudioManager.PlayerSoundEnum.Move);
                lastMoveVector = moveVector;
                if (GameManager.Instance != null) GameManager.Instance.isMouse = false;
                if (canPlayAnim) CheckForPlayAnimation(moveVector, true);
            }
            else if (InDash && !falling)
            {
                if (canPlayAnim) CheckForPlayAnimation(lastMoveVector.normalized, true);
            }
            else if (!falling)
            {
                AudioManager.Instance.playerMovementAudioSource.Stop();
                if (canPlayAnim) CheckForPlayAnimation(lastMoveVector.normalized, false);
            }

            if (isInEffectFlash)
            {
                if (timerInEffectFlash >= 0)
                {
                    stunFX.SetActive(true);
                    timerInEffectFlash -= Time.deltaTime;
                }
                else
                {
                    timerInEffectFlash = 0;
                    isInEffectFlash = false;
                    stunFX.SetActive(false);
                    MouvementSpeed = defaultSpeed;
                }
            }

            if (InDash)
            {
                if (Vector3.Distance(transform.position, lastImagePosition) > distanceBetweenImage)
                {
                    PlayerAfterImagePool.Instance.GetFromPool();
                    lastImagePosition = transform.position;
                    GameManager.Instance.shooting = false;
                }

                timerDash += Time.deltaTime;
            }

            if (timerBetweenDash <= timeBetweenDash)
            {
                timerBetweenDash += Time.deltaTime;
            }
        }
    }

    private void FixedUpdate() {
        if (!HealthPlayer.Instance.isDeath)
        {
            if (inKnockback)
                return;
            if (!InDash)
            {
                if (TryDash && timerBetweenDash >= timeBetweenDash && !isInEffectFlash)
                {
                    InDash = true;
                    timerBetweenDash = 0;
                    AudioManager.Instance.PlayPlayerSound(AudioManager.PlayerSoundEnum.Dash);
                    AudioManager.Instance.playerMovementAudioSource.Stop();
                    dashDirection = lastMoveVector.normalized;
                    rb.velocity = Vector2.zero;
                    PlayerAfterImagePool.Instance.GetFromPool();
                    lastImagePosition = transform.position;
                }

                if (isInWind || isConvey)
                    rb.velocity = (moveVector * MouvementSpeed + windDirection + conveyorBeltSpeed);

                else rb.velocity = (moveVector * MouvementSpeed);
            }
            else if (timerDash <= timeDash)
            {
                float factorTime = timerDash / timeDash;
                rb.velocity =
                    dashDirection * dashSpeedCurve.Evaluate(factorTime) * maxDashSpeed; // 45 , 135 , 225 , 315


                if (scaleModif)
                {
                    if (currentInputAngle <= 45 || (currentInputAngle > 135 && currentInputAngle < 225) ||
                        currentInputAngle >= 315)
                        transform.localScale = new Vector3(dashScaleX.Evaluate(factorTime),
                            dashScaleY.Evaluate(factorTime), 0);
                    else
                        transform.localScale = new Vector3(dashScaleY.Evaluate(factorTime),
                            dashScaleX.Evaluate(factorTime), 0);
                }
            }
            else if (timerDash > timeDash)
            {
                InDash = false;
                if (shootIsPress) {
                    GameManager.Instance.shooting = true;
                }

                if (ultimateIsPress) {
                    GameManager.Instance.utlimate = true;
                }

                timerDash = 0;
                dashDirection = Vector2.zero;
            }

            if (GameManager.Instance != null && !GameManager.Instance.isMouse)
                GameManager.Instance.ViewPad = playerInput.Player.ViewPad.ReadValue<Vector2>();

            if (falling && startFallAnim)
            {
                rb.velocity = dir.normalized * 1.5f;
            }
            else if (falling)
            {
                rb.velocity = Vector2.zero;
            }
        }
    }
    
    #endregion Basic Method
    
    #region Get Inputs
    //DASH
    private void DashOnperformed(InputAction.CallbackContext obj) => TryDash = true;
    private void DashGamepadOnperformed(InputAction.CallbackContext obj) => TryDash = true;
    private void DashCanceled(InputAction.CallbackContext obj) => TryDash = false;
    private void DashCanceledGamepad(InputAction.CallbackContext obj) => TryDash = false;
    
    //SHOOT
    private void ShootOnperformed(InputAction.CallbackContext obj) {
        if (GameManager.Instance != null) {
          
            if (!InDash)
            {GameManager.Instance.shooting = true;}
            GameManager.Instance.isMouse = true;
            shootIsPress = true;
        }
    }
    private void ShootGamepadOnperformed(InputAction.CallbackContext obj) {
        if (GameManager.Instance != null) {
            if (!InDash) GameManager.Instance.shooting = true;
            GameManager.Instance.isMouse = false;
            shootIsPress = true;
        }
    }
    private void ShootOncanceled(InputAction.CallbackContext obj) { if(GameManager.Instance != null) GameManager.Instance.shooting = false; shootIsPress = false;}
    private void ShootGamepadOncanceled(InputAction.CallbackContext obj) { if(GameManager.Instance != null) GameManager.Instance.shooting = false; shootIsPress = false; }

    private void SpecialShootOnperformed(InputAction.CallbackContext obj) {
        if(GameManager.Instance != null && !InDash) GameManager.Instance.utlimate = true; 
        ultimateIsPress = true;
    }
    private void SpecialShootGamepadOnperformed(InputAction.CallbackContext obj) { 
        if(GameManager.Instance != null && !InDash) GameManager.Instance.utlimate = true;
        ultimateIsPress = true;
    }

    void SpecialShootOncanceled(InputAction.CallbackContext obj) => ultimateIsPress = false;
    void SpecialShootGamepadOncanceled(InputAction.CallbackContext obj) => ultimateIsPress = false;
    #endregion Get Inputs

    //FALL
    private Vector3 dir;
    private Vector3 dashPos;
    
    public void KnockBack(float time, Vector3 direction, AnimationCurve curveSpeed, float speed)
    {
        inKnockback = true;
        currentDirectionKnockback = direction;
        speedKnockback = speed;
        curveSpeedKnockback = curveSpeed;
        timeKnockback = time;

    }

    void ResetKnockack()
    {
        currentDirectionKnockback = Vector2.zero;
        curveSpeedKnockback = null;
        speedKnockback = 0;
        timeKnockback = 0;
        timerKnockBack = 0;
        contactWall = false;
        inKnockback = false; 
    }
    
    #region FALL
    /// <summary>
    /// When the player start to fall
    /// </summary>
    /// <param name="Indash"></param>
    /// <param name="fl"></param>
    public void StartFall(fallScript.Side side, bool Indash = false, fallScript fl = null) {
      
        switch (side)
        {
            case fallScript.Side.top:
                dir = new Vector3(0, 1, 0);
                break;
            
            case fallScript.Side.left:
                dir = new Vector3(-1, 0, 0);
                break;
            
            case fallScript.Side.right:
                dir = new Vector3(1, 0, 0);
                break;
            
            case fallScript.Side.bot:
                dir = new Vector3(0, -1, 0);
                break;
        }
        dir = rb.velocity;
        dashPos = fl !=null && Indash ? fl.dashPos : Vector3.zero;
        falling = true;
        canMove = false;
        AudioManager.Instance.PlayPlayerSound(AudioManager.PlayerSoundEnum.Fall);
        if(canPlayAnim) playerAnimator.Play("fall"); 
    }

    public void moveAnimFall()
    {
        startFallAnim = true;
    }

    /// <summary>
    /// When the player end falling. This method is called in an animation
    /// </summary>
    public void EndFall() {
        if(enableMovementAtLaunch) GetComponent<HealthPlayer>().TakeDamagePlayer(1);
        falling = false;
        startFallAnim = false;
        if(gameObject.activeSelf) StartCoroutine(FreezeAfterFall());
        if (dashPos == Vector3.zero) {
            transform.position -= dir.normalized * 2.5f;
        }
        else {
            transform.position = dashPos;
        }
        
        
    }
    
    private IEnumerator FreezeAfterFall()
    {
        ///Debug.Log("start Freeze");
        yield return new WaitForSeconds(0.3f);
        canMove = true;
        //Debug.Log("canMove :" + canMove);
    }
    
    #endregion FALL
    
    #region COLLISION
    /// <summary>
    /// When the player enter in a trigger
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other) {
        if (!HealthPlayer.Instance.isDeath)
        {
            switch (other.tag)
            {
                case "Wind":
                    StateWind stateWind = other.GetComponent<WindParticleManager>().StateWind;
                    windDirection += stateWind.direction * stateWind.speedWind;
                    isInWind = true;
                    break;

                case "Flash":
                    isInFlash = true;
                    break;

                case "Convey":
                    conveyorBeltSpeed += other.GetComponent<LDConveyorBelt>().direction;
                    isConvey = true;
                    break;
            }
        }
    }
    
    /// <summary>
    /// When the player enter in a trigger
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit2D(Collider2D other) {
        if (!HealthPlayer.Instance.isDeath)
        {
            switch (other.tag)
            {
                case "Wind":
                    StateWind stateWind = other.GetComponent<WindParticleManager>().StateWind;
                    windDirection -= stateWind.direction * stateWind.speedWind;
                    isInWind = false;
                    break;

                case "Flash":
                    isInFlash = false;
                    break;

                case "Convey":
                    isConvey = false;
                    conveyorBeltSpeed -= other.GetComponent<LDConveyorBelt>().direction;
                    break;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (!HealthPlayer.Instance.isDeath)
        {
            if (inKnockback)
            {
                if (other.gameObject.CompareTag("Walls"))
                    contactWall = true;

            }


        }
    }

    #endregion COLLISION
    
    /// <summary>
    /// When the player is flashed by an enemy
    /// </summary>
    /// <param name="timeFlash"></param>
    /// <param name="lowSpeed"></param>
    public void SetEffectFlash(float timeFlash, float lowSpeed) {
        isInEffectFlash = true;
        timerInEffectFlash += timeFlash;
        MouvementSpeed = lowSpeed;
    }
    
    #region PLAYER ANIMATION
    /// <summary>
    /// get the different animation
    /// </summary>
    /// <param name="newAngleAnimation"></param>
    /// <returns></returns>
    private AngleAnimationPlayer SetAngleAnimation(AngleAnimationPlayer newAngleAnimation) {
        AngleAnimationPlayer angleAnimation = new AngleAnimationPlayer();
        angleAnimation.angleMax = newAngleAnimation.angleMax;
        angleAnimation.angleMin = newAngleAnimation.angleMin;
        angleAnimation.idle = newAngleAnimation.idle;
        angleAnimation.move = newAngleAnimation.move;
        return angleAnimation;
    }
    /// <summary>
    /// Play an animation based on the parameter
    /// </summary>
    /// <param name="angleAnimation"></param>
    /// <param name="move"></param>
    private void PlayAnimation(AngleAnimationPlayer angleAnimation, bool move) {
        if (move) {
            if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(angleAnimation.move)) return;
            playerAnimator.Play(currentAngleAnimation.move);
            currentAngleAnimation = SetAngleAnimation(angleAnimation);
        }
        else {
            if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(angleAnimation.idle)) return;
            playerAnimator.Play(currentAngleAnimation.idle);
            currentAngleAnimation = SetAngleAnimation(angleAnimation);
        }
    }
    /// <summary>
    /// Check if the player need to play an animation
    /// </summary>
    /// <param name="input"></param>
    /// <param name="move"></param>
    private void CheckForPlayAnimation(Vector3 input, bool move) {
        currentInputAngle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
        if (Math.Abs(Mathf.Sign(currentInputAngle) - (-1)) < 0.05f) {
            currentInputAngle = 360 + currentInputAngle;
        }

        foreach (AngleAnimationPlayer angleAnim in baseAngleAnimation) {
            if (currentInputAngle >= angleAnim.angleMin &&
                currentInputAngle <= angleAnim.angleMax) {
                PlayAnimation(angleAnim, move);
            }
        }
    }
    #endregion PLAYER ANIMATION

    public void LaunchEndAnimation()
    {
        Debug.Log("Splash !");
        UIManager.Instance.endSplash.SetActive(true);
        UIManager.Instance.endSplash.GetComponent<Animator>().Play("End_Splash");
    }
}

[Serializable]
public class AngleAnimationPlayer {
    public float angleMin;
    public float angleMax;
    public string move;
    public string idle;
}