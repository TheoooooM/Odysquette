using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CmdPrefabData : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI methodText = null;
    [SerializeField] private Button buttonGam = null;
    public string textToWrite = "";
    
    public TextMeshProUGUI MethodText => methodText;
    public Button ButtonGam => buttonGam;
    

    /// <summary>
    /// Increment the text
    /// </summary>
    public void IncrementText() => CommandConsoleRuntime.Instance.MakeTabulation(textToWrite);
}