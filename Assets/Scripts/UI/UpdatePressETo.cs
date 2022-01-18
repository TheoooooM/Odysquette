using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdatePressETo : MonoBehaviour {
    [SerializeField] private string textInput = null;
    
    void Update() {
        if(GetComponent<TextMeshProUGUI>() != null) GetComponent<TextMeshProUGUI>().text = GameManager.Instance.isMouse ? "Press E " + textInput : "Press A " + textInput;
    }
}
