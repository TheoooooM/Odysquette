using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdatePangInput : MonoBehaviour {
    [SerializeField] private string keyboard = "E";
    [SerializeField] private string gamepad = "A";
    
    void Update() {
        if(GetComponent<TextMeshProUGUI>() != null) GetComponent<TextMeshProUGUI>().text = GameManager.Instance.isMouse ? keyboard : gamepad;
    }
}
