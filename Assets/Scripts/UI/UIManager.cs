using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class UIManager : MonoBehaviour {
    #region VARIABLES
    public string sceneToLoad;
    [SerializeField] private bool isHUB = false;
    public GameObject cursor;
    public List<GameObject> HeartsLifes;
    [Space] 
    [SerializeField] private Image ultimateImg = null;
    [SerializeField] private Animator ultimateAnim = null;
    public float ultimateValue = 0;
    [Space] 
    [SerializeField] private CanvasGroup informationPanel = null;
    private bool showingInfoPanel;
    [Space] 
    public  Image backgroundUltimateUI;
    public static UIManager Instance;
    float maxUltSlider = 100f;
    [SerializeField] private CustomSliderUI sfxSlider = null;
    [SerializeField] private CustomSliderUI musicSlider = null;
    public CustomSliderUI SfxSlider => sfxSlider;
    public CustomSliderUI MusicSlider => musicSlider;
    [Space] 
    [SerializeField] private UpdateCanvasInfo informationCanvasUI = null;
    [SerializeField] private float animationInterfacValue = 0.05f;
    
    [Header("----Game Over----")] 
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private TextMeshProUGUI GameOverText;
    [SerializeField] private GameObject gameOverFirstButton;
    public GameObject endSplash;
    public TMP_InputField PlayerName;
    public TextMeshProUGUI totalScoreText;
    [Space] 
    [SerializeField] private LeaderBoard leaderboard;
    [SerializeField] private TextMeshProUGUI[] finalScoreTxt;
    [SerializeField] private TextMeshProUGUI[] finalNameTxt;
    
    
    [Header("----Pause----")] 
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] public GameObject inGameMenu;
    [SerializeField] private GameObject pauseFirstButton;
    public GameObject PauseMenu => pauseMenu;

    [Header("----Ressources & Score----")] 
    public TextMeshProUGUI ressourceText;
    public TextMeshProUGUI scoreText;
    
    [Header("----Timer----")] 
    public TextMeshProUGUI Timer;
    [SerializeField] private Image timerImg = null;
    [SerializeField] private Sprite chronoSpirte = null;
    [SerializeField] private Sprite pauseSprite = null;
    [Header("----Generation----")] public GameObject LoadingScreen;

    private PlayerMapping input;
    #endregion VARIABLES
    
    private void Awake()
    {
        input = new PlayerMapping();
        input.Interface.Enable();
        input.Interface.Select.performed += SelectOnperformed;
        input.Interface.Select.canceled += context => showingInfoPanel = false;
        Instance = this;
        GameOverPanel.SetActive(false);
        
    }

    private void SelectOnperformed(InputAction.CallbackContext obj) {
        informationCanvasUI.UpdateData();
        showingInfoPanel = true;
    }

    private void Start() {
        CommandConsole RESTART = new CommandConsole("restart", "restart : Restart all the game", null, (_) => { PlayAgain(); });
        CommandConsoleRuntime.Instance.AddCommand(RESTART);
        
        CommandConsole RESETSCORE = new CommandConsole("resetScore", "resetscore : reset all the saves score", null, (_) => { ResetPlayerPrefsLeaderboard(); });
        CommandConsoleRuntime.Instance.AddCommand(RESETSCORE);
        
        pauseMenu.SetActive(false);
        leaderboard.gameObject.SetActive(false);

        inGameMenu.GetComponent<CanvasGroup>().alpha = !isHUB ? 1 : 0;
        informationPanel.alpha = 0;
    }

    /// <summary>
    /// update the loading bar
    /// </summary>
    private void Update() {
        if (inGameMenu.activeSelf) {
            UltimateImageData data = ultimateImg.GetComponent<UltimateImageData>();
            ultimateAnim.SetInteger("UltimateProcent", (int) (ultimateValue * 100));
            float posY = Mathf.Clamp(data.MinPosY + Mathf.Abs(data.MinPosY - data.MaxPosY) * ultimateValue, data.MinPosY, data.MaxPosY);
            float height = Mathf.Clamp((data.MaxHeight - data.MinHeight) * ultimateValue, data.MinHeight, data.MaxHeight);
            ultimateImg.GetComponent<RectTransform>().localPosition = new Vector3(ultimateImg.GetComponent<RectTransform>().localPosition.x, Mathf.Lerp( ultimateImg.GetComponent<RectTransform>().localPosition.y , posY, Time.deltaTime), ultimateImg.GetComponent<RectTransform>().localPosition.z);
            ultimateImg.GetComponent<RectTransform>().sizeDelta = new Vector2(ultimateImg.GetComponent<RectTransform>().sizeDelta.x,  Mathf.Lerp( ultimateImg.GetComponent<RectTransform>().sizeDelta.y , height, Time.deltaTime));

            
            informationPanel.alpha += showingInfoPanel ? animationInterfacValue * Time.deltaTime : -animationInterfacValue * Time.deltaTime;
        }
    }

    /// <summary>
    /// Pause the game
    /// </summary>
    public void Pause() {
        if ((Generation.Instance != null && !Generation.Instance.endGeneration)) return; 
        inGameMenu.GetComponent<CanvasGroup>().alpha = 0;
        cursor.SetActive(false);
        pauseMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
        sfxSlider.OpenMethod();
        musicSlider.OpenMethod();
        Time.timeScale = 0f;
        GameManager.Instance.gameIsPause = true;
    }
    
    /// <summary>
    /// Unpause the game
    /// </summary>
    public void Unpause() {
        Time.timeScale = 1f;
        if(!isHUB) inGameMenu.GetComponent<CanvasGroup>().alpha = 1;
        cursor.SetActive(true);
        pauseMenu.SetActive(false);
        GameManager.Instance.gameIsPause = false;
    }

    #region Score
    public void SubmitScore() {
        leaderboard.UpdateArray();
        leaderboard.SetScore(PlayerName.text, NeverDestroy.Instance.Score);
    }
    
    public void setLeaderboard(){
        leaderboard.UpdateArray();
        for (int i = 0; i < 9; i++) {
            finalNameTxt[i].text = leaderboard.nameArray[i];
            finalScoreTxt[i].text = leaderboard.scoreArray[i].ToString();
        }
    }

    /// <summary>
    /// Quit the leaderboard when press resume
    /// </summary>
    public void QuitLeaderboard() {
        leaderboard.gameObject.SetActive(false);
    }
    #endregion Score

    /// <summary>
    /// Show game over panel
    /// </summary>
    public void GameOver(bool end = false)
    {
    /*
        if (end) GameOverText.text = "YOU WIN";
        else GameOverText.text = "GAME OVER";
        */
        totalScoreText.text = "Final Score : " + NeverDestroy.Instance.Score;
        GameOverPanel.SetActive(true);
        cursor.SetActive(false);
        inGameMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(gameOverFirstButton);
        //Time.timeScale = 0;
    }

    /// <summary>
    /// Restart the game
    /// </summary>
    public void PlayAgain() {
        Destroy(NeverDestroy.Instance.gameObject);
        Time.timeScale = 1 ;
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
        AudioManager.Instance.PlayUISound(AudioManager.UISoundEnum.OpenSound);
    }
    public void QuitGame()
    {
        AudioManager.Instance.PlayUISound(AudioManager.UISoundEnum.CloseSound);
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
    
    public void ResetPlayerPrefsLeaderboard() {
        for (int i = 0; i < leaderboard.NameArrayString.Length; i++) {
            PlayerPrefs.SetString(leaderboard.NameArrayString[i], "");
            PlayerPrefs.SetInt(leaderboard.ScoreArrayString[i], 0);
        }
    }


    /// <summary>
    /// Disable the in game UI
    /// </summary>
    public void DisableUI() {
        inGameMenu.SetActive(false);
    }

    // Update is called once per frame

}
