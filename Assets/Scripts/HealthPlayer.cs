using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HealthPlayer : MonoBehaviour {
    public Rigidbody2D rb;
    [SerializeField] private CameraShake cameraShake;

    [SerializeField] public int maxHealth;
    [SerializeField] public int healthPlayer;
    [SerializeField] private float timeInvincible;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private bool isInvincible;
    [SerializeField] private float timerInvincible;

    // Start is called before the first frame update
    public static HealthPlayer Instance;
    public Playercontroller playerController;
    public bool isGameOver;

    private void Awake() {
        Instance = this;
        if (Camera.main != null) cameraShake = Camera.main.GetComponent<CameraShake>();
    }

    private void Start() {
        AddCommandConsole();
        
        playerController = GetComponent<Playercontroller>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (NeverDestroy.Instance != null) healthPlayer = NeverDestroy.Instance.life;
        else healthPlayer = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        if (UIManager.Instance != null) {
            for (int i = 0; i < healthPlayer / 2; i++) {
         
                UIManager.Instance._HeartsLife.Add(UIManager.Instance.HeartsLifes[i]);
                UIManager.Instance._HeartsLife[i].gameObject.SetActive(true);
            }

            if (healthPlayer % 2 == 1) {
                UIManager.Instance._HeartsLife.Add(UIManager.Instance.HeartsLifes[UIManager.Instance._HeartsLife.Count]);
                UIManager.Instance._HeartsLife[UIManager.Instance._HeartsLife.Count - 1].gameObject.SetActive(true);
                UIManager.Instance._HeartsLife[UIManager.Instance._HeartsLife.Count - 1].setHalf();
            }

            UIManager.Instance._HeartsLife[UIManager.Instance._HeartsLife.Count - 1].currentHearth = true;
        }
    }

    /// <summary>
    /// Add the command to the console
    /// </summary>
    private void AddCommandConsole() {
        CommandConsole REGEN = new CommandConsole("life", "life : Take (<0) or Give (>0) life to the player", new List<CommandClass>() {new CommandClass(typeof(int))}, (value) => {
            int life = int.Parse(value[0]);
            
            if(life > 0) GiveHealthPlayer(life);
            else if(life < 0) TakeDamagePlayer(-life);
        });
        
        CommandConsoleRuntime.Instance.AddCommand(REGEN);
    }
    
    private void Update() {
        if (isInvincible) {
            if (timerInvincible > timeInvincible) {
                timerInvincible = 0;
                isInvincible = false;
                spriteRenderer.color = Color.white;
            }
            else {
                timerInvincible += Time.deltaTime;

                //spriteRenderer.color = Color.red;
            }
        }
    }

    /// <summary>
    /// Deal damage to the player
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamagePlayer(int damage) {
        if (!isInvincible) {
            if (healthPlayer - damage <= 0) OnDeathPlayer();
            
            AudioManager.Instance.PlayPlayerSound(AudioManager.PlayerSoundEnum.Damage);
            healthPlayer -= damage;
            if(cameraShake != null) cameraShake.CreateCameraShake(.15f, .4f);
            
            if (UIManager.Instance == null) return;
            foreach (Hearth hearth in UIManager.Instance._HeartsLife) hearth.LifeUpdate();
            
            for (int i = UIManager.Instance._HeartsLife.Count - 1; i > -1; i--) {
                spriteRenderer.material.SetFloat("_HitTime", Time.time);
                if(GameManager.Instance != null && GameManager.Instance.strawSprite != null) GameManager.Instance.strawSprite.material.SetFloat("_HitTime", Time.time);
                isInvincible = true;
            }
        }
    }

    /// <summary>
    /// Heal the player
    /// </summary>
    /// <param name="heal"></param>
    public void GiveHealthPlayer(int heal) {
        healthPlayer = Mathf.Clamp(healthPlayer + heal, 0, 6);
        
        for (int i = 0; i < healthPlayer; i++) {
            if (i <= healthPlayer) UIManager.Instance.HeartsLifes[i].gameObject.SetActive(true);
        }
    }

    private void OnDeathPlayer()
    {
        if(NeverDestroy.Instance.minute != 0f)GameManager.Instance.Score = GameManager.Instance.Score * (NeverDestroy.Instance.minute/20);  
        GameManager.Instance.SetND();
        if (GameManager.Instance != null) GameManager.Instance.enabled = false;
        gameObject.SetActive(false);
        if (UIManager.Instance != null) UIManager.Instance.GameOver();
        AudioManager.Instance.PlayPlayerSound(AudioManager.PlayerSoundEnum.Death);
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("ShieldEnemy")) TakeDamagePlayer(1);
    }
}