using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Items : MonoBehaviour
{

   public enum type
   {
      straw, juice, life, doubleLife
   }
   
   private PlayerMapping playerInput;

   
   public bool inRange;
   [SerializeField] private type itemType;
   [SerializeField] private GameManager.Effect effect;
   [SerializeField] private GameManager.Straw straw;
   [SerializeField] private int healthValue; 
   [SerializeField] private int ressourceValue = 5;
   [SerializeField] private int cost = 5;
   [SerializeField] private GameObject shopCanvas;
   [SerializeField] private GameObject groundCanvas;
   [SerializeField] private itemSO SO;
   public bool shop;
   
   public void SpawnObject(bool ground = false) {
      playerInput = new PlayerMapping();
      if(shop)playerInput.Interface.Enable();
      playerInput.Interface.Button.started += ButtonOnperformed;
      
      shopCanvas.SetActive(false);
      groundCanvas.SetActive(false);
      
      shop = !ground;
   }

   private void Update()
   {
      if (Input.GetKeyUp(KeyCode.E) && !shop) playerInput.Interface.Enable();
   }


   /// <summary>
   /// When the player press a button
   /// </summary>
   /// <param name="obj"></param>
   private void ButtonOnperformed(InputAction.CallbackContext obj) {
         Debug.Log("Performed");
      if (inRange)
      {
         Debug.Log("in range");
         if (cost <= NeverDestroy.Instance.ressources && shop)
         {
            Debug.Log("pay " + cost);
            UseItem(obj.control.displayName);
            NeverDestroy.Instance.AddRessource(-cost);
         }
         else if(!shop)
         {
            UseItem(obj.control.displayName);
         }
         else
         {
            Debug.Log("can't buy");
         }
      }
   }

   /// <summary>
   /// When the player press a key
   /// </summary>
   /// <param name="buttonPress"></param>
   private void UseItem(string buttonPress = "E") {
      switch (buttonPress) {
          case "E":
             switch (itemType) {
                case type.straw :
                   DropStraw();
                   GameManager.Instance.actualStraw = straw;
                   break;
         
                case  type.juice: 
                   GameManager.Instance.secondEffect = effect;
                   break;
         
                case type.life : 
                   //Add life;
                   break;
                case type.doubleLife :
                   
                   //Add double life;
                   break;
                
             }
             break;
          
          case "A":
             GameManager.Instance.firstEffect = effect;
             break;
          
          case "F":
             //Add Ressources
             break;
       }
      Destroy(gameObject);
   }

   void DropStraw()
   {
      GameObject straw = GameManager.Instance.actualStraw switch
      {
         GameManager.Straw.basic => SO.basicStraw,
         GameManager.Straw.bubble => SO.bubbleStraw,
         GameManager.Straw.snipaille => SO.snipStraw,
         GameManager.Straw.eightPaille => SO.eightStraw,
         GameManager.Straw.tripaille => SO.triStraw,
         GameManager.Straw.mitra => SO.mitraStraw,
      };

     GameObject GO = Instantiate(straw, transform.position, Quaternion.identity);
     GO.GetComponent<Items>().SpawnObject(true);
   }
   
   
   /// <summary>
   /// When the player enter in the trigger of the item
   /// </summary>
   /// <param name="other"></param>
   private void OnTriggerEnter2D(Collider2D other) {
      if (other.transform.CompareTag("Player")) {
         GetComponent<SpriteRenderer>().color = Color.yellow;
         inRange = true;
         if(shop) shopCanvas.SetActive(true);
         else groundCanvas.SetActive(true);
      }
   }
   
   /// <summary>
   /// When the player exit the trigger of the item
   /// </summary>
   /// <param name="other"></param>
   private void OnTriggerExit2D(Collider2D other) {
      if (other.transform.CompareTag("Player"))
      {
         inRange = false;
         GetComponent<SpriteRenderer>().color = Color.white;
         if(shop) shopCanvas.SetActive(false);
         else groundCanvas.SetActive(false);
      }
   }
}
