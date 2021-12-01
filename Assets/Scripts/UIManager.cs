using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{

    public string sceneToLoad;
    public GameObject cursor;
    public GameObject[] HeartsLife;
    public Slider UltSlider;
    // Start is called before the first frame update
    public static UIManager Instance;
    float maxUltSlider = 100f;
    [SerializeField]
    private GameObject GameOverPanel;
    private void Awake()
    {
        
        Instance = this;
    }
    private void Start()
    {
        UltSlider.maxValue = GameManager.Instance.maxUltimateValue;
    }

    public void GameOver()
    { GameOverPanel.SetActive(true);
        Time.timeScale = 0;
       
    }

    public void PlayAgain()
    {
        Time.timeScale = 1 ;
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
    }

    // Update is called once per frame

}
