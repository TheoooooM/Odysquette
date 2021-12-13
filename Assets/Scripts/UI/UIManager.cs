using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{

    public string sceneToLoad;
    public GameObject cursor;
    public List<Hearth> HeartsLifes;
    public List<Hearth> _HeartsLife;
    public Slider UltSlider;
    // Start is called before the first frame update
    public static UIManager Instance;
    float maxUltSlider = 100f;
    
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private GameObject pauseMenu;

    [Header("----Ressources----")] 
    public TextMeshProUGUI ressourceText;
    
    
    [Header("----Generation----")] 
    public GameObject LoadingScreen;
    public Slider loadingBar;
    public float chargeSpeed = 2.5f;
    [HideInInspector] public float loadingValue;
    
    
    private void Awake()
    {
        Instance = this;
        GameOverPanel.SetActive(false);
    }
    private void Start()
    {
        UltSlider.maxValue = GameManager.Instance.maxUltimateValue;
    }

    private void FixedUpdate()
    {
        if (loadingBar.value < loadingValue)
        {
            loadingBar.value += loadingBar.maxValue * chargeSpeed*0.01f;
        }
    }


    public void GameOver()
    { 
        GameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void PlayAgain()
    {
        Destroy(NeverDestroy.Instance.gameObject);
        Time.timeScale = 1 ;
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
    }

    // Update is called once per frame

}
