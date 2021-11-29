using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Items : MonoBehaviour
{

   public enum type
   {
      straw, juice, life
   }
   
   private Aled playerInput;
   
   public bool inRange;
   [SerializeField] private type itemType;
   [Header("-----------")]
   [SerializeField] private GameManager.Effect effect;
   [SerializeField] private GameManager.Straw straw;
   [SerializeField] private int value;
   [Header("-----------")] 
   [SerializeField] private int cost;

   
   
   public void Awake()
   {
      playerInput = new Aled();
      playerInput.Player.Enable();
      playerInput.Player.Button.performed += ButtonOnperformed;
         
   }

   private void ButtonOnperformed(InputAction.CallbackContext obj)
   {
      Debug.Log(obj.control.displayName);
      UseItem(obj.control.displayName);
   }

   private void UseItem(string buttonPress = "e")
   {
      bool destroy = true;
      
       if (buttonPress == "e")
      {
         switch (itemType)
         {
            case type.straw :
               GameManager.Instance.actualStraw = straw;
               break;
         
            case  type.juice: 
               GameManager.Instance.secondEffect = effect;
               break;
         
            case type.life : 
               //Add life;
               break;
         }
      }
      else if (buttonPress == "a") GameManager.Instance.firstEffect = effect; 
      else if (buttonPress == "f") ; //Add Ressources
       else
       {
          destroy = false;
       }

       if (destroy)
       {
          Destroy(gameObject);
       }
      
   }
   
   
   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.transform.CompareTag("Player"))
      {
         GetComponent<SpriteRenderer>().color = Color.yellow;
         inRange = true;
      }
   }
   
   private void OnTriggerExit2D(Collider2D other)
   {
      if(other.transform.CompareTag("Player")) inRange = false;
         GetComponent<SpriteRenderer>().color = Color.white;
   }
}
