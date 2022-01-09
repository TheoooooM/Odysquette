using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

public class HealthPlayer : MonoBehaviour {
    public Rigidbody2D rb;
    [SerializeField] private CameraShake cameraShake;

    [SerializeField] public int maxHealth;
    [SerializeField] public int healthPlayer;
    private int lastHealth = 0;
    
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
        lastHealth = healthPlayer;
        
        rb = GetComponent<Rigidbody2D>();
        if (UIManager.Instance != null) {
            for (int i = 0; i < healthPlayer / 2; i++) {
         
                UIManager.Instance._HeartsLife.Add(UIManager.Instance.HeartsLifes[i]);
                UIManager.Instance._HeartsLife[i].gameObject.SetActive(true);
            }

            if (healthPlayer % 2 == 1) {
                UIManager.Instance._HeartsLife.Add(UIManager.Instance.HeartsLifes[UIManager.Instance._HeartsLife.Count]);
                UIManager.Instance._HeartsLife[UIManager.Instance._HeartsLife.Count - 1].gameObject.SetActive(true);
                //UIManager.Instance._HeartsLife[UIManager.Instance._HeartsLife.Count - 1].setHalf();
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
            StartCoroutine(UpdateLife());
            
            //foreach (Hearth hearth in UIManager.Instance._HeartsLife) hearth.LifeUpdate();
            
            for (int i = UIManager.Instance._HeartsLife.Count - 1; i > -1; i--) {
                spriteRenderer.material.SetFloat("_HitTime", Time.time);
                if(GameManager.Instance != null && GameManager.Instance.strawSprite != null) GameManager.Instance.strawSprite.material.SetFloat("_HitTime", Time.time);
                isInvincible = true;
            }
        }
    }

    /// <summary>
    /// Update the life
    /// </summary>
    private IEnumerator UpdateLife() {
        int lifeChange = healthPlayer - lastHealth;
        int lifeChangeValueTo1 = (lifeChange > 0 ? 1 : -1);
        
        for (int i = 0; i < Mathf.Abs(lifeChange); i++) {
            int oldHeartImg = Mathf.CeilToInt((lastHealth / 2f) - 1);
            int newHeartImg = Mathf.CeilToInt(((lastHealth + (1 * lifeChangeValueTo1)) / 2f) - 1);
            Debug.Log(oldHeartImg + " " + newHeartImg);
            
            //int newHeartImg = Mathf.CeilToInt((healthPlayer / 2f) - 1);

            if (oldHeartImg == newHeartImg) {
                UIManager.Instance._HeartsLife[newHeartImg].GetComponent<Animator>().SetFloat("lifeValue", UIManager.Instance._HeartsLife[newHeartImg].GetComponent<Animator>().GetFloat("lifeValue") + (0.5f * lifeChangeValueTo1));
                for (int j = 0; j < 3; j++) {
                    if(j != newHeartImg) UIManager.Instance._HeartsLife[j].GetComponent<Animator>().SetTrigger("UpdateLife");
                }
                lastHealth += 1 * lifeChangeValueTo1;
            }
            else {
                if (lifeChange > 0) {
                    UIManager.Instance._HeartsLife[newHeartImg].GetComponent<Animator>().SetFloat("lifeValue", UIManager.Instance._HeartsLife[newHeartImg].GetComponent<Animator>().GetFloat("lifeValue") + (0.5f * lifeChangeValueTo1));
                    for (int j = 0; j < 3; j++) {
                        if(j != newHeartImg) UIManager.Instance._HeartsLife[j].GetComponent<Animator>().SetTrigger("UpdateLife");
                    }
                }
                else {
                    UIManager.Instance._HeartsLife[oldHeartImg].GetComponent<Animator>().SetFloat("lifeValue", UIManager.Instance._HeartsLife[oldHeartImg].GetComponent<Animator>().GetFloat("lifeValue") + (0.5f * lifeChangeValueTo1));
                    for (int j = 0; j < 3; j++) {
                        if(j != oldHeartImg) UIManager.Instance._HeartsLife[j].GetComponent<Animator>().SetTrigger("UpdateLife");
                    }
                }
                
                lastHealth += 1 * lifeChangeValueTo1;
            }

            yield return new WaitForSeconds(2);
        }
    }


    /// <summary>
    /// Heal the player
    /// </summary>
    /// <param name="heal"></param>
    public void GiveHealthPlayer(int heal) {
        healthPlayer = Mathf.Clamp(healthPlayer + heal, 0, 6);

        StartCoroutine(UpdateLife());
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