using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
 

    public GameObject viewFinder;
    public GameObject[] HeartsLife;
    public Slider UltSlider;
    // Start is called before the first frame update
    public static UIManager Instance;
    float maxUltSlider = 100f;
    private void Awake()
    {
        
        Instance = this;
    }
    private void Start()
    {
        UltSlider.maxValue = GameManager.Instance.maxUltimateValue;
    }


    // Update is called once per frame

}
