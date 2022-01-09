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
    [Space] 
    [SerializeField] private Image ultimateImg = null;
    [SerializeField] private Animator ultimateAnim = null;
    [HideInInspector] public float ultimateValue = 0;
    [Space] 
    [SerializeField] private CanvasGroup informationPanel = null;
    [Space] 
    public static UIManager Instance;
    float maxUltSlider = 100f;
    
    
    [Header("----Game Over----")] 
    [SerializeField] private GameObject GameOverPanel;
    public TMP_InputField PlayerName;
    public TextMeshProUGUI totalScoreText;
    
    
    [Header("----Pause----")] 
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject inGameMenu;
    public GameObject PauseMenu => pauseMenu;

    [Header("----Ressources & Score----")] 
    public TextMeshProUGUI ressourceText;
    public TextMeshProUGUI scoreText;
    
    [Header("----Timer----")] 
    public TextMeshProUGUI Timer;
    [SerializeField] private Image timerImg = null;
    [SerializeField] private Sprite chronoSpirte = null;
    [SerializeField] private Sprite pauseSprite = null;
    
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
        pauseMenu.SetActive(false);
        if(NeverDestroy.Instance != null) inGameMenu.SetActive(true);
        informationPanel.alpha = 0;
    }

    /// <summary>
    /// update the loading bar
    /// </summary>
    private void Update() {
        if (loadingBar.value < loadingValue) {
            loadingBar.value += loadingBar.maxValue * chargeSpeed * 0.01f;
        }

        UltimateImageData data = ultimateImg.GetComponent<UltimateImageData>();
        ultimateAnim.SetInteger("UltimateProcent", (int) ultimateValue * 100);
        float posY = Mathf.Clamp(data.MinPosY + Mathf.Abs(data.MinPosY - data.MaxPosY) * ultimateValue, data.MinPosY, data.MaxPosY);
        float height = Mathf.Clamp((data.MaxHeight - data.MinHeight) * ultimateValue, data.MinHeight, data.MaxHeight);
        ultimateImg.GetComponent<RectTransform>().localPosition = new Vector3(ultimateImg.GetComponent<RectTransform>().localPosition.x, Mathf.Lerp( ultimateImg.GetComponent<RectTransform>().localPosition.y , posY, Time.deltaTime), ultimateImg.GetComponent<RectTransform>().localPosition.z);
        ultimateImg.GetComponent<RectTransform>().sizeDelta = new Vector2(ultimateImg.GetComponent<RectTransform>().sizeDelta.x,  Mathf.Lerp( ultimateImg.GetComponent<RectTransform>().sizeDelta.y , height, Time.deltaTime));

        informationPanel.alpha += Input.GetKey(KeyCode.LeftAlt) ? .1f : -.1f;
    }

    /// <summary>
    /// Pause the game
    /// </summary>
    public void Pause() {
        inGameMenu.SetActive(false);
        cursor.SetActive(false);
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        GameManager.Instance.gameIsPause = true;
    }
    
    /// <summary>
    /// Unpause the game
    /// </summary>
    public void Unpause() {
        Time.timeScale = 1f;
        inGameMenu.SetActive(true);
        cursor.SetActive(true);
        pauseMenu.SetActive(false);
        GameManager.Instance.gameIsPause = false;
    }

    public void SubmitScoreND(int ID)
    {
        if (PlayerName.text == null) NeverDestroy.Instance.SubmitScore("Player", ID);
        else NeverDestroy.Instance.SubmitScore(PlayerName.text, ID);
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
    
    #region Timer

    public void PauseTimer() {
        timerImg.sprite = pauseSprite;
        Timer.color = Color.gray;
    }

    public void StartTimer() {
        timerImg.sprite = chronoSpirte;
        Timer.color = Color.white;
    }

    #endregion Timer


    // Update is called once per frame

}
