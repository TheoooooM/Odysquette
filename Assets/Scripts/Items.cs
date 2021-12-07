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
   [SerializeField] private int ressourceValue;
   [SerializeField] private int cost;
   [SerializeField] private GameObject canvas;
   
   public bool shop;

   
   
   public void Awake() {
      playerInput = new PlayerMapping();
      playerInput.Interface.Enable();
      playerInput.Interface.Button.performed += ButtonOnperformed;
      canvas.SetActive(false);
   }

   private void ButtonOnperformed(InputAction.CallbackContext obj) {
      Debug.Log(obj.control.displayName);
      if(inRange) UseItem(obj.control.displayName);
   }

   private void UseItem(string buttonPress = "E") {
      switch (buttonPress) {
          case "E":
             switch (itemType) {
                case type.straw :
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
   
   
   private void OnTriggerEnter2D(Collider2D other) {
      if (other.transform.CompareTag("Player")) {
         GetComponent<SpriteRenderer>().color = Color.yellow;
         inRange = true;
         canvas.SetActive(true);
      }
   }
   
   private void OnTriggerExit2D(Collider2D other) {
      if (other.transform.CompareTag("Player"))
      {
         inRange = false;
         GetComponent<SpriteRenderer>().color = Color.white;
         canvas.SetActive(false);
      }
   }
}
