using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogSystem : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI text = null;
    [SerializeField] private GameObject pressEGam = null;
    [Space]
    [SerializeField, TextArea] private List<string> textList = new List<string>();
    [SerializeField] private float maxCharacterVisible = 0;
    [Space] 
    [SerializeField] private string endTrigger = "";
    [SerializeField] private string endAnimTrigger = "";
    
    private Animator dialogAnimator = null;
    private bool isInDialog = false;
    private bool canSkip = false;
    private int actualTextId = 0;

    private PlayerMapping input;

    private void Awake()
    {
        input = new PlayerMapping();
        input.Interface.Enable();
        input.Interface.InteractBtn.started += InteractBtnOnstarted;
    }

    private void InteractBtnOnstarted(InputAction.CallbackContext obj)
    {
        if (actualTextId == textList.Count - 1 && text.maxVisibleCharacters >= textList[actualTextId].Length) {
            dialogAnimator.SetTrigger(endTrigger);
            Playercontroller.Instance.ChangeInputState(true);
            isInDialog = false;
            actualTextId = 0;
        }
        else if(canSkip) {
            canSkip = false;
            maxCharacterVisible = 0;
            text.maxVisibleCharacters = (int) maxCharacterVisible;
            actualTextId++;
            pressEGam.SetActive(false);
        }
    }

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
            }
            
        }
    }
}
