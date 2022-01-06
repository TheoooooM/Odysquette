using System.Collections.Generic;
using UnityEngine;

public class DisquetteDiscussionManager : MonoBehaviour {
    [SerializeField] private List<string> textDisquetteList = new List<string>();
    private List<string> enabledText = new List<string>();

    /// <summary>
    /// Update the 
    /// </summary>
    /// <returns></returns>
    public string UpdateText() {
        if (enabledText.Count == 0) SetEnableList();
        string text = enabledText[Random.Range(0, enabledText.Count)];
        enabledText.Remove(text);
        return text;
    }

    /// <summary>
    /// Add the string in the enable List
    /// </summary>
    private void SetEnableList() => enabledText = textDisquetteList;
}
