using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateCanvasInfo : MonoBehaviour {
    [SerializeField] private GameObject firstEffectNameGam = null;
    [SerializeField] private GameObject secondEffectNameGam = null;
    [Space] 
    [SerializeField] private TextMeshProUGUI ultimateTxt = null;
    [SerializeField] private TextMeshProUGUI firstEffectTxt = null;
    [SerializeField] private TextMeshProUGUI secondEffectTxt = null;


    /// <summary>
    /// Update the data
    /// </summary>
    public void UpdateData() {
        firstEffectNameGam.SetActive(GameManager.Instance.firstEffect != GameManager.Effect.none);
        secondEffectNameGam.SetActive(GameManager.Instance.secondEffect != GameManager.Effect.none);
        firstEffectTxt.text = GameManager.Instance.firstEffect.ToString();
        secondEffectTxt.text = GameManager.Instance.secondEffect.ToString();

        int ultiPourcent = (int) ((GameManager.Instance.ultimateValue / GameManager.Instance.maxUltimateValue) * 100);
        ultimateTxt.text = "ULTIME " + (ultiPourcent < 10 ? "0" + ultiPourcent : ultiPourcent.ToString()) + "%";
    }
}
