using SOHNE.Accessibility.Colorblindness;
using TMPro;
using UnityEngine;

public class ColorBlindTransition : MonoBehaviour {
    [SerializeField] private Colorblindness colorBlind = null;

    private void Awake() => colorBlind = colorBlind == null ? Camera.main.GetComponent<Colorblindness>() : colorBlind;

    /// <summary>
    /// Change color blind settings
    /// </summary>
    /// <param name="dropdown"></param>
    public void ChangeColorBlindSettings(TMP_Dropdown dropdown) {
        int value = dropdown.value;
        
        colorBlind.Change(value);
    }
}