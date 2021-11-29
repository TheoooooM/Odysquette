using UnityEngine;
using UnityEngine.InputSystem;

public class Items : MonoBehaviour
{

   public enum type
   {
      straw, juice, life
   }
   
   private PlayerMapping playerInput;
   
   public bool inRange;
   [SerializeField] private type itemType;
   [SerializeField] private GameManager.Effect effect;
   [SerializeField] private GameManager.Straw straw;
   [SerializeField] private int healthValue;
   [SerializeField] private int ressourceValue;
   [SerializeField] private int cost;

   
   
   public void Awake() {
      playerInput = new PlayerMapping();
      playerInput.Interface.Enable();
      playerInput.Interface.Button.performed += ButtonOnperformed;
   }

   private void ButtonOnperformed(InputAction.CallbackContext obj) {
      Debug.Log(obj.control.displayName);
      UseItem(obj.control.displayName);
   }

   private void UseItem(string buttonPress = "e") {
      bool destroy = true;
      
       switch (buttonPress) {
          case "e":
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
             }
             break;
          
          case "a":
             GameManager.Instance.firstEffect = effect;
             break;
          
          case "f":
             //Add Ressources
             break;
          
          default:
             destroy = false;
             break;
       }

       if (destroy) {
          Destroy(gameObject);
       }
   }
   
   
   private void OnTriggerEnter2D(Collider2D other) {
      if (other.transform.CompareTag("Player")) {
         GetComponent<SpriteRenderer>().color = Color.yellow;
         inRange = true;
      }
   }
   
   private void OnTriggerExit2D(Collider2D other) {
      if(other.transform.CompareTag("Player")) inRange = false;
         GetComponent<SpriteRenderer>().color = Color.white;
   }
}
