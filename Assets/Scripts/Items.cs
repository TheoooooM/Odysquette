using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Items : MonoBehaviour {

   public enum type {
      straw,
      juice,
      life,
      doubleLife
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

   /// <summary>
   /// Spawn the object
   /// </summary>
   /// <param name="ground"></param>
   public void SpawnObject(bool ground = false) {
      playerInput = new PlayerMapping();
      if (shop) playerInput.Interface.Enable();
      playerInput.Interface.Button.started += ButtonOnperformed;

      groundCanvas.SetActive(false);
      if (transform.GetChild(0).GetComponent<SetStrawUI>() == null) shopCanvas.SetActive(false);

      shop = !ground;
   }

   private void Update() {
      if (Input.GetKeyUp(KeyCode.E) && !shop) playerInput.Interface.Enable();
   }


   /// <summary>
   /// When the player press a button
   /// </summary>
   /// <param name="obj"></param>
   private void ButtonOnperformed(InputAction.CallbackContext obj) {
      Debug.Log("Performed");
      if (inRange) {
         Debug.Log("in range");
         if (cost <= NeverDestroy.Instance.ressources && shop) {
            Debug.Log("pay " + cost);
            UseItem(obj.control.displayName);
            NeverDestroy.Instance.AddRessource(-cost);
         }
         else if (!shop) {
            UseItem(obj.control.displayName);
         }
         else {
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
               case type.straw:
                  transform.GetChild(0).GetComponent<SetStrawUI>().DestroyActualStrawData();
                  DropStraw();
                  GameManager.Instance.actualStraw = straw;
                  
                  break;

               case type.juice:
                  GameManager.Instance.secondEffect = effect;
                  GameManager.Instance.SetVisualEffect();
                  break;

               case type.life:
                  //Add life;
                  break;
               case type.doubleLife:

                  //Add double life;
                  break;
            }
            AudioManager.Instance.PlayPlayerSound(AudioManager.PlayerSoundEnum.TakeItem);
            Destroy(gameObject);
            break;

         case "A":
            switch (itemType) {
               case type.straw: break;
               case type.juice:
                  GameManager.Instance.firstEffect = effect;
                  GameManager.Instance.SetVisualEffect();
                  Destroy(gameObject);
                  break;

               case type.life: break;

               case type.doubleLife: break;
            }
            AudioManager.Instance.PlayPlayerSound(AudioManager.PlayerSoundEnum.TakeItem);
            break;
      }
   }

   /// <summary>
   /// Drop the actual straw on the ground
   /// </summary>
   private void DropStraw() {
      GameObject newStrawToSpawn = GameManager.Instance.actualStraw switch {
         GameManager.Straw.basic => SO.basicStraw,
         GameManager.Straw.bubble => SO.bubbleStraw,
         GameManager.Straw.sniper => SO.snipStraw,
         GameManager.Straw.helix => SO.eightStraw,
         GameManager.Straw.tri => SO.triStraw,
         GameManager.Straw.riffle => SO.mitraStraw,
         _ => throw new ArgumentOutOfRangeException()
      };

      GameObject GO = Instantiate(newStrawToSpawn, transform.position, Quaternion.identity);
      GO.GetComponent<Items>().SpawnObject(true);
      GO.transform.GetChild(0).GetComponent<SetStrawUI>().setData(GameManager.Instance.actualStraw, false, true);
   }
   
   /// <summary>
   /// Set a new straw called by SetStrawUI script
   /// </summary>
   /// <param name="newStraw"></param>
   public void SetNewStraw(GameManager.Straw newStraw) {
      itemType = type.straw;
      straw = newStraw;
   }

   #region PLAYER DETECTION

   /// <summary>
   /// When the player enter in the trigger of the item
   /// </summary>
   /// <param name="other"></param>
   private void OnTriggerEnter2D(Collider2D other) {
      if (other.transform.CompareTag("Player")) {
         GetComponent<SpriteRenderer>().color = Color.yellow;
         inRange = true;
         if (itemType == type.straw) {
            groundCanvas.SetActive(true);
            transform.GetChild(0).GetComponent<SetStrawUI>().ShowActualStrawData();
         }
         else {
            if (shop) shopCanvas.SetActive(true);
            else groundCanvas.SetActive(true);
         }
      }
   }

   /// <summary>
   /// When the player exit the trigger of the item
   /// </summary>
   /// <param name="other"></param>
   private void OnTriggerExit2D(Collider2D other) {
      if (other.transform.CompareTag("Player")) {
         inRange = false;
         GetComponent<SpriteRenderer>().color = Color.white;
         if (itemType == type.straw) {
            groundCanvas.SetActive(false);
            transform.GetChild(0).GetComponent<SetStrawUI>().DestroyActualStrawData();
         }
         else {
            if (shop) shopCanvas.SetActive(false);
            else groundCanvas.SetActive(false);
         }
      }
   }

   #endregion PLAYER DETECTION
}
