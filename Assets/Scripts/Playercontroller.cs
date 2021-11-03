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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();


        playerInput = new Aled();
        playerInput.Player.Enable();
        playerInput.Player.Shoot.performed += ShootOnperformed;
        playerInput.Player.Shoot.canceled += ShootOncanceled;
        playerInput.Player.ShootGamepad.performed += ShootGamepadOnperformed;
        playerInput.Player.ShootGamepad.canceled += ShootGamepadOncanceled;
    }


    private void ShootGamepadOnperformed(InputAction.CallbackContext obj)
    {
        GameManager.Instance.isMouse = false;
        GameManager.Instance.shooting = true;
    }
    private void ShootGamepadOncanceled(InputAction.CallbackContext obj)
    {
        GameManager.Instance.shooting = false;
    }

    
    private void ShootOnperformed(InputAction.CallbackContext obj)
    {
        GameManager.Instance.shooting = true;
        GameManager.Instance.isMouse = true;
    }
    
    private void ShootOncanceled(InputAction.CallbackContext obj)
    {
        GameManager.Instance.shooting = false;
    }


    void FixedUpdate()
    {
        if (playerInput.Player.Movement.ReadValue<Vector2>() != Vector2.zero)
        {
            Vector2 moveVector = playerInput.Player.Movement.ReadValue<Vector2>();
            rb.velocity = (moveVector*MouvementSpeed);
            GameManager.Instance.isMouse = true;

        }
        else if(playerInput.Player.MovementGamepad.ReadValue<Vector2>() != Vector2.zero)
        {
            Vector2 moveVector = playerInput.Player.MovementGamepad.ReadValue<Vector2>();
            rb.velocity = (moveVector*MouvementSpeed);
            GameManager.Instance.isMouse = false;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        if (!GameManager.Instance.isMouse)
        {
            GameManager.Instance.ViewPad = playerInput.Player.ViewPad.ReadValue<Vector2>();
        }
        
        
        Debug.Log(GameManager.Instance.isMouse);

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
}
