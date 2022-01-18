using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateStrawwItemText : MonoBehaviour
{
    void Update() {
        if(GetComponent<TextMeshProUGUI>() != null) GetComponent<TextMeshProUGUI>().text = GameManager.Instance.isMouse ? "E" : "A";
    }
}
