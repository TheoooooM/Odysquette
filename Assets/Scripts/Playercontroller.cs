using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Playercontroller : MonoBehaviour {
    #region Variables

    [HideInInspector] public static Playercontroller Instance;
    
    [Header("---- PLAYER DATA")]
    [SerializeField] private String CurrentController = "";
    public GameObject gun;
    private PlayerMapping playerInput;


    [Header("---- MOVEMENT")] 
    [SerializeField] private bool enableMovementAtLaunch = true;
    [SerializeField] float MouvementSpeed = 0.01f;
    public bool falling;
    
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
    
    [Header("---- OTHER")]
    public bool isInFlash;
    private bool isInEffectFlash;
    private float timerInEffectFlash;
    
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
    }

    public void StartInput() {
        canPlayAnim = true;
        playerInput = new PlayerMapping();
        playerInput.Player.Enable();
        playerInput.Player.Shoot.performed += ShootOnperformed;
        playerInput.Player.Shoot.canceled += ShootOncanceled;
        playerInput.Player.ShootGamepad.performed += ShootGamepadOnperformed;
        playerInput.Player.ShootGamepad.canceled += ShootGamepadOncanceled;
        playerInput.Player.SpecialShoot.performed += SpecialShootOnperformed;
        playerInput.Player.SpecialShootGamepad.performed += SpecialShootGamepadOnperformed;
        
        playerInput.Player.Dash.performed += DashOnperformed;
        playerInput.Player.DashGamepad.performed += DashGamepadOnperformed;
        playerInput.Player.Dash.canceled += DashCanceled;
        playerInput.Player.DashGamepad.canceled += DashCanceledGamepad;
        
        defaultSpeed = MouvementSpeed;
    }
    
    private void Start() {
        currentAngleAnimation = baseAngleAnimation[0];
        lastMoveVector = Vector2.right;
    }
    private void Update() {
        if (playerInput == null) return;
        moveVector = Vector2.zero;

        if(GameManager.Instance != null) GameManager.Instance.isMouse = true;
        if (playerInput.Player.Movement.ReadValue<Vector2>() != Vector2.zero && !falling) {
            moveVector = playerInput.Player.Movement.ReadValue<Vector2>();
            lastMoveVector = moveVector;
            if(canPlayAnim) CheckForPlayAnimation(moveVector, true);
        }
        else if (playerInput.Player.MovementGamepad.ReadValue<Vector2>() != Vector2.zero && !falling) {
            moveVector = playerInput.Player.MovementGamepad.ReadValue<Vector2>();
            lastMoveVector = moveVector;
            if(GameManager.Instance != null) GameManager.Instance.isMouse = false;
            if(canPlayAnim) CheckForPlayAnimation(moveVector, true);
        }
        else if (InDash && !falling) {
            if(canPlayAnim) CheckForPlayAnimation(lastMoveVector.normalized, true);
        }
        else if(!falling){
            if(canPlayAnim) CheckForPlayAnimation(lastMoveVector.normalized, false);
        }

        if (isInEffectFlash) {
            if (timerInEffectFlash >= 0) {
                timerInEffectFlash -= Time.deltaTime;
            }
            else {
                timerInEffectFlash = 0;
                isInEffectFlash = false;
                MouvementSpeed = defaultSpeed;
            }
        }

        if (InDash) {
            timerDash += Time.deltaTime;
        }

        if (timerBetweenDash <= timeBetweenDash) {
            timerBetweenDash += Time.deltaTime;
        }
    }
    private void FixedUpdate() {
        if (!InDash) {
            if (TryDash && timerBetweenDash >= timeBetweenDash && !isInEffectFlash) {
                InDash = true;
                timerBetweenDash = 0;
                dashDirection = lastMoveVector.normalized;
                rb.velocity = Vector2.zero;
            }

            if (isInWind || isConvey) rb.velocity = (moveVector * MouvementSpeed + windDirection + conveyorBeltSpeed);
            else rb.velocity = (moveVector * MouvementSpeed);
        }
        else if (timerDash <= timeDash) {
            float factorTime = timerDash / timeDash;
            rb.velocity = dashDirection * dashSpeedCurve.Evaluate(factorTime) * maxDashSpeed;
        }
        else if (timerDash > timeDash) {
            InDash = false;
            timerDash = 0;
            dashDirection = Vector2.zero;
        }

        if (GameManager.Instance != null && !GameManager.Instance.isMouse) GameManager.Instance.ViewPad = playerInput.Player.ViewPad.ReadValue<Vector2>();

        if (falling) {
            rb.velocity = dir.normalized * 1.5f;
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
            if (!InDash) GameManager.Instance.shooting = true;
            GameManager.Instance.isMouse = true;
        }
    }
    private void ShootGamepadOnperformed(InputAction.CallbackContext obj) {
        if (GameManager.Instance != null) {
            if (!InDash) GameManager.Instance.shooting = true;
            GameManager.Instance.isMouse = true;
        }
    }
    private void ShootOncanceled(InputAction.CallbackContext obj) { if(GameManager.Instance != null) GameManager.Instance.shooting = false; }
    private void ShootGamepadOncanceled(InputAction.CallbackContext obj) { if(GameManager.Instance != null) GameManager.Instance.shooting = false; }
    private void SpecialShootOnperformed(InputAction.CallbackContext obj) { if(GameManager.Instance != null) GameManager.Instance.utlimate = true; }
    private void SpecialShootGamepadOnperformed(InputAction.CallbackContext obj) { if(GameManager.Instance != null) GameManager.Instance.utlimate = true; }
    #endregion Get Inputs

    //FALL
    private Vector3 dir;
    private Vector3 dashPos;
    
    #region FALL
    /// <summary>
    /// When the player start to fall
    /// </summary>
    /// <param name="Indash"></param>
    /// <param name="fl"></param>
    public void StartFall(bool Indash = false, fallScript fl = null) {
        dir = rb.velocity;
        dashPos = fl !=null && Indash ? fl.dashPos : Vector3.zero;
        falling = true;
        if(canPlayAnim) playerAnimator.Play("fall"); 
    }

    /// <summary>
    /// When the player end falling. This method is called in an animation
    /// </summary>
    public void EndFall() {
        if(enableMovementAtLaunch) GetComponent<HealthPlayer>().TakeDamagePlayer(1);
        falling = false;
        if (dashPos == Vector3.zero) {
            transform.position -= dir.normalized * 2.5f;
        }
        else {
            transform.position = dashPos;
        }
    }
    #endregion FALL
    
    #region COLLISION
    /// <summary>
    /// When the player enter in a trigger
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other) {
        switch (other.tag) {
            case "Wind" :
                StateWind stateWind = other.GetComponent<WindParticleManager>().StateWind;
                windDirection += stateWind.direction * stateWind.speedWind;
                isInWind = true;
                break;
            
            case "Flash" :
                isInFlash = true;
                break;
            
            case "Convey" :
                conveyorBeltSpeed += other.GetComponent<LDConveyorBelt>().direction;
                isConvey = true;
                break;
        }
    }
    
    /// <summary>
    /// When the player enter in a trigger
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit2D(Collider2D other) {
        switch (other.tag) {
            case "Wind" :
                StateWind stateWind = other.GetComponent<WindParticleManager>().StateWind;
                windDirection -= stateWind.direction * stateWind.speedWind;
                isInWind = false;
                break;
            
            case "Flash" :
                isInFlash = false;
                break;
            
            case "Convey" :
                isConvey = false;
                conveyorBeltSpeed -= other.GetComponent<LDConveyorBelt>().direction;
                break;
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
        float currentInputAngle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
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
}

[Serializable]
public class AngleAnimationPlayer {
    public float angleMin;
    public float angleMax;
    public string move;
    public string idle;
}