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
    public static UIManager Instance;
    float maxUltSlider = 100f;
    
    
    [Header("----Game Over----")] 
    [SerializeField] private GameObject GameOverPanel;
    public TMP_InputField PlayerName;
    public TextMeshProUGUI totalScoreText;
    [Space] 
    [SerializeField] private LeaderBoard leaderboard;
    [SerializeField] private TextMeshProUGUI[] finalScoreText;
    
    
    [Header("----Pause----")] 
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject main;
    [SerializeField] private GameObject option;
    public GameObject PauseMenu => pauseMenu;

    [Header("----Ressources & Score----")] 
    public TextMeshProUGUI ressourceText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI Timer;
    
    
    
    
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
        pauseMenu.SetActive(false);
        leaderboard.gameObject.SetActive(false);
    }

    /// <summary>
    /// update the loading bar
    /// </summary>
    private void Update() {
        if (loadingBar.value < loadingValue) {
            loadingBar.value += loadingBar.maxValue * chargeSpeed * 0.01f;
        }

        /*
         * Open the pause menu is now in the commandConsoleRuntime
         */
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        main.SetActive(true);
        option.SetActive(false);
        Time.timeScale = 0f;
        GameManager.Instance.gameIsPause = true;
    }

    
    public void OpenOption() => option.SetActive(true);
    public void Unpause()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GameManager.Instance.gameIsPause = false;
    }

    public void SubmitScore()
    {
        leaderboard.SetScore(PlayerName.text, NeverDestroy.Instance.Score);
    }
    
    public void setLeaderboard()
    {
        leaderboard.UpdateArray();
        for (int i = 0; i < 5; i++)
        {
           finalScoreText[i].text = ((i + 1) + ". " + leaderboard.nameArray[i] + " . . . . . . . . . . . . . . . . . . . . . . . . . . . . " + leaderboard.scoreArray[i]);
        }
    }

    /// <summary>
    /// Show game over panel
    /// </summary>
    public void GameOver()
    {
        totalScoreText.text = "Final Scote : " + NeverDestroy.Instance.Score;
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
    public void QuitGame()
    {
        Application.Quit();
    }
    

    // Update is called once per frame

}
