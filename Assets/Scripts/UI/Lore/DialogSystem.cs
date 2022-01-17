using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DialogSystem : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI text = null;
    [SerializeField] private GameObject pressEGam = null;
    [Space]
    [SerializeField, TextArea] private List<string> textList = new List<string>();
    [SerializeField] private float maxCharacterVisible = 0;
    [Space] 
    [SerializeField] private string endTrigger = "";
    [SerializeField] private string endAnimTrigger = "";
    [Space]
    [SerializeField] private bool useCustomInput = false;
    [SerializeField] private int pressCustomInputID = 0;

    [SerializeField] private UnityEvent endDiscussion = null;
    
    private Animator dialogAnimator = null;
    private bool isInDialog = false;
    private bool canSkip = false;
    private int actualTextId = 0;
    
    private void Start() => dialogAnimator = GetComponent<Animator>();
    
    public void StartDialog() {
        pressEGam.SetActive(false);
        isInDialog = true;
    }

    private void Update() {
        if (isInDialog) {
            Playercontroller.Instance.ChangeInputState(false);
            maxCharacterVisible++;
            text.maxVisibleCharacters = (int) maxCharacterVisible;
            text.text = textList[actualTextId];

            if (text.maxVisibleCharacters >= textList[actualTextId].Length) {
                canSkip = true;
                pressEGam.SetActive(true);
                pressEGam.GetComponent<TextMeshProUGUI>().text = actualTextId == pressCustomInputID && useCustomInput ? "PRESS RIGHT MOUSE BUTTON" : "PRESS E";
            }
            
            if (Input.GetKeyDown(KeyCode.E) && (actualTextId != pressCustomInputID || !useCustomInput)) {
                ChangeTextState();
            }
            else if (Input.GetMouseButtonDown(1) && actualTextId == pressCustomInputID && useCustomInput) {
                GameManager.Instance.ShootUltimate();
                ChangeTextState();
            }
        }
    }

    /// <summary>
    /// Change the text by writing all the letters or going to the next text
    /// </summary>
    private void ChangeTextState() {
        if (actualTextId == textList.Count - 1 && text.maxVisibleCharacters >= textList[actualTextId].Length) {
            dialogAnimator.SetTrigger(endTrigger);
            Playercontroller.Instance.ChangeInputState(true);
            isInDialog = false;
            actualTextId = 0;
            endDiscussion.Invoke();
        }
        else if(canSkip) {
            canSkip = false;
            maxCharacterVisible = 0;
            text.maxVisibleCharacters = (int) maxCharacterVisible;
            actualTextId++;
            
            if (actualTextId == pressCustomInputID && useCustomInput) {
                if (GameManager.Instance != null) GameManager.Instance.ultimateValue = GameManager.Instance.maxUltimateValue;
            }
            
            pressEGam.SetActive(false);
        }
    }
}
