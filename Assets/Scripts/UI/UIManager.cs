using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour {
    #region VARIABLES
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
    #endregion VARIABLES
    
    private void Awake()
    {
        Instance = this;
        GameOverPanel.SetActive(false);
    }
    private void Start() {
        CommandConsole RESTART = new CommandConsole("restart", "restart : Restart all the game", null, (_) => { PlayAgain(); });
        CommandConsoleRuntime.Instance.AddCommand(RESTART);

        UltSlider.maxValue = GameManager.Instance.maxUltimateValue;
    }

    /// <summary>
    /// update the loading bar
    /// </summary>
    private void Update() {
        if (loadingBar.value < loadingValue) {
            loadingBar.value += loadingBar.maxValue * chargeSpeed*0.01f;
        }
    }

    /// <summary>
    /// Show game over panel
    /// </summary>
    public void GameOver()
    { 
        GameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    /// <summary>
    /// Restart the game
    /// </summary>
    public void PlayAgain()
    {
        Destroy(NeverDestroy.Instance.gameObject);
        Time.timeScale = 1 ;
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
    }
}
