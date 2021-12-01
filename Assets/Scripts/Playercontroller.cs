using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Playercontroller : MonoBehaviour
{
    [SerializeField]
    private Animator playerAnimator;

    [SerializeField] private AngleAnimationPlayer currentAngleAnimation;
    [SerializeField] float MouvementSpeed = 0.01f;
    private float defaultSpeed;
    public GameObject gun;
    private Vector2 lastMoveVector = Vector3.right;
    private Rigidbody2D rb;
    private PlayerMapping playerInput;
    private bool isInEffectFlash;
    private float timerInEffectFlash;
    [SerializeField] private String CurrentController;
    private CapsuleCollider2D capsuleCollider2D;
    [SerializeField]
    private bool isInWind;
    [SerializeField]
    Vector2 windDirection;
    private float windSpeed;
    private Vector2 moveVector;
    public bool isInFlash;
    [SerializeField]
    private AnimationCurve dashSpeedCurve;
    [SerializeField]
    private float maxDashSpeed;
    [SerializeField]
    private float timeDash;
    [SerializeField]
    private float timeBetweenDash;
[SerializeField]
    private Vector2 dashDirection;
    [SerializeField]
    private float timerDash;
    [SerializeField]
    private float timerBetweenDash;

    private bool isConvey;
    public Vector2 conveyorBeltSpeed;
    public AngleAnimationPlayer[] baseAngleAnimation;
    public bool InDash;
    [SerializeField]
    private bool TryDash;

    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        


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

    private void DashCanceled(InputAction.CallbackContext obj)
    {
        TryDash = false;
    }

    private void DashCanceledGamepad(InputAction.CallbackContext obj)
    {
        TryDash = false;
    }

    private void SpecialShootGamepadOnperformed(InputAction.CallbackContext obj)
    {
        GameManager.Instance.utlimate = true;
    }

    private void SpecialShootOnperformed(InputAction.CallbackContext obj)
    {
        GameManager.Instance.utlimate = true;
    }
    private void DashOnperformed(InputAction.CallbackContext obj)
    {
        TryDash = true;
        
    }
    private void DashGamepadOnperformed(InputAction.CallbackContext obj)
    {
        TryDash = true;
    }


    private void ShootGamepadOnperformed(InputAction.CallbackContext obj)
    {
        GameManager.Instance.isMouse = false;
        if(!InDash)
        GameManager.Instance.shooting = true;
    }
    private void ShootGamepadOncanceled(InputAction.CallbackContext obj)
    {
        
        GameManager.Instance.shooting = false;
    }

    
    private void ShootOnperformed(InputAction.CallbackContext obj)
    {
        if(!InDash)
        GameManager.Instance.shooting = true;
        GameManager.Instance.isMouse = true;
    }
    
    private void ShootOncanceled(InputAction.CallbackContext obj)
    {
        GameManager.Instance.shooting = false;
    }

    private void Start()
    {
        currentAngleAnimation = baseAngleAnimation[0];
        lastMoveVector = Vector2.right;
       
    }

    private void Update()
    {moveVector = Vector2.zero;
          
                 moveVector = playerInput.Player.Movement.ReadValue<Vector2>();
             
               GameManager.Instance.isMouse = true;
               if (playerInput.Player.Movement.ReadValue<Vector2>() != Vector2.zero)
                         {
                             lastMoveVector = moveVector;
                          CheckForPlayAnimation(moveVector, true);
                         }
             else if(playerInput.Player.MovementGamepad.ReadValue<Vector2>() != Vector2.zero)
             {
               moveVector = playerInput.Player.MovementGamepad.ReadValue<Vector2>();
               lastMoveVector = moveVector;
               GameManager.Instance.isMouse = false;
               CheckForPlayAnimation(moveVector, true);
               
             }
               else if (InDash)
               {
                   CheckForPlayAnimation(lastMoveVector.normalized, true);
               }
               else
               {
                  CheckForPlayAnimation(lastMoveVector.normalized, false);
               }
        if (isInEffectFlash)
        {
            if (timerInEffectFlash >= 0)
            {
                timerInEffectFlash -= Time.deltaTime;
            }
            else
            {
                timerInEffectFlash = 0;
                isInEffectFlash = false;
                MouvementSpeed = defaultSpeed;
            }
        }
        if (InDash)
        {
            timerDash += Time.deltaTime;
        }
if(timerBetweenDash<= timeBetweenDash)
{
    timerBetweenDash += Time.deltaTime;
}

    }

    void FixedUpdate()
    {
        
        if (!InDash)
        {
            if (TryDash && timerBetweenDash >= timeBetweenDash && !isInEffectFlash)
                   {
                       InDash = true;
                       timerBetweenDash = 0;
                       dashDirection = lastMoveVector.normalized;
                       rb.velocity = Vector2.zero;
                   }
               if (isInWind|| isConvey)
                        rb.velocity = (moveVector * MouvementSpeed + windDirection+conveyorBeltSpeed);
               
                       else
                    
                   rb.velocity = (moveVector*MouvementSpeed);
        }
        else if(timerDash<= timeDash)
        {
           float factorTime = timerDash / timeDash;
            rb.velocity = dashDirection * dashSpeedCurve.Evaluate(factorTime) * maxDashSpeed;
        }
        else if (timerDash > timeDash)
        {
            InDash = false;
            timerDash = 0;
            dashDirection = Vector2.zero;
        }

        if (!GameManager.Instance.isMouse)
        {
            GameManager.Instance.ViewPad = playerInput.Player.ViewPad.ReadValue<Vector2>();
        }
        
        
        //Debug.Log(GameManager.Instance.isMouse);

        #region OldMovement

        /*if (Input.GetKey(KeyCode.D))
        {
            rb.position += new Vector2(MouvementSpeed*0.2f, 0);
         
        }
        if (Input.GetKey(KeyCode.Q))
        {
            rb.position += new Vector2(-MouvementSpeed*0.2f, 0);
          
        }
        if (Input.GetKey(KeyCode.Z))
        {
            rb.position += new Vector2(0, MouvementSpeed*0.2f);
          
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.position += new Vector2(0, -MouvementSpeed*0.2f);
           
        }*/

        #endregion
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wind"))
        {
            StateWind stateWind = other.GetComponent<WindParticleManager>().StateWind;
            windDirection += stateWind.direction*stateWind.speedWind;
            
            isInWind = true;
        }

        if (other.CompareTag("Flash"))
        {
            isInFlash = true;
        }

        if (other.CompareTag("Convey"))
        {
            conveyorBeltSpeed += other.GetComponent<LDConveyorBelt>().direction;
            isConvey = true;
        }
        
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Wind"))
        {  
            StateWind stateWind = other.GetComponent<WindParticleManager>().StateWind;
            windDirection -= stateWind.direction*stateWind.speedWind;
             isInWind = false;
        }
        if (other.CompareTag("Flash"))
        {
            isInFlash = false;
        }
        if (other.CompareTag("Convey"))
        {
            isConvey = false;
            conveyorBeltSpeed -= other.GetComponent<LDConveyorBelt>().direction;
        }
       
    }

    public void SetEffectFlash(float timeFlash, float lowSpeed)
    {
        isInEffectFlash = true;
        timerInEffectFlash += timeFlash;
        MouvementSpeed = lowSpeed;


    }
[Serializable]
    public class AngleAnimationPlayer 
    {
        public float angleMin;
        public float angleMax;
        public string move;
        public string idle;


    }
        public AngleAnimationPlayer SetAngleAnimation(AngleAnimationPlayer newAngleAnimation)
        {
            AngleAnimationPlayer angleAnimation = new AngleAnimationPlayer();
            angleAnimation.angleMax = newAngleAnimation.angleMax;
            angleAnimation.angleMin = newAngleAnimation.angleMin;
            angleAnimation.idle = newAngleAnimation.idle;
            angleAnimation.move = newAngleAnimation.move;
            return angleAnimation;
        }
    public void PlayAnimation(AngleAnimationPlayer angleAnimation, bool move)
    {
        if (move)
        {
             if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(angleAnimation.move))
                        return; 
             playerAnimator.Play(currentAngleAnimation.move);
             currentAngleAnimation = SetAngleAnimation(angleAnimation);
        }
        else
        {
            if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(angleAnimation.idle))
                return;
            playerAnimator.Play(currentAngleAnimation.idle);
            currentAngleAnimation = SetAngleAnimation(angleAnimation);
        }
       
    }

    void CheckForPlayAnimation(Vector3 input, bool move)
    {
        float currentInputAngle =  Mathf.Atan2(input.y, input.x)*Mathf.Rad2Deg;
        if (Mathf.Sign(currentInputAngle) == -1)
        {
            currentInputAngle = 360 + currentInputAngle;
        }
        for (int i = 0; i < baseAngleAnimation.Length; i++)
        {

            if (currentInputAngle >= baseAngleAnimation[i].angleMin &&
                currentInputAngle <= baseAngleAnimation[i].angleMax)
            {
                          
                PlayAnimation(baseAngleAnimation[i], move);
                
            }
        } 
    }
    
}
