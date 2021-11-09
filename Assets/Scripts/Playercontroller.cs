using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Playercontroller : MonoBehaviour
{
    [SerializeField] float MouvementSpeed = 0.01f;
    public GameObject gun;
    private Rigidbody2D rb;
    private Aled playerInput;
    [SerializeField] private String CurrentController;
    private CapsuleCollider2D capsuleCollider2D;
    private bool isInWind;
    Vector2 windDirection;
    private float windSpeed;
    private Vector2 moveVector;
    
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

    public bool InDash;
    [SerializeField]
    private bool TryDash;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();


        playerInput = new Aled();
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

    private void Update()
    {
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
        moveVector = Vector2.zero;
        if (playerInput.Player.Movement.ReadValue<Vector2>() != Vector2.zero)
        {
          moveVector = playerInput.Player.Movement.ReadValue<Vector2>();
          if (!InDash&& TryDash && timerBetweenDash >= timeBetweenDash)
          {
              InDash = true;
              timerBetweenDash = 0;
              dashDirection = moveVector.normalized;
              rb.velocity = Vector2.zero;
          }
           
            GameManager.Instance.isMouse = true;

        }
        else if(playerInput.Player.MovementGamepad.ReadValue<Vector2>() != Vector2.zero)
        {
          moveVector = playerInput.Player.MovementGamepad.ReadValue<Vector2>();
          if (!InDash&& TryDash && timerBetweenDash >= timeBetweenDash)
          {
              InDash = true;
              timerBetweenDash = 0;
              dashDirection = moveVector.normalized;
              rb.velocity = Vector2.zero;
          }
            GameManager.Instance.isMouse = false;
        }

        if (!InDash)
        {
               if (isInWind)
                        rb.velocity = (moveVector * MouvementSpeed + windDirection * windSpeed);
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

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Wind"))
        {
            StateWind stateWind = other.GetComponent<WindParticleManager>().StateWind;
            windDirection = stateWind.direction;
            windSpeed = stateWind.speedWind;
            isInWind = true;
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        isInWind = false;
    }
}
